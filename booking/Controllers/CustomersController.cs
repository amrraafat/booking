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
    public class Customers : Controller
    {
        private readonly BookingDbContext _dbContext;

        public Customers( BookingDbContext context)
        {
            _dbContext = context;
        }
        [Authorize(Roles = "superadmin, admin, user")]
        public IActionResult CreateCustomer()
        {
            return View();
        }
        // -------------------------------------------------------------------Add the new customer
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer([Bind("CustomerId,CustomerName,Gender,Address,NationalId,NationalIdImage")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                 
                _dbContext.Add(customer);
                await _dbContext.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        public IActionResult CurrentCustomer()
        {
            List<Customer> customers = _dbContext.Customers.ToList();

            return View(customers);
        }
    }
}
