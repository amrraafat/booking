using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace booking.Controllers
{
    public class HotelController : Controller
    {
        private readonly BookingDbContext _context;
        public HotelController(BookingDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            List<HotelViewModel> hotelViewModelList = new List<HotelViewModel>();

            // Retrieve hotels from the database
            List<Hotel> hotelList = await _context.Hotels.ToListAsync();

            // Convert each Hotel to HotelViewModel
            foreach (var hotel in hotelList)
            {
                HotelViewModel hotelViewModel = new HotelViewModel
                {
                    hotel = hotel,
                    // Pass byte[] HotelImage directly
                    file = hotel.HotelImage != null ? new FormFile(new MemoryStream(hotel.HotelImage), 0, hotel.HotelImage.Length, "HotelImage", "hotelImage.jpg") : null
                };

                hotelViewModelList.Add(hotelViewModel);
            }

            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;
            ViewBag.currentCulture = requestCulture.Culture.Name;

            return View(hotelViewModelList);
        }
        public async Task<IActionResult> Detail(int HotelId)
        {
            Hotel hotel=new Hotel();
            if (HotelId > 0)
            {
                hotel = await _context.Hotels.FirstOrDefaultAsync(h=>h.HotelId == HotelId);
                return View(hotel);
            }
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Hotel hotel = new Hotel
                {
                    HotelNameFL = viewModel.hotel.HotelNameFL,
                    HotelNameSL = viewModel.hotel.HotelNameSL,
                    HotelRate = viewModel.hotel.HotelRate,
                    HotelLocation = viewModel.hotel.HotelLocation
                };

                if (viewModel.file != null && viewModel.file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.file.CopyToAsync(memoryStream);
                        hotel.HotelImage = memoryStream.ToArray();
                    }
                }

                _context.Add(hotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewModel);
        }

    }
}
