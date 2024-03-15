using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace booking.Controllers
{
    public class PackagesController : Controller
    {
        private readonly BookingDbContext _context;
        public PackagesController(BookingDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            List<PackageViewModel> packageViewModelList = new List<PackageViewModel>();
            List<Package> packages = await _context.Packages.ToListAsync();

            foreach (var package in packages)
            {
                Hotel hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.HotelId == package.HotelId);

                PackageViewModel packageViewModel = new PackageViewModel
                {
                    Package = package,
                    Hotel = hotel
                };

                packageViewModelList.Add(packageViewModel);
            }
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;
            ViewBag.currentCulture = requestCulture.Culture.Name;

            return View(packageViewModelList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
            var requestCulture = requestCultureFeature?.RequestCulture;
            var hotels = _context.Hotels.ToList(); // Fetch the list of hotels from the database
            if (requestCulture.Culture.Name == "en")
            {
                ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameSL");
            }
            else
            {
                ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameFL");
            }
            return View(new PackageViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                var package = new Package
                {
                    PackageName = model.Package.PackageName,
                    StartDate = model.Package.StartDate,
                    EndDate = model.Package.EndDate,
                    AdultPrice = model.Package.AdultPrice,
                    KidPrice = model.Package.KidPrice,
                    HotelId = model.Package.HotelId
                };

                _context.Packages.Add(package);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

    }
}
