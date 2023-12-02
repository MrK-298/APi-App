using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebApplication1.Data.EF
{
    [Table("User")]
    public partial class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string userName { get; set; }
        [Required]
        [MaxLength(100)]
        public string passWord { get; set; }
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MaxLength(10)]
        [RegularExpression(@"^(0|84)([0-9]{9})$", ErrorMessage = "Xin vui lòng nhập đúng định dạng số điện thoại")]
        public string phoneNumber { get; set; }

        public string? fullName { get; set; }
        public string? address { get; set; }

        public string? Avatar { get; set; }
        public string? imageCCCD { get; set; }
        public string? imageBike { get; set; }
        public bool? emailConfirmed { get; set; }
        public string? licensePlates { get; set; }
        public bool? isDelete { get; set; }
        public int? Point { get; set; }
        public bool? lockOutEndDateUtc { get; set; }

        public string? VerificationCode { get; set; }
        public ICollection<Trip> Trips { get; set; }

    }
}
