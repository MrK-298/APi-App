using WebApplication1.Data;
using WebApplication1.Data.EF;

namespace WebApplication1.Function
{
    public class UserRepository
    {
        private readonly MyDbContext _dbContext;

        public UserRepository(MyDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public  User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public void SaveVerificationCode(int userId, string verificationCode)
        {
            var user = _dbContext.Users.Find(userId);
            if (user != null)
            {
                user.VerificationCode = verificationCode;
                _dbContext.SaveChanges();
            }
        }

        public bool VerifyCode(int userId, string verificationCode)
        {
            var user = _dbContext.Users.Find(userId);
            return user != null && user.VerificationCode == verificationCode;
        }

        public void UpdateUser(User user)
        {
            var existingUser = _dbContext.Users.Find(user.Id);
            if (existingUser != null)
            {
                existingUser.userName = user.userName;
                existingUser.passWord = user.passWord;
                existingUser.Email = user.Email;
                existingUser.phoneNumber = user.phoneNumber;
                existingUser.Avatar = user.Avatar;
                existingUser.imageCCCD = user.imageCCCD;
                existingUser.imageBike = user.imageBike;
                existingUser.licensePlates = user.licensePlates;
                existingUser.isDelete = user.isDelete;
                existingUser.Point = user.Point;
                existingUser.lockOutEndDateUtc = user.lockOutEndDateUtc;

                _dbContext.SaveChanges();
            }
        }
    }
}
