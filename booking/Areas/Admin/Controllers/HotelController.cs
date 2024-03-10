using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace booking.Areas.Admin.Controllers
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
            List<Hotel> hotel = new List<Hotel>();
            hotel = await _context.Hotels.ToListAsync();
            return View("~/Areas/Admin/Views/Hotel/Index.cshtml", hotel); 
        }

        public IActionResult Create()
        {
            return View("~/Areas/Admin/Views/Hotel/Create.cshtml");
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelViewModel hotel)
        {
            if (!ModelState.IsValid)
            {
                return View(hotel);
            }

            try
            {
                var model = MapToHotelModel(hotel);

                if (hotel.file.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        await hotel.file.CopyToAsync(ms);
                        model.HotelImage = ms.ToArray();
                    }
                }

                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError("", "An error occurred while saving to the database.");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
            }

            return View(hotel);
        }

        private Hotel MapToHotelModel(HotelViewModel hotel)
        {
            return new Hotel
            {
                HotelId = hotel.hotel.HotelId,
                HotelNameSL = hotel.hotel.HotelNameSL,
                HotelNameFL = hotel.hotel.HotelNameFL,
                HotelRate = hotel.hotel.HotelRate,
                HotelLocation = hotel.hotel.HotelLocation
            };
        }
    }
}
