using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Xin vui lòng nhập tên của bạn")]
        [Display(Name = "FullName")]
        public string fullName { get; set; }
        [Required(ErrorMessage = "Xin vui lòng nhập địa chỉ của bạn")]
        [Display(Name = "Address")]
        public string address { get; set; }
        [Required]
        [MaxLength(100)]
        public string userName { get; set; }
        [Required]
        [MaxLength(100)]
        public string passWord { get; set; }
        [Required]
        [MaxLength(100)]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Số điện thoại không hợp lệ")]
        public string phoneNumber { get; set; }
    }
}
