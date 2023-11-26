using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Data.Model
{
    public class BookingViewModel
    {
        public int UserId { get; set; }

        public string startLocation { get; set; }

        public string endLocation { get; set; }

        public string distance { get; set; }

        public string time { get; set; }

        public decimal? price { get; set; }

    }
}
