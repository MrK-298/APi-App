using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.EF;
using X.PagedList;

namespace WebApplication2.Controllers
{
    public class PriceController : Controller
    {
        private readonly MyDbContext db;
        public PriceController(MyDbContext context)
        {
            db = context;
        }
        public ActionResult Index(string SearchText, int? page)
        {
            var pageSize = 10;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<PriceTrip> items = db.PriceTrip.OrderByDescending(x => x.Id);

            var pageIndex = page.HasValue ? Convert.ToInt32(page) : 1;
            items = items.ToPagedList(pageIndex, pageSize);
            ViewBag.PageSize = pageSize;
            ViewBag.Page = page;
            return View(items);
        }
        public ActionResult Edit(int id)
        {
            var item = db.PriceTrip.Find(id);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PriceTrip model)
        {
            if (ModelState.IsValid)
            {
                var price = db.PriceTrip.Find(model.Id);
                price.Price = model.Price;
                price.PriceLow = model.PriceLow;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(model);
        }
    }
}
