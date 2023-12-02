using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models.View
{
    public class LoginViewModel
    {
            [Required]
            [Display(Name = "Account")]
            public string Account { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [Display(Name = "Nhớ tài khoản?")]
            public bool RememberMe { get; set; }
    }
}
