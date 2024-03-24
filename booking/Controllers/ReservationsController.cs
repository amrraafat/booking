using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.IO;
using Microsoft.Extensions.Localization;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Header;
using Microsoft.Data.SqlClient;
using Rotativa.AspNetCore;
using Infarstuructre.Helper;

namespace booking.Controllers
{
    public class ReservationsController : Controller
    {
        protected readonly BookingDbContext _context;
        private readonly IStringLocalizer<ReservationsController> _localizer;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        public ReservationsController(BookingDbContext Context, IWebHostEnvironment webHostEnvironment, IStringLocalizer<ReservationsController> localizer)
        {
            _context = Context;
            _localizer = localizer;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(Reservation reservationpag)
        {
            List<ReservationViewModel> viewModelList = new List<ReservationViewModel>();
            List<Reservation> reservations = new List<Reservation>();
            reservations = await _context.Reservations.Where(r=>r.IsDeleted == false).ToListAsync();
            foreach (var reservation in reservations)
            {
                Hotel hotel = await _context.Hotels.FindAsync(reservation.HotelId);
                Package package = await _context.Packages.FindAsync(reservation.PackageId);
                Customer customer = await _context.Customers.FindAsync(reservation.CustomerId);
                ReservationViewModel viewModel = new ReservationViewModel
                {
                    reservation = reservation,
                    hotel = hotel,
                    package = package,
                    customer = customer
                };
                viewModelList.Add(viewModel);
            }
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;
            ViewBag.currentCulture = requestCulture.Culture.Name;

            ViewBag.Reservation = reservationpag;
            return View(viewModelList);
        }
        public async Task<IActionResult> test()
        {
           

            return View();
        }


        public async Task<IActionResult> Create(int hotelId=0,int packageId=0)
        {
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;
            var hotels = _context.Hotels.ToList();
            var packages = _context.Packages.ToList();
            var customers = _context.Customers.ToList();

            if (requestCulture.Culture.Name == "en")
            {
                ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameSL");
            }
            else
            {
                ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameFL");
            }
            ViewBag.PackageList = new SelectList(packages, "PackageId", "PackageName");
            ViewBag.CustomerList = customers.ToList();
            ViewBag.packageId = packageId.ToString();
            ViewBag.hotelId = hotelId.ToString();

            return View();
        }
        [HttpGet]
        public IActionResult GetPackagesByHotelId(int hotelId)
        {
            var packages = new List<Package>();
            if (hotelId > 0)
            {
                packages = _context.Packages.Where(p => p.HotelId == hotelId).ToList();
            }
            else
            {
                packages = _context.Packages.ToList();
            }
            return Json(packages);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ReservationViewModel viewModel)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string loggedInUserName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(loggedInUserId) || string.IsNullOrEmpty(loggedInUserName))
            {
                return RedirectToAction("Logout", "Accounts");
            }
            Reservation reservation = new Reservation
            {
                EmployeeId = loggedInUserId,
                HotelId = viewModel.reservation.HotelId,
                PackageId = viewModel.reservation.PackageId,
                CustomerId = viewModel.reservation.CustomerId,
                AdultNo = viewModel.reservation.AdultNo,
                KidNo = viewModel.reservation.KidNo,
                ReservationDateTime = DateTime.UtcNow,
                TotalPrice = viewModel.reservation.TotalPrice,
                Discount = viewModel.reservation.Discount,
                Paid = viewModel.reservation.Paid,
                Remain = viewModel.reservation.Remain,
                LastModify = DateTime.UtcNow,
                UserName = loggedInUserName,
                IsDeleted = false,
                DeleteReason = ""
            };
            _context.Add(reservation);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult GetPackageData(int packageId)
        {
            return Json(_context.Packages.Find(packageId));
        }
        //public IActionResult Print()
        //{
        //    var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
        //    var requestCulture = requestCultureFeature?.RequestCulture;
        //    DataTable dataTable = new DataTable();
        //    using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
        //    {
        //        using (var command = connection.CreateCommand())
        //        {
        //            command.CommandText = "PrintReservation";
        //            command.CommandType = CommandType.StoredProcedure;

        //            var param1 = new SqlParameter("@ReservationId", SqlDbType.Int);
        //            param1.Value = 7;
        //            command.Parameters.Add(param1);

        //            var param2 = new SqlParameter("@Lang", SqlDbType.NVarChar);
        //            param2.Value = requestCulture;
        //            command.Parameters.Add(param2);

        //            connection.Open();

        //            using (var reader = command.ExecuteReader())
        //            {
                        
        //                dataTable.Load(reader);
                       
        //            }
        //        }
        //    }

        //    LocalReport localReport = new LocalReport();
        //    localReport.ReportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Report.rdlc");

        //    var rds = new ReportDataSource();
        //    rds.Name = "DeviceSalesReport";
        //    rds.Value = dataTable;

        //    localReport.DataSources.Add(rds);

        //    string mimeType;
        //    string encoding;
        //    string filenameExtension;
        //    string[] streams;
        //    Warning[] warnings;

