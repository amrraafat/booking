using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Dynamic;

namespace booking.Controllers
{
    public class HomePageController : Controller
    {
        private readonly BookingDbContext _context;
        private readonly IStringLocalizer<Customers> _localizer;
        public HomePageController(BookingDbContext context, IStringLocalizer<Customers> localizer)
        {
            _context = context;
            _localizer = localizer;
        }

        public async Task<IActionResult> HomePage(Customer customer)
        {
            var hotelViewModelList = new List<HomePageViewModel>();

            var hotelPackages = await _context.Hotels.ToListAsync();
            
            foreach (var item in hotelPackages)
            {
                var hotel = item;
                List<Package> packages1 = (from p in _context.Packages
                                           where p.HotelId == item.HotelId
                                           select p).ToList();
                // Create a HotelViewModel object
                var hotelViewModel = new HomePageViewModel
                {
                    Hotel = hotel,
                    ImageFileName = hotel.HotelImage != null ? "hotelImage.jpg" : null,
                    Packages = packages1
                };
                
                hotelViewModelList.Add(hotelViewModel);
            }

            ViewBag.Customer = customer;
            // Pass hotel view models to the view
            return View(hotelViewModelList);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer([Bind("CustomerId,CustomerName,Gender,Address,NationalId,NationalIdImage")] Customer customer)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(customer);
                    await _context.SaveChangesAsync();

                    HttpContext.Session.SetString("msgType", "success");
                    HttpContext.Session.SetString("titel", _localizer["lbadded"].Value);
                    HttpContext.Session.SetString("msg", _localizer["lbsddedSuccessfully"].Value);
                }
                else
                {
                    HttpContext.Session.SetString("msgType", "erorr");
                    HttpContext.Session.SetString("titel", _localizer["lbaddedfeild"].Value);
                    HttpContext.Session.SetString("msg", _localizer["lbaddingNotCompleted"].Value);
                }
                return RedirectToAction("HomePage", "HomePage");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating customer information: " + ex.Message;
                HttpContext.Session.SetString("msgType", "erorr");
                HttpContext.Session.SetString("titel", _localizer["lbaddedfeild"].Value);
                HttpContext.Session.SetString("msg", _localizer["lbaddingNotCompleted"].Value);
                return RedirectToAction("HomePage", "HomePage");
            }

        }

        [HttpPost]
        public IActionResult Reservation(int hotelId, int selectedPackageId)
        {
            return View();
        }

    }
}
