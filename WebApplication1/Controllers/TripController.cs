
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.EF;
using WebApplication1.Data.Model;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripController : ControllerBase
    {
        private readonly MyDbContext _context;
        public TripController(MyDbContext context)
        {
            _context = context;
        }
        [HttpPost("Booking")]
        public IActionResult BookingTrip(BookingViewModel model)
        {
            if (model != null) 
            {
                var find = _context.Users.SingleOrDefault(p => p.Id == model.UserId);
                var trip = new Trip
                {
                    UserId = model.UserId,
                    fullName = find.fullName,
                    distance = model.distance,
                    time = model.time,
                    timeBook = DateTime.Now,
                    startLocation = model.startLocation,
                    endLocation = model.endLocation,
                    price = model.price,
                    status = "Chưa nhận",
                    isPaid = false,
                };
                _context.Trips.Add(trip);
                _context.SaveChanges();
                return Ok("Booking successful");
            }
            return BadRequest("Booking failed");
        }
        [HttpGet("GetAllTrips")]
        public IActionResult GetAllTrips()
        {
            var trips = _context.Trips.ToList(); // Lấy tất cả các trip từ CSDL

            if (trips == null || trips.Count == 0)
            {
                return NotFound("Không có Trip nào được tìm thấy");
            }

            return Ok(trips);
        }
        [HttpGet("GetDetailTrip")]
        public IActionResult GetDetailTrips(int id)
        {
            var trips = _context.Trips.SingleOrDefault(p=>p.Id==id); // Lấy tất cả các trip từ CSDL

            if (trips == null)
            {
                return NotFound("Không có Trip nào được tìm thấy");
            }

            return Ok(trips);
        }
        [HttpPost("AcceptTrip")]
        public IActionResult AcceptTrip(AcceptTripViewModel model)
        {
            var trip = _context.Trips.SingleOrDefault(p => p.Id == model.TripId);
            var find = _context.Users.SingleOrDefault(p => p.Id == model.DriverId);
            if (trip == null)
            {
                return NotFound("Không có Trip nào được tìm thấy");
            }
            trip.status = "Đã nhận đơn";
            trip.DriverId = model.DriverId;
            trip.driverName = find.fullName;
            _context.SaveChanges();
            return Ok("Accept Trip successful");
        }
    }
}
