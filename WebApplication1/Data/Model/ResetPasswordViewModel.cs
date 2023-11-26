using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.Model
{
    public class ResetPasswordViewModel
    {
        [EmailAddress]
        public string email { get; set; }
        public string newPassword { get; set; }

        public string verificationCode { get; set; }
    }
}
