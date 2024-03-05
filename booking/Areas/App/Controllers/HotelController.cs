using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace booking.Areas.App.Controllers
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
            return View(hotel);
        }

        public IActionResult Create()
        {
            return View();
        }

        // POST: Hotels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HotelViewModel hotel)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    if (hotel.file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            hotel.file.CopyTo(ms);
                            var fileByte = ms.ToArray();
                            hotel.hotel.HotelImage = fileByte;
                        }
                    }
                    _context.Add(hotel);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                return View(hotel);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while processing your request. Please try again later.");
                return View(hotel);
            }
        }
    }
}
