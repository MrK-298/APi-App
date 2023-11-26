using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.Model
{
    public class ForgotPasswordViewModel
    {
        [EmailAddress]
        public string email { get; set; }
    }
}
