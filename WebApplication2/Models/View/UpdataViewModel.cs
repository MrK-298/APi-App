using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.View
{
    public class UpdataViewModel
    {
        [Display(Name = "Avatar")]
        public string Avatar { get; set; }
        [Required(ErrorMessage = "Xin vui lòng nhập tên của bạn")]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "Xin vui lòng nhập địa chỉ của bạn")]
        [Display(Name = "Address")]
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(0|84)([0-9]{9})$", ErrorMessage = "Xin vui lòng nhập đúng định dạng số điện thoại")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Xin vui lòng nhập địa chỉ của bạn")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
