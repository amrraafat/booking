using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace booking.Controllers
{
 
    [Authorize]

    public class HomeController : Controller
    {
        private readonly BookingDbContext context;

        public HomeController( BookingDbContext context)
        {
            this.context = context;
        }
        [Authorize(Roles = "superadmin, admin, user")]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer([Bind("CustomerId,CustomerName,Gender,Address,NationalId,NationalIdImage")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                // Add the new customer to the database
                context.Add(customer);
                await context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }
    }
}
