using DotNetWMS.Data;
using DotNetWMS.Models;
using static DotNetWMS.Resources.UserLoginGenerator;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetWMS.Resources;

namespace DotNetWMS.Controllers
{
    public class EmployeesNEWController : Controller
    {
        private readonly DotNetWMSContext _context;
        private readonly UserManager<WMSIdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public EmployeesNEWController(DotNetWMSContext context, UserManager<WMSIdentityUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(string order, string search)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["Search"] = search;

            var users = _context.Users.Select(u => u);

            if (!string.IsNullOrEmpty(search))
            {
                users = users.Where(e => e.Surname.Contains(search) || e.Name.Contains(search) || e.EmployeeNumber.Contains(search));
            }

            users = order switch
            {
                "name_desc" => users.OrderByDescending(e => e.Surname).Include(e => e.Department),
                _ => users.OrderBy(e => e.Surname).Include(e => e.Department),
            };
            return View(await users.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            var url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(u => u.Department)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            ViewBag.QrCode = QRCodeCreator.ShowQRCode(url);
            TempData["Adress"] = GoogleMapsGenerator.PrepareAdressToGeoCodeEmployee(user);

            return View(user);
        }
        
        public IActionResult Create()
        {

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");

            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WMSIdentityUser user)
        {
            bool isEmployeeExists = _context.Users.Any(i => i.EmployeeNumber == user.EmployeeNumber);
            string autoGenPassword = "Test123!";

            if (ModelState.IsValid)
            {
                if (!isEmployeeExists)
                {
                    user.UserName = GenerateUserLogin(user.Name, user.Surname, user.EmployeeNumber);
                    await _userManager.CreateAsync(user, autoGenPassword);
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Pracownik {user.FullName} został już wprowadzony do systemu!");
                }

            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);

            return View(user);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            TempData["Adress"] = GoogleMapsGenerator.PrepareAdressToGeoCodeEmployee(user);

            return View(user);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="user">WMSIdentityUser model class with binding DB attributes</param>
        /// <returns>If succeed, returns Department's Index view, data validation on the model side</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(WMSIdentityUser user)
        {
            var _user = await _userManager.FindByIdAsync(user.Id);

            if (_user == null)
            {
                return NotFound();
            }

            string oldEmployeeNumber = _context.Users.Find(user.Id).EmployeeNumber;

            bool isEmployeeExists = _context.Users.Any(e => e.EmployeeNumber == user.EmployeeNumber
            && e.EmployeeNumber != oldEmployeeNumber
            && !string.IsNullOrEmpty(oldEmployeeNumber));
            
            if (ModelState.IsValid)
            {
                if (!isEmployeeExists)
                {
                    try
                    {
                        _user.Name = user.Name;
                        _user.Surname = user.Surname;
                        _user.EmployeeNumber = user.EmployeeNumber;
                        _user.DepartmentId = user.DepartmentId;
                        _user.Street = user.Street;
                        _user.ZipCode = user.ZipCode;
                        _user.City = user.City;

                        var result = await _userManager.UpdateAsync(_user);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(user.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Pracownik {user.FullName} został już wprowadzony do systemu!");
                }
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            TempData["Adress"] = GoogleMapsGenerator.PrepareAdressToGeoCodeEmployee(user);

            return View(user);
        }
        /// <summary>
        /// GET method responsible for returning an WMSIdentityUser's Delete view
        /// </summary>
        /// <param name="id">WMSIdentityUser ID to delete</param>
        /// <returns>Returns WMSIdentityUser's Delete view if exists</returns>
        public async Task<IActionResult> Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var user = await _context.Users
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
        /// <summary>
        /// POST method responsible for removing user from DB if the user confirms this action
        /// </summary>
        /// <param name="id">WMSIdentityUser ID to delete</param>
        /// <returns>Returns WMSIdentityUser's Index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania pracownika wystąpił błąd!";
                ViewBag.ErrorMessage = $"Istnieje przedmiot przypisany do tego pracownika.<br>" +
                    $"Przed usunięciem pracownika upewnij się, że zdał wszystkie przedmioty.<br>" +
                    $"Odznacz je w dziale \"Majątek\" i ponów próbę.";
                return View("DbExceptionHandler");
            }

        }
        /// <summary>
        /// A private method responsible for checking if there is a user with the given id
        /// </summary>
        /// <param name="id">WMSIdentityUser ID to check</param>
        /// <returns>Returns true if the user exists. Otherwise - false.</returns>
        private bool EmployeeExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}