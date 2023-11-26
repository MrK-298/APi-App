using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.EF
{
        [Table("Trip")]
        public partial class Trip
        {
            [Key]
            [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
            public int Id { get; set; }

            public int UserId { get; set; }

            public string startLocation { get; set; }

            public string endLocation { get; set; }

            [StringLength(50)]
            public string distance { get; set; }

            [StringLength(50)]
            public string time { get; set; }

            public decimal? price { get; set; }

            public DateTime? timeBook { get; set; }

            public DateTime? orderDate { get; set; }

            public string status { get; set; }

            public int? DriverId { get; set; }
            public bool? isPaid { get; set; }
            public string? locationIP { get; set; }


    }
}
