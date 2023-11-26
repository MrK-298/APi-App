using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApplication1.Data.EF
{
    [Table("Role")]
    public partial class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int roleId { get; set; }
        [Required]
        [MaxLength(20)]
        public string roleName { get; set; }

    }
}
