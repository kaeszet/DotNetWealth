using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace DotNetWMS.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<WMSIdentityUser> userManager;
        private readonly SignInManager<WMSIdentityUser> signInManager;

        public AccountController(UserManager<WMSIdentityUser> userManager,
            SignInManager<WMSIdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            WMSIdentityUser user;

            if (ModelState.IsValid)
            {
                user = new WMSIdentityUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    EmployeeNumber = model.EmployeeNumber,
                    City = model.City,
                    Email = model.Email
                };
                if (model.Surname.Length < 5 && model.Name.Length < 3)
                {
                    user.UserName = $"{model.Surname}{model.Name}{model.EmployeeNumber.Substring(model.EmployeeNumber.Length - (5 - model.Surname.Length) - (3 - model.Name.Length) - 4)}";
                }
                else if (model.Surname.Length < 5)
                {
                    user.UserName = $"{model.Surname}{model.Name.Substring(0, 3)}{model.EmployeeNumber.Substring(model.EmployeeNumber.Length - (5 - model.Surname.Length) - 4)}";
                }
                else if (model.Name.Length < 3)
                {
                    user.UserName = $"{model.Surname.Substring(0, 5)}{model.Name}{model.EmployeeNumber.Substring(model.EmployeeNumber.Length - (3 - model.Name.Length) - 4)}";
                }
                else
                {
                    user.UserName = $"{model.Surname.Substring(0, 5)}{model.Name.Substring(0, 3)}{model.EmployeeNumber.Substring(model.EmployeeNumber.Length - 4)}";   
                }
                
                var result = await userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    if (userManager.Users.Count() == 1)
                    {
                        IdentityRole identityRole = new IdentityRole { Name = "Admin" };
                        await userManager.AddToRoleAsync(user, identityRole.Name);
                    }
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Konto z powyższym adresem ({email}) już istnieje!");
            }
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(
                    model.Login, model.Password, model.RememberMe, false);

                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        return RedirectToAction("index", "home");
                    }
                    
                }

                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło");
            }

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
    }
}