using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class to operate the administration panel
    /// </summary>
    [Authorize(Roles = "Admin")]
    public class AdministrationController : Controller
    {
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the RoleManager class to support assigning and editing user-assigned roles
        /// </summary>
        private readonly RoleManager<IdentityRole> _roleManager;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the UserManager class to maintain the user account
        /// </summary>
        private readonly UserManager<WMSIdentityUser> _userManager;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<AdministrationController> _logger;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<WMSIdentityUser> userManager, ILogger<AdministrationController> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }
        /// <summary>
        /// GET method returning a view with access denied information
        /// </summary>
        /// <returns>Returns the view with access denied information</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            _logger.LogDebug("Dostęp został wzbroniony");
            return View();
        }
        /// <summary>
        /// GET method returning a view with list of roles
        /// </summary>
        /// <returns>Returns the view with list of created roles</returns>
        [HttpGet]
        public IActionResult ListOfRoles()
        {
            var roles = _roleManager.Roles;
            return View(roles);
        }
        /// <summary>
        /// GET method returning a view with list of users
        /// </summary>
        /// <returns>Returns the view with list of registered users</returns>
        [HttpGet]
        public IActionResult ListOfUsers()
        {
            var users = _userManager.Users;

            return View(users);
        }
        
        /// <summary>
        /// GET method to handle the role edition view
        /// </summary>
        /// <param name="id">Role ID which will be edited</param>
        /// <returns>Returns the view with details of the role with the entered ID or an error if the role was not found</returns>
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {id} nie została odnaleziona!";
                _logger.LogError($"Rola o numerze ID: {id} nie została odnaleziona!");
                return View("NotFound");
            }

            var model = new Admin_EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in _userManager.Users.ToList())
            {
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    model.Users.Add(user.UserName);
                }
            }

            return View(model);
        }
        /// <summary>
        /// POST method to handle the completed role edition form
        /// </summary>
        /// <param name="model">Admin_EditRoleViewModel class object which will be processed by an instance of RoleManager class</param>
        /// <returns>Returns the view resulting from the processing of user-entered data</returns>
        [HttpPost]
        public async Task<IActionResult> EditRole(Admin_EditRoleViewModel model)
        {
            var role = await _roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {model.Id} nie została odnaleziona!";
                _logger.LogError($"Rola o numerze ID: {model.Id} nie została odnaleziona!");
                return View("NotFound");
            }
            if (Admin_DefaultRoles.IsDefaultRole(model.RoleName))
            {
                ViewBag.ErrorTitle = "Podczas edycji roli wystąpił błąd!";
                ViewBag.ErrorMessage = "Nie można edytować domyślnej roli.";
                _logger.LogError($"Nie można edytować domyślnej roli: {model.RoleName}");
                return View("DbExceptionHandler");
            }
            else
            {
                role.Name = model.RoleName;

                var result = await _roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfRoles");
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogDebug(error.Description);
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }
        }
        /// <summary>
        /// POST method to handle the role removal process 
        /// </summary>
        /// <param name="id">Role ID to remove</param>
        /// <returns>If succeed, returns an updated role list. Otherwise - the page with the error message</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {id} nie została odnaleziona!";
                _logger.LogError($"Rola o numerze ID: {id} nie została odnaleziona!");
                return View("NotFound");
            }
            else if (Admin_DefaultRoles.IsDefaultRole(role.Name))
            {
                ViewBag.ErrorTitle = "Podczas usuwania roli wystąpił błąd!";
                ViewBag.ErrorMessage = "Nie można usunąć domyślnej roli.";
                _logger.LogError($"Nie można usunąć domyślnej roli: {role.Name}");
                return View("DbExceptionHandler");
            }
            else
            {
                try
                {
                    var result = await _roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListOfRoles");
                    }

                    foreach (var error in result.Errors)
                    {
                        _logger.LogDebug(error.Description);
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListOfRoles");
                }
                catch (DbUpdateException)
                {

                    ViewBag.ErrorTitle = "Podczas usuwania roli wystąpił błąd!";
                    ViewBag.ErrorMessage = $"Istnieje użytkownik przypisany do tej roli.<br>" +
                        $"Przed usunięciem roli usuń wszystkich pracowników przypisanych do roli.<br>" +
                        $"Po wykonaniu tych czynności ponów próbę.";
                    _logger.LogError("Podczas usuwania roli wystąpił błąd! Istnieje użytkownik przypisany do tej roli.");
                    return View("DbExceptionHandler");
                }
                
            }
        }
        /// <summary>
        /// GET method to handle the view with list of users connected with the role
        /// </summary>
        /// <param name="roleId">The ID of the role whose user list should be displayed</param>
        /// <returns>Returns the view with the list of users assigned to the role</returns>
        [HttpGet]
        public async Task<IActionResult> EditUsersInRole(string roleId)
        {
            ViewBag.roleId = roleId;

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {roleId} nie została odnaleziona!";
                _logger.LogError($"Rola o numerze ID: {roleId} nie została odnaleziona!");
                return View("NotFound");
            }

            var model = new List<Admin_UsersInRoleViewModel>();

            foreach (var user in _userManager.Users.ToList())
            {
                var userRoleViewModel = new Admin_UsersInRoleViewModel
                {
                    UserId = user.Id,
                    FullName = user.FullName,
                    EmployeeNumber = user.EmployeeNumber,
                    UserName = user.UserName
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRoleViewModel.IsSelected = true;
                }
                else
                {
                    userRoleViewModel.IsSelected = false;
                }

                model.Add(userRoleViewModel);
            }

            return View(model);
        }
        /// <summary>
        /// POST method to process changes on the list of users assigned to a given role
        /// </summary>
        /// <param name="model">List of users assigned to the role</param>
        /// <param name="roleId">The ID of the role whose user list will be processed</param>
        /// <returns>Updates input about users in role in DB</returns>
        [HttpPost]
        public async Task<IActionResult> EditUsersInRole(List<Admin_UsersInRoleViewModel> model, string roleId)
        {
            Admin_DefaultRoles defaultRoles = new Admin_DefaultRoles();

            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {roleId} nie została odnaleziona!";
                _logger.LogError($"Rola o numerze ID: {roleId} nie została odnaleziona!");
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await _userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result = null;
                
                if (model[i].IsSelected && !await _userManager.IsInRoleAsync(user, role.Name))
                {
                    if (await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        foreach (var x in _userManager.Users.ToList())
                        {
                            if (await _userManager.IsInRoleAsync(x, "Admin") && x.EmployeeNumber != user.EmployeeNumber)
                            {
                                result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                                result = await _userManager.AddToRoleAsync(user, role.Name);
                                break;
                            }
                        }

                        if (result == null)
                        {
                            ViewBag.ErrorTitle = "Podczas zmiany roli użytkownika wystąpił błąd!";
                            ViewBag.ErrorMessage = "Co najmniej jeden użytkownik musi mieć uprawnienia administratora.";
                            _logger.LogError($"Podczas zmiany roli użytkownika wystąpił błąd! Co najmniej jeden użytkownik musi mieć uprawnienia administratora.");
                            return View("DbExceptionHandler");
                        }
                    }
                    else if (await _userManager.IsInRoleAsync(user, "Kadry") && (role.Name == "Standard" || role.Name == "StandardPlus"))
                    {
                        if (!await _userManager.IsInRoleAsync(user, "Standard") && !await _userManager.IsInRoleAsync(user, "StandardPlus"))
                        {
                            result = await _userManager.AddToRoleAsync(user, role.Name);
                        }

                        if (await _userManager.IsInRoleAsync(user, "Standard") && !await _userManager.IsInRoleAsync(user, "StandardPlus"))
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, "Standard");
                            result = await _userManager.AddToRoleAsync(user, role.Name);
                        }

                        if (await _userManager.IsInRoleAsync(user, "StandardPlus") && !await _userManager.IsInRoleAsync(user, "Standard"))
                        {
                            result = await _userManager.RemoveFromRoleAsync(user, "StandardPlus");
                            result = await _userManager.AddToRoleAsync(user, role.Name);
                        }

                       
                        
                    }
                    else if ((await _userManager.IsInRoleAsync(user, "Standard") || await _userManager.IsInRoleAsync(user, "StandardPlus")) && role.Name == "Kadry")
                    {
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                    else
                    {
                        result = await _userManager.RemoveFromRolesAsync(user, await _userManager.GetRolesAsync(user));
                        result = await _userManager.AddToRoleAsync(user, role.Name);
                    }
                   
                }
                else if (!model[i].IsSelected && await _userManager.IsInRoleAsync(user, role.Name))
                {
                    if (role.Name == "Admin")
                    {
                        var findUser = model.FirstOrDefault(m => m.IsSelected == true);

                        if (findUser == null)
                        {
                            ViewBag.ErrorTitle = "Podczas usuwania użytkownika z roli wystąpił błąd!";
                            ViewBag.ErrorMessage = "Co najmniej jeden użytkownik musi mieć uprawnienia administratora.";
                            _logger.LogError($"Podczas usuwania użytkownika z roli wystąpił błąd! Co najmniej jeden użytkownik musi mieć uprawnienia administratora.");
                            return View("DbExceptionHandler");
                        }
                        
                    }
                    result = await _userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (result.Succeeded)
                {
                    if (i < (model.Count - 1))
                        continue;
                    else
                        return RedirectToAction("EditRole", new { Id = roleId });
                }
            }

            return RedirectToAction("EditRole", new { Id = roleId });
        }
        /// <summary>
        /// GET method to handle the view with form with editable user's data
        /// </summary>
        /// <param name="id">ID of the edited user</param>
        /// <returns>Returns the view with editable user's data</returns>
        [HttpGet]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {id} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {id} nie został odnaleziony!");
                return View("NotFound");
            }

            var userRoles = await _userManager.GetRolesAsync(user);

            var model = new Admin_EditUserViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                EmployeeNumber = user.EmployeeNumber,
                City = user.City,
                Email = user.Email,
                Roles = userRoles
            };

            return View(model);
        }
        /// <summary>
        /// POST method to handle the completed user edition form
        /// </summary>
        /// <param name="model">Admin_EditUserViewModel class object which will be processed by an instance of UserManager class</param>
        /// <returns>If succeed, returns a list of users with the current data. Otherwise - the page with the error message.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(Admin_EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {model.Id} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {model.Id} nie został odnaleziony!");
                return View("NotFound");
            }

            if (ModelState.IsValid)
            {
                if (!RegexUtilities.IsValidEmail(model.Email))
                {
                    ModelState.AddModelError("Email", "Adres mailowy powinien być zapisany w formacie użytkownik@domena np. jankowalski@twojafirma.pl");
                    return View(model);
                }
                else
                {
                    user.Id = model.Id;
                    user.Name = model.Name;
                    user.Surname = model.Surname;
                    user.EmployeeNumber = model.EmployeeNumber;
                    user.City = model.City;
                    user.Email = model.Email;

                    var result = await _userManager.UpdateAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Employees");
                    }

                    foreach (var error in result.Errors)
                    {
                        _logger.LogDebug(error.Description);
                        ModelState.AddModelError("", error.Description);
                    }

                    
                }
            }

            return View(model);


        }
        [HttpGet]
        public async Task<IActionResult> ChangePassword(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {id} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {id} nie został odnaleziony!");
                return View("NotFound");
            }

            var model = new Admin_ChangePasswordViewModel
            {
                FullName = user.FullName,
                Login = user.UserName
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ChangePassword(Admin_ChangePasswordViewModel model)
        {
            var user = await _userManager.FindByNameAsync(model.Login);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik: {model.Login} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {model.Login} nie został odnaleziony!");
                return View("NotFound");
            }

            if (model.NewPassword != model.NewPasswordConfirm)
            {
                ModelState.AddModelError(string.Empty, "Hasła nie pasują do siebie");
                return View(model);
            }
            else
            {

                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    GlobalAlert.SendGlobalAlert($"Hasło zmienione!", "info");
                    return RedirectToAction("EditUser", new { id = user.Id });
                }

                foreach (var error in result.Errors)
                {
                    _logger.LogDebug(error.Description);
                    ModelState.AddModelError("", error.Description);
                }

                return View(model);
            }

        }
        /// <summary>
        /// POST method to handle the user removal process 
        /// </summary>
        /// <param name="id">User ID to remove</param>
        /// <returns>If succeed, returns an updated users list. Otherwise - the page with the error message</returns>
        [HttpPost]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {id} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {id} nie został odnaleziony!");
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await _userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Employees");
                    }

                    foreach (var error in result.Errors)
                    {
                        _logger.LogDebug(error.Description);
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("Index", "Employees");
                }
                catch (DbUpdateException)
                {
                    ViewBag.ErrorTitle = "Podczas usuwania użytkownika wystąpił błąd!";
                    ViewBag.ErrorMessage = $"Istnieje rola przypisana do tego użytkownika.<br>" +
                        $"Przed usunięciem użytkownika usuń wszystkie role, które zostały mu przypisane.<br>" +
                        $"Po wykonaniu tych czynności ponów próbę.";
                    _logger.LogError($"Podczas usuwania użytkownika wystąpił błąd! Istnieje rola przypisana do tego użytkownika.");
                    return View("DbExceptionHandler");
                }
                
            }
        }
        [HttpGet]
        public async Task<IActionResult> ManageUserRoles(string userId)
        {
            ViewBag.userId = userId;

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {userId} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {userId} nie został odnaleziony!");
                return View("NotFound");
            }

            var model = new List<UserRolesViewModel>();

            foreach (var role in _roleManager.Roles)
            {
                var userRolesViewModel = new UserRolesViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name
                };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userRolesViewModel.IsSelected = true;
                }
                else
                {
                    userRolesViewModel.IsSelected = false;
                }

                model.Add(userRolesViewModel);
            }

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ManageUserRoles(List<UserRolesViewModel> model, string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {userId} nie został odnaleziony!";
                _logger.LogError($"Użytkownik o numerze ID: {userId} nie został odnaleziony!");
                return View("NotFound");
            }

            var roles = await _userManager.GetRolesAsync(user);
            var result = await _userManager.RemoveFromRolesAsync(user, roles);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nie można usunąć istniejących ról użytkownika");
                return View(model);
            }

            result = await _userManager.AddToRolesAsync(user,
                model.Where(x => x.IsSelected).Select(y => y.RoleName));

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Nie można przypisać zaznaczonych ról do użytkownika");
                return View(model);
            }

            return RedirectToAction("EditUser", new { Id = userId });
        }
        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> IsEmailInUse(string email, string InitialEmail)
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



    }
}
