using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetWMS.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<WMSIdentityUser> userManager;
        private readonly SignInManager<WMSIdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<WMSIdentityUser> userManager,
            SignInManager<WMSIdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, confirmationLink);

                    if (userManager.Users.Count() == 1)
                    {
                        await IsDefaultRolesExists();
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                    
                    
                    if (signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("ListOfUsers", "Administration");
                    }

                    ViewBag.ExceptionTitle = "Rejestracja udana!";
                    ViewBag.ExceptionMessage = "Przed zalogowaniem musisz aktywować konto klikając na link wysłany mailem";
                    return View("GlobalExceptionHandler");

                    //else
                    //{
                    //    await signInManager.SignInAsync(user, isPersistent: false);
                    //    return RedirectToAction("index", "home");
                    //}

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
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await userManager.FindByIdAsync(userId);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"Identyfikator użytkownika {userId} jest nieprawidłowy";
                return View("NotFound");
            }

            var result = await userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ExceptionTitle = "Nie można potwierdzić adresu email!";
            return View("GlobalExceptionHandler");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByNameAsync(model.Login);

                if (user != null && !user.EmailConfirmed &&
                    (await userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Konto nie zostało jeszcze aktywowane!");
                    return View(model);
                }

                var result = await signInManager.PasswordSignInAsync(
                    model.Login, model.Password, model.RememberMe, false);


                if (result.Succeeded)
                {
                    if (userManager.Users.Count() == 1)
                    {
                        await IsDefaultRolesExists();
                        if (await userManager.IsInRoleAsync(user, "Admin") == false)
                        {
                            await userManager.AddToRoleAsync(user, "Admin");
                        }
                        
                    }
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
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);
                
                if (user != null && await userManager.IsEmailConfirmedAsync(user))
                {
                   
                    var token = await userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token }, Request.Scheme);

                    logger.Log(LogLevel.Warning, passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                //drugi return ma zabezp. przed bruteforcem
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            
            if (token == null || email == null)
            {
                ModelState.AddModelError(string.Empty, "Nieprawidłowy token!");
            }
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }

                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }

        private async Task IsDefaultRolesExists()
        {
            if (!roleManager.Roles.Any(r => r.Name == "Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }
            if (!roleManager.Roles.Any(r => r.Name == "Moderator"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR" });
            }
            if (!roleManager.Roles.Any(r => r.Name == "StandardPlus"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "StandardPlus", NormalizedName = "STANDARDPLUS" });
            }
            if (!roleManager.Roles.Any(r => r.Name == "Standard"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Standard", NormalizedName = "STANDARD" });
            }
            if (!roleManager.Roles.Any(r => r.Name == "Kadry"))
            {
                await roleManager.CreateAsync(new IdentityRole { Name = "Kadry", NormalizedName = "KADRY" });
            }
        }
    }
}