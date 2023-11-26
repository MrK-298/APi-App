using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.EF;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriceController : ControllerBase
    {
        private readonly MyDbContext _context;
        public PriceController(MyDbContext context)
        {
            _context = context;
        }
        [HttpGet("GetPrice")]
        public IActionResult getPrice()
        {
            var priceTrip = _context.PriceTrip.SingleOrDefault();

            if (priceTrip == null)
            {
                return NotFound();
            }

            return Ok(priceTrip);
        }
    }
}
