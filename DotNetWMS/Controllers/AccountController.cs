﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class for handling the account creation process and logging into the application. This class includes all the procedures necessary for the correct verification of data entered by the user.
    /// </summary>
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the UserManager class to maintain the user account
        /// </summary>
        private readonly UserManager<WMSIdentityUser> _userManager;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the SignInManager class to handle the user's login to the application (also external)
        /// </summary>
        private readonly SignInManager<WMSIdentityUser> _signInManager;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the RoleManager class to support assigning and editing user-assigned roles
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// AccountController's logger implementation
        /// </summary>
        private readonly ILogger<AccountController> _logger;
        /// <summary>
        /// NETCore.MailKit.Core Email Service
        /// </summary>
        private readonly IEmailService _emailService;

        public AccountController(UserManager<WMSIdentityUser> userManager,
            SignInManager<WMSIdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger, DotNetWMSContext context, 
            IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _context = context;
            _emailService = emailService;
        }
        /// <summary>
        /// GET method to handle the registration view
        /// </summary>
        /// <returns>Returns the registration form view</returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        /// <summary>
        /// POST method to handle the completed registration form
        /// </summary>
        /// <param name="model">RegisterViewModel class object whose data will be processed by the Identity Framework</param>
        /// <returns>Returns the view resulting from the processing of user-entered data</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            WMSIdentityUser user;
            Location location;

            if (ModelState.IsValid)
            {


                user = new WMSIdentityUser
                {
                    Name = model.Name,
                    Surname = model.Surname,
                    EmployeeNumber = model.EmployeeNumber,
                    Street = model.Street,
                    ZipCode = model.ZipCode,
                    City = model.City,
                    Email = model.Email,
                    UserName = UserLoginGenerator.GenerateUserLogin(model.Name, model.Surname, model.EmployeeNumber)
                };

                location = new Location()
                {
                    Address = model.Address,
                    Latitude = model.Latitude,
                    Longitude = model.Longitude
                };

                var loc = _context.Locations.FirstOrDefault(l => l.Address == model.Address);

                if (loc == null)
                {
                    _context.Add(location);
                    await _context.SaveChangesAsync();

                    var locId = _context.Locations.FirstOrDefault(l => l.Address == model.Address).Id;
                    user.LocationId = locId;

                }
                else
                {
                    user.LocationId = loc.Id;
                }

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var confirmationLink = Url.Action("ConfirmEmail", "Account",
                        new { userId = user.Id, token = token }, Request.Scheme);

                    if (user.Email != null)
                    {
                        string subject = "Potwierdzenie rejestracji";
                        string message = $"Aby dokończyć rejestrację kliknij w link aktywacyjny -> <a href=\"{confirmationLink}\">Potwierdzam!<a>";

                        await _emailService.SendAsync(user.Email, subject, message, true);
                    }
                    
                    _logger.LogInformation(confirmationLink);

                    if (_userManager.Users.Count() == 1)
                    {
                        await IsDefaultRolesExists();
                        await _userManager.AddToRoleAsync(user, "Admin");
                    }


                    if (_signInManager.IsSignedIn(User) && User.IsInRole("Admin"))
                    {
                        return RedirectToAction("Index", "Employees");
                    }

                    return View("RegisterConfirmation");

                }

                foreach (var error in result.Errors)
                {
                    _logger.LogDebug(error.Description);
                    ModelState.AddModelError(string.Empty, error.Description);
                }

                //else
                //{
                //    ModelState.AddModelError("Email", "Adres mailowy powinien być zapisany w formacie użytkownik@domena np. jankowalski@twojafirma.pl");
                //    return View(model);
                //}

            }

            return View(model);
        }
        /// <summary>
        /// GET/POST method to check if the provided e-mail address was previously used for registration
        /// </summary>
        /// <param name="email">E-mail address to check if it was used during successful registration</param>
        /// <returns>Returns true if e-mail adress does not exists, otherwise - false. Response is serialized to JSON format.</returns>
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            if (!RegexUtilities.IsValidEmail(email))
            {
                return Json($"Adres mailowy powinien być zapisany w formacie użytkownik@domena np. jankowalski@twojafirma.pl");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Json(true);
            }
            else
            {
                return Json($"Konto z powyższym adresem ({email}) już istnieje!");
            }
        }
        /// <summary>
        /// A method that supports the email address confirmation process
        /// </summary>
        /// <param name="userId">User ID provided for confirmation</param>
        /// <param name="token">Security token assigned to the email confirmation command</param>
        /// <returns>Returns a view depending on whether the provided data is valid or not</returns>
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
            {
                return RedirectToAction("index", "home");
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Identyfikator użytkownika {userId} jest nieprawidłowy";
                _logger.LogError($"Identyfikator użytkownika {userId} jest nieprawidłowy");
                return View("NotFound");
            }

            var result = await _userManager.ConfirmEmailAsync(user, token);

            if (result.Succeeded)
            {
                return View();
            }

            ViewBag.ExceptionTitle = "Nie można potwierdzić adresu email!";
            _logger.LogError($"Nie można potwierdzić adresu email!");
            return View("GlobalExceptionHandler");
        }
        /// <summary>
        /// GET method to handle the login view
        /// </summary>
        /// <returns>Returns login page view</returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// POST method to handle and proceeding the completed login form
        /// </summary>
        /// <param name="model">LoginViewModel class object whose data will be processed by the Identity Framework</param>
        /// <param name="returnUrl">Captured url to redirect the user to the previous page after successful login</param>
        /// <returns>Returns a view depending on whether the provided data is valid or not</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Login);

                //If the user exists, but the account has not been activated - return an error. Otherwise, try to log in.
                if (user != null && !user.EmailConfirmed &&
                    (await _userManager.CheckPasswordAsync(user, model.Password)))
                {
                    ModelState.AddModelError(string.Empty, "Konto nie zostało jeszcze aktywowane!");
                    _logger.LogDebug($"Konto nie zostało jeszcze aktywowane");
                    return View(model);
                }

                var result = await _signInManager.PasswordSignInAsync(
                    model.Login, model.Password, model.RememberMe, false);

                //If there are no other accounts in the DB, the user is assigned the administrator role
                if (result.Succeeded)
                {
                    user.LoginCount++;
                    await _userManager.UpdateAsync(user);

                    if (_userManager.Users.Count() == 1)
                    {
                        await IsDefaultRolesExists();
                        if (await _userManager.IsInRoleAsync(user, "Admin") == false)
                        {
                            await _userManager.AddToRoleAsync(user, "Admin");
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
                _logger.LogError($"Nieprawidłowy login lub hasło");
                ModelState.AddModelError(string.Empty, "Nieprawidłowy login lub hasło");
            }

            return View(model);
        }
        /// <summary>
        /// POST method to handle and proceeding logout procedure
        /// </summary>
        /// <returns>After a successful logout, the user is redirected to the home page</returns>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("index", "home");
        }
        /// <summary>
        /// GET method to handle the forgot password view
        /// </summary>
        /// <returns>Returns forgot password view</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        /// <summary>
        /// POST method to handle data entered in the link to reset the password
        /// </summary>
        /// <param name="model">ForgotPasswordViewModel class object which will be handled by UserManager instance</param>
        /// <returns>Returns confirmation of password reset regardless of success (protection against bruteforce attack)</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null && await _userManager.IsEmailConfirmedAsync(user))
                {

                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var passwordResetLink = Url.Action("ResetPassword", "Account",
                            new { email = model.Email, token }, Request.Scheme);

                    string subject = "Żądanie resetu hasła!";
                    string message = $"Aby zresetować hasło kliknij link -> <a href=\"{passwordResetLink}\">Potwierdzam!<a>";

                    await _emailService.SendAsync(user.Email, subject, message, true);

                    _logger.LogInformation(passwordResetLink);

                    return View("ForgotPasswordConfirmation");
                }

                //protection against bruteforce
                return View("ForgotPasswordConfirmation");
            }

            return View(model);
        }
        /// <summary>
        /// GET method to support password reset. This method should be activated after the user clicks the link to set a new password.
        /// </summary>
        /// <param name="token">An application-generated security token</param>
        /// <param name="email">E-mail address in the database</param>
        /// <returns>if succeed, returns the form to establish a new password. Otherwise, it returns the view with the error message.</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {

            if (token == null || email == null)
            {
                _logger.LogError($"Nieprawidłowy token: {token}");
                ModelState.AddModelError(string.Empty, "Nieprawidłowy token!");
            }
            return View();
        }
        /// <summary>
        /// POST method to handle complete form with new password
        /// </summary>
        /// <param name="model">ResetPasswordViewModel class object which will be handled by UserManager instance</param>
        /// <returns>Returns confirmation of password changing regardless of success (protection against bruteforce attack)</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                    if (result.Succeeded)
                    {
                        return View("ResetPasswordConfirmation");
                    }
                    foreach (var error in result.Errors)
                    {
                        _logger.LogDebug(error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View(model);
                }
                //protection against bruteforce
                return View("ResetPasswordConfirmation");
            }
            return View(model);
        }
        /// <summary>
        /// A private method that checks whether the default roles have been deleted from the system. If so, it creates them.
        /// </summary>
        /// <returns>Returns a new IdentityRole instance if it did not exist before</returns>
        private async Task IsDefaultRolesExists()
        {
            if (!_roleManager.Roles.Any(r => r.Name == "Admin"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            }
            if (!_roleManager.Roles.Any(r => r.Name == "Moderator"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Moderator", NormalizedName = "MODERATOR" });
            }
            if (!_roleManager.Roles.Any(r => r.Name == "StandardPlus"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "StandardPlus", NormalizedName = "STANDARDPLUS" });
            }
            if (!_roleManager.Roles.Any(r => r.Name == "Standard"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Standard", NormalizedName = "STANDARD" });
            }
            if (!_roleManager.Roles.Any(r => r.Name == "Kadry"))
            {
                await _roleManager.CreateAsync(new IdentityRole { Name = "Kadry", NormalizedName = "KADRY" });
            }
        }
    }
}