using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace booking.Controllers
{
 
    [Authorize]
    public class Customers : Controller
    {
        private readonly BookingDbContext _dbContext;
        private readonly IStringLocalizer<Customers> _localizer;
  

        public Customers( BookingDbContext context , IStringLocalizer<Customers> localizer)
        {
            _dbContext = context;
            _localizer = localizer;
        }



        [Authorize(Roles = "superadmin, admin, user")]
        public IActionResult CreateCustomer()
        {
            return View();
        }


        // ------------------------------------------------------------------- Add the new customer -----------------------------------------------------\\


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCustomer([Bind("CustomerId,CustomerName,Gender,Address,NationalId,NationalIdImage")] Customer customer)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _dbContext.Add(customer);
                    await _dbContext.SaveChangesAsync();

                    HttpContext.Session.SetString("msgType", "success");
                    HttpContext.Session.SetString("titel", _localizer["lbadded"].Value);
                    HttpContext.Session.SetString("msg", _localizer["lbsddedSuccessfully"].Value);
                }
                else
                {
                    HttpContext.Session.SetString("msgType", "erorr");
                    HttpContext.Session.SetString("titel", _localizer["lbaddedfeild"].Value);
                    HttpContext.Session.SetString("msg", _localizer["lbaddingNotCompleted"].Value);
                }
                return RedirectToAction("CurrentCustomer");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while updating customer information: " + ex.Message;
                HttpContext.Session.SetString("msgType", "erorr");
                HttpContext.Session.SetString("titel", _localizer["lbaddedfeild"].Value);
                HttpContext.Session.SetString("msg", _localizer["lbaddingNotCompleted"].Value);
                return RedirectToAction("CurrentCustomer");
            }

    }
    // ------------------------------------------------------------------- Veiw Current customer -----------------------------------------------------\\

    public IActionResult CurrentCustomer()
        {
            var model = new CustomerViewModel
            {
                customerlist = _dbContext.Customers.OrderBy(x => x.CustomerId).ToList()
            };
            return View(model);
        }

        // ------------------------------------------------------------------- delete Current customer -----------------------------------------------------\\
        [HttpPost]
        public IActionResult DeleteCustomer(int id)
        {
            Customer customer = _dbContext.Customers.Find(id);
           

            if (ModelState.IsValid && customer == null)
            {
                return NotFound(); 
            }

            _dbContext.Customers.Remove(customer);
            _dbContext.SaveChanges();
            return RedirectToAction("CurrentCustomer");
        }
        // ------------------------------------------------------------------- Edit Current customer -----------------------------------------------------\\
        [HttpPost]
        public async Task<IActionResult> EditCustomer(CustomerViewModel updatedCustomer)
        {
            try
            {
               

                var existingCustomer = await _dbContext.Customers.FindAsync(updatedCustomer.customer.CustomerId);
               
                if (ModelState.IsValid && existingCustomer != null)
                {
                    existingCustomer.CustomerName = updatedCustomer.customer.CustomerName;
                    existingCustomer.Gender = updatedCustomer.customer.Gender;
                    existingCustomer.Address = updatedCustomer.customer.Address;
                    existingCustomer.NationalId = updatedCustomer.customer.NationalId;

                    await _dbContext.SaveChangesAsync();

                    HttpContext.Session.SetString("msgType", "success");
                    HttpContext.Session.SetString("titel", _localizer["lbUpated"].Value);
                    HttpContext.Session.SetString("msg", _localizer["lbUpdatedSuccessfully"].Value);
                }
                
                else
                {
                    HttpContext.Session.SetString("msgType", "erorr");
                    HttpContext.Session.SetString("titel", _localizer["lbUpdatefeild"].Value);
                    HttpContext.Session.SetString("msg", _localizer["lbUpdateNotCompleted"].Value);
                }

                return RedirectToAction("CurrentCustomer");
            }
            catch (Exception ex)
            {
               
                TempData["ErrorMessage"] = "An error occurred while updating customer information: " + ex.Message;
                HttpContext.Session.SetString("msgType", "erorr");
                HttpContext.Session.SetString("titel", _localizer["lbUpdatefeild"].Value);
                HttpContext.Session.SetString("msg", _localizer["lbUpdateNotCompleted"].Value);
                return RedirectToAction("CurrentCustomer");
            }
        }


    }

}
