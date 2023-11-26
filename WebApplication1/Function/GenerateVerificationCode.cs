namespace WebApplication1.Function
{
    public class GenerateVerificationCode
    {
        public static string GenerateCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var verificationCode = new string(
                Enumerable.Repeat(chars, 6)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray()
            );
            return verificationCode;
        }
    }
}
