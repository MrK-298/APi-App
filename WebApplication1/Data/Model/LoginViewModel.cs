using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data
{
    public class LoginViewModel
    {
        [Required]
        [MaxLength(100)]
        public string userName { get; set; }
        [Required]
        [MaxLength(100)]
        public string passWord { get; set; }
    }
}
