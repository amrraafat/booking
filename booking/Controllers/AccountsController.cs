
using Domin.Entity;
using Infarstuructre.Domin;
using Infarstuructre.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.EntityFrameworkCore;



namespace booking.Controllers
{
    
    [Authorize]
    public class Accounts : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<Accounts> _localizer;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BookingDbContext _context;

        public Accounts(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> UserManager , IStringLocalizer<Accounts> localizer, SignInManager<ApplicationUser> signInManager, BookingDbContext context)
        {
            _roleManager = roleManager;
            _userManager = UserManager;
            _localizer = localizer;
            _signInManager = signInManager;
            _context = context;
        }

        [Authorize(Roles = "superadmin")]
        public IActionResult Roles()
        {
           return View(new RoleViweModel
            {
                NewRole = new NewRole(),
                Roles = _roleManager.Roles.OrderBy(x=>x.Name).ToList()
            });
            

        }
        //-------------------------------------------------------------------------- calling Users to Viwe -----------------------------------------------------\\
        [Authorize(Roles = "superadmin")]
        public IActionResult Register()
        {
            var model = new RegisterViweModel
            {
                NewRegister = new NewRegister(),
                Roles = _roleManager.Roles.OrderBy(x => x.Name).ToList(),
                Users = _context.VwUsers.OrderBy(x => x.Role).ToList()
                /*_userManager.Users.OrderBy(x => x.Name).ToList() */
            };
            return View(model);


        }

        //--------------------------------------------------------------------------locliazetion -----------------------------------------------------\\

        public IActionResult SelectLanguage(string culture, string returnURL)
        {
            // Set the selected language in a cookie or other storage
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            // Return a response indicating success
            return LocalRedirect(returnURL);
        }


        //--------------------------------------------------------------------------create New Users -----------------------------------------------------\\
        [HttpPost]
        public async Task <IActionResult> Register( RegisterViweModel model)
        {
            if (ModelState.IsValid)
            {
                var file = HttpContext.Request.Form.Files;
                if (file.Count()>0)
                {
                    string ImageName = Guid.NewGuid().ToString() + Path.GetExtension(file[0].FileName);
                    var fileStream = new FileStream(Path.Combine(@"wwwroot/", Helper.PathSaveImageuser, ImageName), FileMode.Create);
                    file[0].CopyTo(fileStream);
                    model.NewRegister.ImgeUser = ImageName;


                }

                var User = new ApplicationUser
                {
                    Id = model.NewRegister.Id,
                    Name = model.NewRegister.Name,
                    UserName = model.NewRegister.Email,
                    Email = model.NewRegister.Email,
                    ImageUser = model.NewRegister.ImgeUser,
                    ActiveUser = model.NewRegister.AciveUser,
                   


                };
                if (User.Id == null)
                {
                    User.Id = Guid.NewGuid().ToString();
                    var result = await _userManager.CreateAsync(User,model.NewRegister.Password);
                    if (result.Succeeded)
                    {
                        var Role = await _userManager.AddToRoleAsync(User, model.NewRegister.RoleName);
                        if (Role.Succeeded)
                        {
                            HttpContext.Session.SetString("msgType", "success");
                            HttpContext.Session.SetString("titel", _localizer["lbadded"].Value);
                            HttpContext.Session.SetString("msg", _localizer["lbSavedSuccessfully"].Value);

                        }
                        else
                        {
                            HttpContext.Session.SetString("msgType", " erorr");
                            HttpContext.Session.SetString("titel", _localizer["lbMsgDuplicateName"].Value);
                            HttpContext.Session.SetString("msg", _localizer["lbNotSaved"].Value);
                        }

                    }
                    else
                    {
                        HttpContext.Session.SetString("msgType", " erorr");
                        HttpContext.Session.SetString("titel", _localizer["lbMsgDuplicateName"].Value);
                        HttpContext.Session.SetString("msg", _localizer["lbNotSaved"].Value);
                    }
                }
               else
                {
                    var userUpdate = await _userManager.FindByIdAsync(User.Id);

                        userUpdate.Name = model.NewRegister.Name;
                        userUpdate.UserName = model.NewRegister.Email;
                        userUpdate.Email = model.NewRegister.Email;
                        userUpdate.ImageUser = model.NewRegister.ImgeUser;
                        userUpdate.ActiveUser = model.NewRegister.AciveUser;

                        var updateResult = await _userManager.UpdateAsync(userUpdate);

                        if (updateResult.Succeeded)
                        {
                            // Update the role if needed
                            var roles = await _userManager.GetRolesAsync(userUpdate);
                            if (roles.Any())
                            {
                                var removeRoleResult = await _userManager.RemoveFromRolesAsync(userUpdate, roles);
                                if (removeRoleResult.Succeeded)
                                {
                                    var addRoleResult = await _userManager.AddToRoleAsync(userUpdate, model.NewRegister.RoleName);

                                    if (addRoleResult.Succeeded)
                                    {
                                        HttpContext.Session.SetString("msgType", "success");
                                        HttpContext.Session.SetString("titel", "تم التحديث");
                                        HttpContext.Session.SetString("msg", "تم تحديث مستخدم بنجاح");
                                    }
                                    else
                                    {
                                        HttpContext.Session.SetString("msgType", "erorr");
                                        HttpContext.Session.SetString("titel", "خطأ في تحديث الدور");
                                        HttpContext.Session.SetString("msg", "لم يتم تحديث مستخدم بنجاح");
                                    }
                                }
                            }
                        }
                        else
                        {
                            HttpContext.Session.SetString("msgType", "erorr");
                            HttpContext.Session.SetString("titel", "خطأ في تحديث المستخدم");
                            HttpContext.Session.SetString("msg", "لم يتم تحديث مستخدم بنجاح");
                        }
                   
                }

            }
            else
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors);

