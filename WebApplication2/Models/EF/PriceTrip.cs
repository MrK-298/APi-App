using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Data.EF
{
    [Table("PriceTrip")]
    public partial class PriceTrip
    {
        [Key]
        public int Id { get; set; }
        public int Price { get; set; }
        public int PriceLow { get; set; }
    }
}
