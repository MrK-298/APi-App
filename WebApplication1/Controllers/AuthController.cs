
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApplication1.Data;
using WebApplication1.Data.EF;
using WebApplication1.Data.Model;
using WebApplication1.Function;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly SymmetricSecurityKey _secretKey;
        private readonly UserRepository _userRepository;

        public AuthController(MyDbContext context, UserRepository userRepository)
        {
            _context = context;
            _secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ntqbqhrychczumzisfmojgjtpvpsfgwm"));
            _userRepository = userRepository;
        }


        [HttpPost("Login")]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.userName == model.userName);
            if (user != null)
            {
                var passwordHasher = new PasswordHasher<User>();
                var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.passWord, model.passWord);
                if (passwordVerificationResult == PasswordVerificationResult.Success)
                {
                    var userrole = _context.userRoles.SingleOrDefault(p => p.userId == user.Id);
                    var role = _context.Roles.SingleOrDefault(p=>p.roleId == userrole.roleId);
                    var claims = new List<Claim>
                    {
                        new Claim("Fullname", user.fullName),
                        new Claim("Id", user.Id.ToString()),
                        new Claim("Email", user.Email),
                        new Claim("PhoneNumber", user.phoneNumber ),
                        new Claim("Role", role.roleName)
                    };


                    var token = new JwtSecurityToken(
                        issuer: "https://localhost:7145/swagger",
                        audience: "api", // Hoặc "api/login" tùy theo cấu hình
                        claims: claims,
                        expires: DateTime.UtcNow.AddHours(1),
                        signingCredentials: new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256)
                    );

                    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                    return Ok(new { Token = tokenString, Message = "Login success" });
                }
                else
                {
                    return Ok(new { Message = "Login fail ! try again" });
                }
            }
            else
            {
                return Ok(new { Message = "Login fail ! try again" });
            }
        }
        [HttpPost("Register")]
        public IActionResult Register(RegisterViewModel model)
        {
            if (!_context.Users.Any(u => u.userName == model.userName))
            {
                var user = new User
                {
                    userName = model.userName,
                    passWord = new PasswordHasher<User>().HashPassword(null, model.passWord),
                    Email = model.Email,
                    phoneNumber = model.phoneNumber,
                    fullName = model.fullName,
                    address = model.address,
                };
                SendMail.SendEmail(model.Email, "Xác nhận tài khoản", "Bạn đã xác nhận thành công", "");
                _context.Users.Add(user);
                _context.SaveChanges();
                var user1 = _context.Users.SingleOrDefault(u => u.userName == model.userName);
                var role = _context.Roles.SingleOrDefault(r => r.roleName == "Member");
                if (user != null && role != null)
                {
                    var userRole = new UserRole
                    {
                        userId = user1.Id,
                        roleId = role.roleId
                    };
                    _context.userRoles.Add(userRole);
                    _context.SaveChanges();
                }
                return Ok("Registration successful");
            }

            return BadRequest("Username is already taken");
        }
        [HttpPost("sentCode")]
        public IActionResult SentVerificationCode(ForgotPasswordViewModel model)
        {
            // Tìm người dùng với địa chỉ email trong cơ sở dữ liệu của bạn
            var user = _context.Users.SingleOrDefault(p=>p.Email==model.email);
            if (user == null)
            {
                // Không tìm thấy người dùng với địa chỉ email này
                return NotFound();
            }

            // Tạo và lưu mã xác minh trong cơ sở dữ liệu
            var verificationCode = GenerateVerificationCode.GenerateCode();
            _userRepository.SaveVerificationCode(user.Id, verificationCode);

            SendMail.SendEmail(model.email, "Mã xác minh", verificationCode, "");

            return Ok(new { VerificationCode = verificationCode });
        }
        [HttpPost("resetPassword")]
        public IActionResult ResetPassword(ResetPasswordViewModel model)
        {
            // Tìm người dùng với địa chỉ email trong cơ sở dữ liệu của bạn
            var user = _userRepository.GetUserByEmail(model.email);

            if (user == null)
            {
                // Không tìm thấy người dùng với địa chỉ email này
                return NotFound();
            }

            // Kiểm tra mã xác minh từ người dùng
            if (!_userRepository.VerifyCode(user.Id, model.verificationCode))
            {
                // Mã xác minh không hợp lệ
                return BadRequest("Invalid verification code");
            }

            // Đổi mật khẩu của người dùng
            user.passWord = new PasswordHasher<User>().HashPassword(null, model.newPassword);
            _userRepository.UpdateUser(user);

            // Đổi mật khẩu thành công
            return Ok();
        }
        [HttpPost("DecodeToken")]
        public IActionResult DecodeToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes("ntqbqhrychczumzisfmojgjtpvpsfgwm");

                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = "https://localhost:7145/swagger",
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidAudience = "api",
                    ValidateLifetime = true, // Kiểm tra xem Token còn hiệu lực không
                    ClockSkew = TimeSpan.Zero
                };


                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                var jwtToken = (JwtSecurityToken)validatedToken;

                // Extract information from the token as needed
                var fullname = jwtToken.Claims.First(x => x.Type == "Fullname").Value;
                var userId = jwtToken.Claims.First(x => x.Type == "Id").Value;
                var email = jwtToken.Claims.First(x => x.Type == "Email").Value;
                var phoneNumber = jwtToken.Claims.First(x => x.Type == "PhoneNumber").Value;
                var role = jwtToken.Claims.First(x => x.Type == "Role").Value;

                return Ok(new
                {
                    FullName = fullname,
                    UserId = userId,
                    Email = email,
                    PhoneNumber = phoneNumber,
                    Role = role
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return BadRequest(new { message = "Invalid token" });
            }
        }
        [HttpGet("UserInfo")]
        public IActionResult getUserInfo(int id)
        {
            var user = _context.Users.SingleOrDefault(p=>p.Id == id);
            if(user==null)
            {
                return BadRequest("Not user found");
            }
            return Ok(user);
        }
        [HttpPost("Logout")]
        public IActionResult Logout(String token)
        {
            // Lấy token từ request
            if (!string.IsNullOrEmpty(token))
            {
                Blacklist.Add(token);
                return Ok(new { Message = "Logout success" });
            }
            else
            {
                return BadRequest(new { Message = "Invalid token" });
            }
        }
        [HttpPut("ChangePassword")]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.Id == model.Id);
            if (user == null)
            {
                return BadRequest("Not user found");
            }
            var passwordHasher = new PasswordHasher<User>();
            var passwordVerificationResult = passwordHasher.VerifyHashedPassword(user, user.passWord, model.oldPassword);
            if (passwordVerificationResult == PasswordVerificationResult.Success)
            {
                if(model.newPassword == model.verifyNewPassword)
                {
                    user.passWord = new PasswordHasher<User>().HashPassword(null, model.newPassword);
                    _userRepository.UpdateUser(user);
                    return Ok("Change password successfully");
                }
                return BadRequest("Xác minh mật khẩu sai");
            }
            return BadRequest();
        }
        [HttpPut("ChangeProfile")]
        public IActionResult ChangeProfile(ChangeProfileViewModel model)
        {
            var user = _context.Users.SingleOrDefault(p => p.Id == model.Id);
            if (user == null)
            {
                return BadRequest("Not user found");
            }
            user.Email = model.email;
            user.phoneNumber = model.phoneNumber;
            user.fullName = model.Name;
            _userRepository.UpdateUser(user);
            var userrole = _context.userRoles.SingleOrDefault(p => p.userId == user.Id);
            var role = _context.Roles.SingleOrDefault(p => p.roleId == userrole.roleId);
            var updatedClaims = new List<Claim>
            {
                new Claim("Fullname", user.fullName),
                new Claim("Id", user.Id.ToString()),
                new Claim("Email", user.Email),
                new Claim("PhoneNumber", user.phoneNumber ),
                new Claim("Role", role.roleName)
            };

            var updatedToken = new JwtSecurityToken(
                issuer: "https://localhost:7145/swagger",
                audience: "api",
                claims: updatedClaims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(_secretKey, SecurityAlgorithms.HmacSha256)
            );

            var updatedTokenString = new JwtSecurityTokenHandler().WriteToken(updatedToken);

            // Trả về token mới
            return Ok(new { Token = updatedTokenString, Message = "Profile updated successfully" });
        }
    }
}

