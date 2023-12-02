using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Functions;
using WebApplication1.Data.EF;
using WebApplication2.Models.View;
using WebApplication1.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebApplication2.Controllers
{
    public class Account : Controller
    {
        private readonly MyDbContext _context;
        private readonly UserRepository _userRepository;

        public Account(MyDbContext context, UserRepository userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.userName == model.Account);
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            if (model.Account.Contains("@"))
            {
                user = _context.Users.SingleOrDefault(p => p.Email == model.Account);
            }
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.passWord, model.Password);
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    if (user.emailConfirmed == false)
                    {
                        await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                        return View("NotificationEmailConfirm");
                    }
                    else if (user.isDelete == true)
                    {
                        return View("IsDeleted");
                    }
                    else if (user.lockOutEndDateUtc != null)
                    {
                        return View("LockOut");
                    }
                    else
                    {
                        var claims = new List<Claim>
                        {
                            new Claim("Username", user.fullName),
                            new Claim("Id", user.Id.ToString()),
                            new Claim("Email", user.Email),
                            new Claim("PhoneNumber", user.phoneNumber ),
                            new Claim("Image",user.Avatar ?? ""),
                        };
                        var claimsIdentity = new ClaimsIdentity(
                            claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var authProperties = new AuthenticationProperties
                        {
                            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30),
                            IsPersistent = true,
                            AllowRefresh = true,
                            RedirectUri = "/Home/Index"
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);
                    }
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public  ActionResult Register(RegisterViewModel model)
        {
            if (!_context.Users.Any(u => u.userName == model.UserName))
            {
                var user = new User
                {
                    userName = model.UserName,
                    passWord = new PasswordHasher<User>().HashPassword(null, model.Password),
                    Email = model.Email,
                    phoneNumber = model.Phone,
                    fullName = model.FullName,
                    address = model.Address,
                };
                _context.Users.Add(user);
                var result = _context.SaveChanges();
                if (result > 0)
                {
                    var user1 = _context.Users.SingleOrDefault(u => u.userName == model.UserName);
                    var role = _context.Roles.SingleOrDefault(r => r.roleName == "Member");
                    var userRole = new UserRole
                    {
                        userId = user1.Id,
                        roleId = role.roleId
                    };
                    _context.userRoles.Add(userRole);
                    _context.SaveChanges();
                    string code = GenerateVerificationCode.GenerateCode();
                    _userRepository.SaveVerificationCode(user1.Id, code);
                    var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    SendMail.SendEmail(user.Email, "Confirm your account", "Please confirm your account by <a href=\"" + callbackUrl + "\">here", "");
                    return View("NotificationEmailConfirm");
                }
            }
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult ConfirmEmail(int userId, string code)
        {
            var user = _context.Users.SingleOrDefault(p => p.Id == userId);
            if (userId == 0 || code == null)
            {
                return View("Error");
            }
            var result = _userRepository.VerifyCode(userId,code);
            if(result==true)
            {
                user.emailConfirmed = true;
                return View("ConfirmEmail");
            }
            return View("Error");
        }
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LogOff()
        {
            if (User.Identity.IsAuthenticated)
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
