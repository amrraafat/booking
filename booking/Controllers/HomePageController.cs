using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;
using System.Dynamic;

namespace booking.Controllers
{
    public class HomePageController : Controller
    {
        private readonly BookingDbContext _context;
        public HomePageController(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> HomePage()
        {
            var hotelViewModelList = new List<HomePageViewModel>();

            // Retrieve hotels with their associated packages using a join operation
            //var hotelPackagesQuery =
            //    from hotel in _context.Hotels
            //    join package in _context.Packages on hotel.HotelId equals package.HotelId into packages
            //    select new { Hotel = hotel, Packages = packages };

            // Execute the query asynchronously
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

            // Pass hotel view models to the view
            return View(hotelViewModelList);
        }


    }
}
