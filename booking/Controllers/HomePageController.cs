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
        public IActionResult Reservation(int hotelId, int packageId)
        {
            return View();
        }

    }
}
