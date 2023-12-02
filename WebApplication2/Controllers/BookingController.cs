using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Data.EF;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using X.PagedList;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication2.Controllers
{
    public class BookingController : Controller
    {
        private readonly MyDbContext db;

        public BookingController(MyDbContext context)
        {
            db = context;
        }
        [HttpGet]
        public ActionResult Index(string SearchText, int? page, string filterStatus)
        {
            //var items = db.Trips.OrderByDescending(x => x.OrderDate).ToList();
            string errorMessage = "Không tìm thấy kết quả.";
            ViewBag.filterStatus = filterStatus;
            var pageNumber = page ?? 1;
            var pageSize = 10;
            ViewBag.PageSize = pageSize;
            ViewBag.Page = pageNumber;
            if (page == null)
            {
                page = 1;
            }
            IEnumerable<Trip> items = db.Trips.OrderByDescending(x => x.Id);
            if (filterStatus == "Tất cả") filterStatus = "";
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.fullName.Contains(SearchText) || x.driverName.Contains(SearchText));
            }
            else if (!string.IsNullOrEmpty(filterStatus))
            {
                items = items.Where(x => x.status == filterStatus);
            }
            return View(items.ToPagedList(pageNumber, pageSize));
        }
        [HttpPost]
        public ActionResult Errortrip(int id)
        {
            var trip = db.Trips.Find(id);
            if (trip != null)
            {
                trip.status = "Đã hủy";
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }


        [HttpPost]
        public FileResult ExportToExcel(string SearchText, string filterStatus)
        {
            IEnumerable<Trip> items = db.Trips.OrderByDescending(x => x.Id);
            if (filterStatus == "Tất cả") filterStatus = "";
            if (!string.IsNullOrEmpty(SearchText))
            {
                items = items.Where(x => x.fullName.Contains(SearchText) || x.driverName.Contains(SearchText));
            }
            else if (!string.IsNullOrEmpty(filterStatus))
            {
                items = items.Where(x => x.status == filterStatus);
            }

            // tao tep excel
            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Booking Data");

                // dat tieu de
                worksheet.Cells[1, 1].Value = "ID";
                worksheet.Cells[1, 2].Value = "Tên khách hàng";
                worksheet.Cells[1, 3].Value = "Tên tài xế";
                worksheet.Cells[1, 4].Value = "Trạng Thái";
                worksheet.Cells[1, 5].Value = "Điểm đi";
                worksheet.Cells[1, 6].Value = "Điểm đến";
                worksheet.Cells[1, 7].Value = "Khoảng cách";
                worksheet.Cells[1, 8].Value = "Thời gian";
                worksheet.Cells[1, 9].Value = "Giá";
                worksheet.Cells[1, 10].Value = "Thời gian đặt";
                worksheet.Cells[1, 11].Value = "Thời gian tạo";
                worksheet.Cells[1, 12].Value = "Thanh toán";



                // lay du lieu do vao cot
                int row = 2;
                foreach (var item in items)
                {
                    worksheet.Cells[row, 1].Value = item.Id;
                    worksheet.Cells[row, 2].Value = item.fullName;
                    worksheet.Cells[row, 3].Value = item.driverName;
                    worksheet.Cells[row, 4].Value = item.status;
                    worksheet.Cells[row, 5].Value = item.startLocation;
                    worksheet.Cells[row, 6].Value = item.endLocation;
                    worksheet.Cells[row, 7].Value = item.distance;
                    worksheet.Cells[row, 8].Value = item.time;
                    worksheet.Cells[row, 9].Value = item.price;
                    worksheet.Cells[row, 10].Value = item.timeBook;
                    worksheet.Cells[row, 11].Value = item.orderDate;
                    worksheet.Cells[row, 12].Value = item.isPaid;

                    row++;
                }

                // chỉnh tiêu đề
                using (var range = worksheet.Cells[1, 1, 1, 12])
                {
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                    range.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    range.Style.Font.Bold = true;
                }

                // autofit các cột
                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                //chuyen toi thanh dang byte
                byte[] excelBytes = package.GetAsByteArray();

                // tra ve 1 fiel excel
                return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "booking_data.xlsx");
            }

        }
    }
}
