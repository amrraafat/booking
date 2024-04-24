using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
            List<Package> packages = await _context.Packages.Where(p=>p.IsDeleted == false).ToListAsync();

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
            var hotels = _context.Hotels.Where(h => h.IsDeleted == false).ToList(); // Fetch the list of hotels from the database
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
                    //StartDate = model.Package.StartDate,
                    //EndDate = model.Package.EndDate,
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
        public IActionResult Edit(int Id)
        {

            PackageViewModel package = new PackageViewModel();
            package.Package = _context.Packages.Find(Id);
            if (package != null)
            {
                var requestCultureFeature = HttpContext.Features.Get<IRequestCultureFeature>();
                var requestCulture = requestCultureFeature?.RequestCulture;
                var hotels = _context.Hotels.Where(h => h.IsDeleted == false).ToList(); // Fetch the list of hotels from the database
                if (requestCulture.Culture.Name == "en")
                {
                    ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameSL");
                }
                else
                {
                    ViewBag.HotelList = new SelectList(hotels, "HotelId", "HotelNameFL");
                }
                return View(package);
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(PackageViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Retrieve the existing package from the database
                var package = await _context.Packages.FindAsync(model.Package.PackageId);

                if (package == null)
                {
                    return NotFound(); // Or handle the case where the package is not found
                }

                // Update the properties of the existing package
                package.PackageName = model.Package.PackageName;
                //package.StartDate = model.Package.StartDate;
                //package.EndDate = model.Package.EndDate;
                package.AdultPrice = model.Package.AdultPrice;
                package.KidPrice = model.Package.KidPrice;
                package.HotelId = model.Package.HotelId;

                // Update the package in the context
                _context.Update(package);

                // Save changes to the database
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the hotel by ID
            Package package = await _context.Packages.FindAsync(id);

            if (package == null)
            {
                return NotFound();
            }
            package.IsDeleted = true;
            _context.Packages.Update(package);
            await _context.SaveChangesAsync();


            return RedirectToAction(nameof(Index));
        }
    }
}
