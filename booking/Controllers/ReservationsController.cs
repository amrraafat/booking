using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Reporting.WebForms;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;
using System.IO;

namespace booking.Controllers
{
    public class ReservationsController : Controller
    {
        protected readonly BookingDbContext _context;
        protected readonly IWebHostEnvironment _webHostEnvironment;
        public ReservationsController(BookingDbContext Context, IWebHostEnvironment webHostEnvironment)
        {
            _context = Context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
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
            return View(viewModelList);
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
        public IActionResult Print()
        {
            // Step 1: Create DataTable and Populate with Data
            DataTable dataTable = new DataTable("MyDataSet");
            dataTable.Columns.Add("ID", typeof(int));
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Rows.Add(1, "John");
            dataTable.Rows.Add(2, "Alice");
            dataTable.Rows.Add(3, "Bob");

            // Step 2: Set up Report Viewer
            LocalReport localReport = new LocalReport();
            localReport.ReportPath = Path.Combine(_webHostEnvironment.WebRootPath, "Reports", "Report.rdlc");

            // Add dataset to report data sources
            var rds = new ReportDataSource();
            rds.Name = "DeviceSalesReport";
            rds.Value = dataTable;

            localReport.DataSources.Add(rds);

            // Step 3: Render the Report
            string mimeType;
            string encoding;
            string filenameExtension;
            string[] streams;
            Warning[] warnings;

            byte[] renderedBytes = localReport.Render(
                "PDF", // PDF, Excel, Word, etc.
                null,
                out mimeType,
                out encoding,
                out filenameExtension,
                out streams,
                out warnings);

            // Step 4: Return the rendered report as a file
            return File(renderedBytes, mimeType);

        }
    }
}
