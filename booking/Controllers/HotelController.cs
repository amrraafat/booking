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

            List<Hotel> hotelList = await _context.Hotels.Where(h=>h.IsDeleted == false).ToListAsync();

            foreach (var hotel in hotelList)
            {
                HotelViewModel hotelViewModel = new HotelViewModel
                {
                    hotel = hotel,
                    ImageFileName = hotel.HotelImage != null ? "hotelImage.jpg" : null 
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
        public IActionResult GetImage(int id)
        {
            var hotel = _context.Hotels.Find(id);
            if (hotel == null || hotel.HotelImage == null)
            {
                return NotFound();
            }

            return File(hotel.HotelImage, "image/jpeg");
        }
        public ActionResult Edit(int id)
        {
            // Retrieve the hotel by ID
            Hotel hotel = new Hotel(); 
            hotel = _context.Hotels.Find(id);
            if (hotel != null)
            {
                HotelViewModel hotelViewModel = new HotelViewModel
                {
                    hotel = hotel,
                    ImageFileName = hotel.HotelImage != null ? "hotelImage.jpg" : null
                };
                var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
                var requestCulture = requestCultureFeature?.RequestCulture;
                ViewBag.currentCulture = requestCulture.Culture.Name;
                return View(hotelViewModel);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHotel(HotelViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                Hotel existingHotel = await _context.Hotels.FindAsync(viewModel.hotel.HotelId);

                if (existingHotel == null)
                {
                    return NotFound();
                }

                existingHotel.HotelNameFL = viewModel.hotel.HotelNameFL;
                existingHotel.HotelNameSL = viewModel.hotel.HotelNameSL;
                existingHotel.HotelRate = viewModel.hotel.HotelRate;
                existingHotel.HotelLocation = viewModel.hotel.HotelLocation;

                if (viewModel.file != null && viewModel.file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await viewModel.file.CopyToAsync(memoryStream);
                        existingHotel.HotelImage = memoryStream.ToArray();
                    }
                }

                _context.Update(existingHotel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the hotel by ID
            Hotel hotel = await _context.Hotels.FindAsync(id);

            if (hotel == null)
            {
                return NotFound();
            }

            List<Package> packages = await _context.Packages
            .Where(p => p.HotelId == id)
            .ToListAsync();
            foreach (var package in packages)
            {
                package.IsDeleted = true;
            }
           
            hotel.IsDeleted = true;
            _context.Packages.UpdateRange(packages);
            _context.Hotels.Update(hotel);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }

    }
}