        //    byte[] renderedBytes = localReport.Render(
        //        "PDF", // PDF, Excel, Word, etc.
        //        null,
        //        out mimeType,
        //        out encoding,
        //        out filenameExtension,
        //        out streams,
        //        out warnings);
        //    return File(renderedBytes, mimeType);

        //}



        public async Task<IActionResult> Edit(int id)
        {
            Reservation reservation =await _context.Reservations.FindAsync(id);
            Hotel hotel = await _context.Hotels.FindAsync(reservation.HotelId);
            Customer customer = await _context.Customers.FindAsync(reservation.CustomerId);
            Package package = await _context.Packages.FindAsync(reservation.PackageId);
            ReservationViewModel model = new ReservationViewModel
            {
                reservation = reservation,
                hotel = hotel,
                customer = customer,
                package = package,
            };
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;
            var hotels = _context.Hotels.ToList();
            var packages = _context.Packages.ToList();
            var customers = _context.Customers.ToList();

            if (requestCulture.Culture.Name == "en")
            {
                ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameSL");
            }
            else
            {
                ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameFL");
            }
            ViewBag.PackageList = new SelectList(packages, "PackageId", "PackageName");
            ViewBag.CustomerList = customers.ToList();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ReservationViewModel viewModel)
        {
            string loggedInUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            string loggedInUserName = User.FindFirstValue(ClaimTypes.Name);
            if (string.IsNullOrEmpty(loggedInUserId) || string.IsNullOrEmpty(loggedInUserName))
            {
                return RedirectToAction("Logout", "Accounts");
            }
            Reservation reservation = new Reservation
            {
                ReservationId = viewModel.reservation.ReservationId,
                EmployeeId = viewModel.reservation.EmployeeId,
                HotelId = viewModel.reservation.HotelId,
                PackageId = viewModel.reservation.PackageId,
                CustomerId = viewModel.reservation.CustomerId,
                AdultNo = viewModel.reservation.AdultNo,
                KidNo = viewModel.reservation.KidNo,
                ReservationDateTime = viewModel.reservation.ReservationDateTime,
                TotalPrice = viewModel.reservation.TotalPrice,
                Discount = viewModel.reservation.Discount,
                Paid = viewModel.reservation.Paid,
                Remain = viewModel.reservation.Remain,
                LastModify = DateTime.UtcNow,
                UserName = loggedInUserName,
                IsDeleted = false,
                DeleteReason = ""
            };
            _context.Update(reservation);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult DeleteReservation(int id, string reason)
        {
            Reservation reservation = _context.Reservations.Find(id);

            if (reservation == null)
            {
                return NotFound();
            }

            // Set the reason for deletion in the reservation entity
            reservation.DeleteReason = reason;
            reservation.IsDeleted = true; // Assuming you have a property like this to mark soft deletion

            _context.SaveChanges();

            return RedirectToAction("index");
        }

        public IActionResult PrintReservation(int id)
        {
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;

            PrintReservationModel model = new PrintReservationModel();
            using (var connection = new SqlConnection(_context.Database.GetDbConnection().ConnectionString))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "PrintReservation";
                    command.CommandType = CommandType.StoredProcedure;

                    var param1 = new SqlParameter("@ReservationId", SqlDbType.Int);
                    param1.Value = 7;
                    command.Parameters.Add(param1);

                    var param2 = new SqlParameter("@Lang", SqlDbType.NVarChar);
                    param2.Value = requestCulture.Culture.Name;
                    command.Parameters.Add(param2);

                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            model.ReservationDateTime = reader.GetDateTime(reader.GetOrdinal("ReservationDateTime"));
                            model.StartDate = reader.GetDateTime(reader.GetOrdinal("StartDate"));
                            model.HotelName = reader.GetString(reader.GetOrdinal("HotelName"));
                            model.PackageName = reader.GetString(reader.GetOrdinal("PackageName"));
                            model.CustomerName = reader.GetString(reader.GetOrdinal("CustomerName"));
                            model.MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber"));
                            model.AdultNo = reader.GetInt32(reader.GetOrdinal("AdultNo"));
                            model.AdultPrice = (int)reader.GetDecimal(reader.GetOrdinal("AdultPrice"));
                            model.AdultTotal = (int)reader.GetDecimal(reader.GetOrdinal("AdultTotal"));
                            model.KidNo = reader.GetInt32(reader.GetOrdinal("KidNo"));
                            model.KidPrice = (int)reader.GetDecimal(reader.GetOrdinal("KidPrice"));
                            model.KidTotal = (int)reader.GetDecimal(reader.GetOrdinal("KidTotal"));
                            model.TotalInvoice = (int)reader.GetDecimal(reader.GetOrdinal("TotalInvoice"));
                            model.Paid = (int)reader.GetDecimal(reader.GetOrdinal("Paid"));
                            model.Remain = (int)reader.GetDecimal(reader.GetOrdinal("Remain"));
                            model.HotelLocation = reader.GetString(reader.GetOrdinal("HotelLocation"));
                        }
                    }
                    var report = new ViewAsPdf("test", model)
                    {
                        PageMargins = { Left = 5, Bottom = 5, Right = 5, Top = 5 },
                    };
                    return report;
                }
            }
        }
    }
}
