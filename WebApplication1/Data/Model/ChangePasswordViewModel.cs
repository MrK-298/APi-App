namespace WebApplication1.Data.Model
{
    public class ChangePasswordViewModel
    {
        public int Id { get; set; }
        public string oldPassword { get; set; }
        public string newPassword { get; set; }
        public string verifyNewPassword { get; set;}
    }
}
