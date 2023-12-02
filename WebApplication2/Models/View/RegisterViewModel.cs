using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.View
{
    public class RegisterViewModel
    {
            [Required(ErrorMessage = "Xin vui lòng nhập tên của bạn")]
            [Display(Name = "FullName")]
            public string FullName { get; set; }
            [Required(ErrorMessage = "Xin vui lòng nhập địa chỉ của bạn")]
            [Display(Name = "Address")]
            public string Address { get; set; }
            [Required(ErrorMessage = "Xin vui lòng nhập địa chỉ Email của bạn")]
            [EmailAddress(ErrorMessage = "Xin vui lòng nhập đúng định dạng địa chỉ email")]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required(ErrorMessage = "Xin vui lòng nhập số điện thoại của bạn")]
            [DataType(DataType.PhoneNumber)]
            [RegularExpression(@"^(0|84)([0-9]{9})$", ErrorMessage = "Xin vui lòng nhập đúng định dạng số điện thoại")]
            public string Phone { get; set; }
            [Required(ErrorMessage = "Xin vui lòng nhập tên tài khoản")]
            [Display(Name = "UserName")]
            public string UserName { get; set; }
            [Required(ErrorMessage = "Xin vui lòng nhập mật khẩu")]
            [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} ký tự.", MinimumLength = 6)]
            [DataType(DataType.Password, ErrorMessage = "Mật khẩu phải có kí tự chữ số, chữ hoa, chữ thường và 1 kí tự đặc biệt")]
            [Display(Name = "Mật Khẩu")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
            public string ConfirmPassword { get; set; }
    }
}
