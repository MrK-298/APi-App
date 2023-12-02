
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.EF
{
    [Table("UserRole")]
    public partial class UserRole
    {
        [Key]
        public int Id { get; set; }

        // Khóa ngoại đến User
        public int userId { get; set; }
        public virtual User user { get; set; }

        // Khóa ngoại đến Role
        public int roleId { get; set; }
        public virtual Role role { get; set; }
    }
}
