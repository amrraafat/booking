using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;

namespace booking.Controllers
{
    public class ReservationsController : Controller
    {
        protected readonly BookingDbContext _context;
        public ReservationsController(BookingDbContext Context)
        {
            _context = Context;
        }

        public async Task<IActionResult> Index()
        {
            List<ReservationViewModel> viewModelList = new List<ReservationViewModel>();
            List<Reservation> reservations = new List<Reservation>();
            reservations = await _context.Reservations.ToListAsync();
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
        public async Task<IActionResult> Create()
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
            ViewBag.CustomerList = new SelectList(customers);
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
            Reservation reservation = new Reservation
            {
                EmployeeId = loggedInUserId,
                HotelId = viewModel.reservation.HotelId,
                PackageId = viewModel.reservation.PackageId,
                CustomerId = 7,
                //CustomerId = viewModel.customer.CustomerId,
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
    }
}