                // Add the errors to the model state for display in the view
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error.ErrorMessage);
                }

                // Return to the same view with validation errors
                return View(errors);
            }
            return RedirectToAction("Register", "Accounts");
        }
        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            var User = _userManager.Users.FirstOrDefault(x => x.Id == Id);
            if (User.ImageUser== null && User.ImageUser != Guid.Empty.ToString() )
            {
                var PathImage = Path.Combine(@"wwwroot/", Helper.PathImageUser, User.ImageUser);
                if (System.IO.File.Exists(PathImage))
                {
                    System.IO.File.Delete(PathImage);
                }
            }
            if ((await _userManager.DeleteAsync(User)).Succeeded)
            {
                return RedirectToAction("Register", "Accounts");
            }
            return RedirectToAction("Register", "Accounts");
        }

        
        [HttpPost]
        
        public async Task<IActionResult> Roles(RoleViweModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole
                {
                    Id = model.NewRole.RoleId,
                    Name = model.NewRole.RoleName
                };
                if (role.Id == null)
                {
                    role.Id = Guid.NewGuid().ToString();

                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {
                        HttpContext.Session.SetString("msgType", "success");
                        HttpContext.Session.SetString("titel", " تم الحفظ");
                        HttpContext.Session.SetString("msg", "تم مجموعة المستخدم ");

                        return RedirectToAction("Roles");
                    }
                    else
                    {
                        List<IdentityError> errorList = result.Errors.ToList();
                        var errors = string.Join(", ", errorList.Select(e => e.Description));
                        if (result.Errors.Any(x=>x.Code == "DuplicateRoleName"))
                        {
                            HttpContext.Session.SetString("msgType", "erorr");
                            HttpContext.Session.SetString("titel", "اسم المستخدم مستخدم من قبل");
                            HttpContext.Session.SetString("msg", _localizer["lbNotSavedMsgRo"].Value);
                        }
                        else
                        {
                            HttpContext.Session.SetString("msgType", "erorr");
                            HttpContext.Session.SetString("titel", errors);
                            HttpContext.Session.SetString("msg", "لم يتم مجموعة المستخدم ");
                        }                         
                        //return Content(errors);
                        return RedirectToAction("Roles");

                    }
                }
                else
                {
                    var RoleUpdate = await _roleManager.FindByIdAsync(role.Id);
                    RoleUpdate.Id = model.NewRole.RoleId;
                    RoleUpdate.Name = model.NewRole.RoleName;
                    var Result =await _roleManager.UpdateAsync(RoleUpdate);
                    if (Result.Succeeded)
                    {
                        HttpContext.Session.SetString("msgType", "success");
                        HttpContext.Session.SetString("titel", " تم الحفظ");
                        HttpContext.Session.SetString("msg", "تم مجموعة المستخدم ");

                        return RedirectToAction("Roles");
                    }
                    else
                    {
                        HttpContext.Session.SetString("msgType", "erorr");
                        HttpContext.Session.SetString("titel", "لم يتم الحفظ");
                        HttpContext.Session.SetString("msg", "لم يتم مجموعة المستخدم ");
                        return RedirectToAction("Roles");
                    }
                }
            }
            
           
            return View();
        }
      
        public async Task<IActionResult> DeleteRole(string Id)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == Id);
            if ((await _roleManager.DeleteAsync(role)).Succeeded)
                return RedirectToAction(nameof(Roles));

            return RedirectToAction("Roles");
        }


        [Authorize(Roles = "superadmin")]
        public async Task<IActionResult> UpdateRole(string Id, string Name)
        {
            var role = _roleManager.Roles.FirstOrDefault(x => x.Id == Id);
            if ((await _roleManager.DeleteAsync(role)).Succeeded)
            {
                return RedirectToAction("Roles");
            }
            return RedirectToAction("Roles");
        }




        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Result = await _signInManager.PasswordSignInAsync(model.Eamil,
                    model.Password, model.RememberMy, false);
                if (Result.Succeeded)
                    return RedirectToAction("HomePage", "HomePage");
                else
                    ViewBag.ErrorLogin = false;
            }
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        private void SessionMsg(string MsgType, string Title, string Msg)
        {
            HttpContext.Session.SetString(Helper.MsgType, MsgType);
            HttpContext.Session.SetString(Helper.Title, Title);
            HttpContext.Session.SetString(Helper.Msg, Msg);
        }
    }
}
