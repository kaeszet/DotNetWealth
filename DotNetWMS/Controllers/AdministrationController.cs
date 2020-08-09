using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly RoleManager<IdentityRole> roleManager;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the UserManager class to maintain the user account
        /// </summary>
        private readonly UserManager<WMSIdentityUser> userManager;

        public AdministrationController(RoleManager<IdentityRole> roleManager, UserManager<WMSIdentityUser> userManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        /// <summary>
        /// GET method returning a view with access denied information
        /// </summary>
        /// <returns>Returns the view with access denied information</returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }
        /// <summary>
        /// GET method returning a view with list of roles
        /// </summary>
        /// <returns>Returns the view with list of created roles</returns>
        [HttpGet]
        public IActionResult ListOfRoles()
        {
            var roles = roleManager.Roles;
            return View(roles);
        }
        /// <summary>
        /// GET method returning a view with list of users
        /// </summary>
        /// <returns>Returns the view with list of registered users</returns>
        [HttpGet]
        public IActionResult ListOfUsers()
        {
            var users = userManager.Users;
            return View(users);
        }
        /// <summary>
        /// GET method to handle the role creation view
        /// </summary>
        /// <returns>Returns the role creation form view</returns>
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        /// <summary>
        /// POST method to handle the completed role creation form
        /// </summary>
        /// <param name="model">Admin_CreateRoleViewModel class object which will be processed by an instance of the Identity framework classes</param>
        /// <returns>Returns the view resulting from the processing of user-entered data</returns>
        [HttpPost]
        public async Task<IActionResult> CreateRole(Admin_CreateRoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                IdentityRole identityRole = new IdentityRole
                {
                    Name = model.RoleName
                };

                IdentityResult result = await roleManager.CreateAsync(identityRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfRoles", "Administration");
                }

                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }
        /// <summary>
        /// GET method to handle the role edition view
        /// </summary>
        /// <param name="id">Role ID which will be edited</param>
        /// <returns>Returns the view with details of the role with the entered ID or an error if the role was not found</returns>
        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {id} nie została odnaleziona!";
                return View("NotFound");
            }

            var model = new Admin_EditRoleViewModel
            {
                Id = role.Id,
                RoleName = role.Name
            };

            foreach (var user in userManager.Users)
            {
                if (await userManager.IsInRoleAsync(user, role.Name))
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
            var role = await roleManager.FindByIdAsync(model.Id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {model.Id} nie została odnaleziona!";
                return View("NotFound");
            }
            if (Admin_DefaultRoles.IsDefaultRole(model.RoleName))
            {
                ViewBag.ErrorTitle = "Podczas edycji roli wystąpił błąd!";
                ViewBag.ErrorMessage = "Nie można edytować domyślnej roli.";
                return View("DbExceptionHandler");
            }
            else
            {
                role.Name = model.RoleName;

                var result = await roleManager.UpdateAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfRoles");
                }

                foreach (var error in result.Errors)
                {
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
            var role = await roleManager.FindByIdAsync(id);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {id} nie została odnaleziona!";
                return View("NotFound");
            }
            else if (Admin_DefaultRoles.IsDefaultRole(role.Name))
            {
                ViewBag.ErrorTitle = "Podczas usuwania roli wystąpił błąd!";
                ViewBag.ErrorMessage = "Nie można usunąć domyślnej roli.";
                return View("DbExceptionHandler");
            }
            else
            {
                try
                {
                    var result = await roleManager.DeleteAsync(role);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListOfRoles");
                    }

                    foreach (var error in result.Errors)
                    {
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

            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {roleId} nie została odnaleziona!";
                return View("NotFound");
            }

            var model = new List<Admin_UsersInRoleViewModel>();

            foreach (var user in userManager.Users)
            {
                var userRoleViewModel = new Admin_UsersInRoleViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
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
            var role = await roleManager.FindByIdAsync(roleId);

            if (role == null)
            {
                ViewBag.ErrorMessage = $"Rola o numerze ID: {roleId} nie została odnaleziona!";
                return View("NotFound");
            }

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);

                IdentityResult result;
                
                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    result = await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await userManager.IsInRoleAsync(user, role.Name))
                {
                    if (role.Name == "Admin")
                    {
                        var findUser = model.FirstOrDefault(m => m.IsSelected == true);
                        if (findUser == null)
                        {
                            ViewBag.ErrorTitle = "Podczas usuwania użytkownika z roli wystąpił błąd!";
                            ViewBag.ErrorMessage = "Co najmniej jeden użytkownik musi mieć uprawnienia administratora.";
                            return View("DbExceptionHandler");
                        }
                        
                    }
                    result = await userManager.RemoveFromRoleAsync(user, role.Name);
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
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {id} nie został odnaleziony!";
                return View("NotFound");
            }

            var userRoles = await userManager.GetRolesAsync(user);

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
        public async Task<IActionResult> EditUser(Admin_EditUserViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {model.Id} nie został odnaleziony!";
                return View("NotFound");
            }
            else
            {
                user.Id = model.Id;
                user.Name = model.Name;
                user.Surname = model.Surname;
                user.EmployeeNumber = model.EmployeeNumber;
                user.City = model.City;
                user.Email = model.Email;
                
                var result = await userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return RedirectToAction("ListOfUsers");
                }

                foreach (var error in result.Errors)
                {
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
            var user = await userManager.FindByIdAsync(id);

            if (user == null)
            {
                ViewBag.ErrorMessage = $"Użytkownik o numerze ID: {id} nie został odnaleziony!";
                return View("NotFound");
            }
            else
            {
                try
                {
                    var result = await userManager.DeleteAsync(user);

                    if (result.Succeeded)
                    {
                        return RedirectToAction("ListOfUsers");
                    }

                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View("ListOfUsers");
                }
                catch (DbUpdateException)
                {
                    ViewBag.ErrorTitle = "Podczas usuwania użytkownika wystąpił błąd!";
                    ViewBag.ErrorMessage = $"Istnieje rola przypisana do tego użytkownika.<br>" +
                        $"Przed usunięciem użytkownika usuń wszystkie role, które zostały mu przypisane.<br>" +
                        $"Po wykonaniu tych czynności ponów próbę.";
                    return View("DbExceptionHandler");
                }
                
            }
        }
        

    }
}
