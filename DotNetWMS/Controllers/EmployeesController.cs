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
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace DotNetWMS.Controllers
{
    [Authorize(Roles = "Kadry,Moderator,Admin")]
    public class EmployeesController : Controller
    {
        private readonly DotNetWMSContext _context;
        private readonly UserManager<WMSIdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<EmployeesController> _logger;

        public EmployeesController(DotNetWMSContext context, UserManager<WMSIdentityUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<EmployeesController> logger)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
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

            switch (order)
            {
                case "name_desc":
                    users = users.OrderByDescending(e => e.Surname).Include(e => e.Department);
                    break;
                default:
                    users = users.OrderBy(e => e.Surname).Include(e => e.Department);
                    break;
            }
            _logger.LogInformation("INFO: Użytkownik wyświetlił listę pracowników");
            return View(await users.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Details(string id)
        {
            var url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();

            if (string.IsNullOrEmpty(id))
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var user = await _context.Users.Include(u => u.Department).FirstOrDefaultAsync(u => u.Id == id);
            var location = await _context.Locations.FindAsync(user.LocationId);

            if (user == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie pracownika o podanym id = {id}");
                return NotFound();
            }

            UserAndLocationViewModel viewModel = new UserAndLocationViewModel()
            {
                UserId = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                EmployeeNumber = user.EmployeeNumber,
                DepartmentId = user.DepartmentId,
                Street = user.Street,
                ZipCode = user.ZipCode,
                City = user.City,
                LocationId = location?.Id,
                Address = location?.Address,
                Latitude = location?.Latitude,
                Longitude = location?.Longitude

            };

            ViewBag.QrCode = QRCodeCreator.ShowQRCode(url);
            ViewBag.DepartmentName = user.Department.Name;
            _logger.LogInformation($"INFO: Użytkownik wyświetlił dane pracownika o id = {id}");

            return View(viewModel);
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            _logger.LogInformation("INFO: Użytkownik wyświetlił kreator dodawania nowego pracownika");
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");

            return View();
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserAndLocationViewModel viewModel)
        {
            bool isEmployeeExists = _context.Users.Any(i => i.EmployeeNumber == viewModel.EmployeeNumber);
            string autoGenPassword = "Test123!";

            if (ModelState.IsValid)
            {
                if (!isEmployeeExists)
                {
                    WMSIdentityUser user = new WMSIdentityUser()
                    {
                        Name = viewModel.Name,
                        Surname = viewModel.Surname,
                        EmployeeNumber = viewModel.EmployeeNumber,
                        DepartmentId = viewModel.DepartmentId,
                        Street = viewModel.Street,
                        ZipCode = viewModel.ZipCode,
                        City = viewModel.City,
                        UserName = GenerateUserLogin(viewModel.Name, viewModel.Surname, viewModel.EmployeeNumber)
                    };

                    Location location = new Location()
                    {
                        Address = viewModel.Address,
                        Latitude = viewModel.Latitude,
                        Longitude = viewModel.Longitude
                    };

                    var loc = _context.Locations.FirstOrDefault(l => l.Address == viewModel.Address);

                    if (loc == null)
                    {
                        _context.Add(location);
                        await _context.SaveChangesAsync();

                        var locId = _context.Locations.FirstOrDefault(l => l.Address == viewModel.Address).Id;
                        user.LocationId = locId;

                    }
                    else
                    {
                        user.LocationId = loc.Id;
                    }

                    await _userManager.CreateAsync(user, autoGenPassword);
                    GlobalAlert.SendGlobalAlert($"Pracownik {user.FullName} został dodany do bazy!", "success");
                    _logger.LogInformation($"Pracownik {user.FullName} został dodany do bazy!");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogError($"Pracownik {viewModel.Name}, {viewModel.Surname} został już wprowadzony do systemu!");
                    ModelState.AddModelError(string.Empty, $"Pracownik {viewModel.Name}, {viewModel.Surname} został już wprowadzony do systemu!");
                }

            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", viewModel.DepartmentId);
            

            return View(viewModel);
        }
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            var location = await _context.Locations.FindAsync(user.LocationId);

            if (user == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie pracownika o podanym id = {id}");
                return NotFound();
            }

            UserAndLocationViewModel viewModel = new UserAndLocationViewModel()
            {
                UserId = user.Id,
                Name = user.Name,
                Surname = user.Surname,
                EmployeeNumber = user.EmployeeNumber,
                DepartmentId = user.DepartmentId,
                Street = user.Street,
                ZipCode = user.ZipCode,
                City = user.City,
                LocationId = location?.Id,
                Address = location?.Address,
                Latitude = location?.Latitude,
                Longitude = location?.Longitude

            };

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", user.DepartmentId);
            return View(viewModel);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="viewModel"><c>UserAndLocationViewModel</c> object contains <c>WMSIdentityUser</c> and <c>Location</c> model classes</param>
        /// <returns>If succeed, returns Department's Index view, data validation on the model side</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(UserAndLocationViewModel viewModel)
        {
            var _user = await _userManager.FindByIdAsync(viewModel.UserId);

            if (_user == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie pracownika o podanym id = {viewModel.UserId}!");
                return NotFound();
            }

            string oldEmployeeNumber = _context.Users.AsNoTracking().FirstOrDefault(u => u.Id == viewModel.UserId)?.EmployeeNumber;

            bool isEmployeeExists = _context.Users.Any(e => e.EmployeeNumber == viewModel.EmployeeNumber
            && e.EmployeeNumber != oldEmployeeNumber
            && !string.IsNullOrEmpty(oldEmployeeNumber));
            
            if (ModelState.IsValid)
            {
                if (!isEmployeeExists)
                {
                    try
                    {
                        _user.Name = viewModel.Name;
                        _user.Surname = viewModel.Surname;
                        _user.EmployeeNumber = viewModel.EmployeeNumber;
                        _user.DepartmentId = viewModel.DepartmentId;
                        _user.Street = viewModel.Street;
                        _user.ZipCode = viewModel.ZipCode;
                        _user.City = viewModel.City;

                        if (!string.IsNullOrEmpty(viewModel.Address) && !string.IsNullOrEmpty(viewModel.Latitude) && !string.IsNullOrEmpty(viewModel.Longitude))
                        {
                            Location location = new Location()
                            {
                                Address = viewModel.Address,
                                Latitude = viewModel.Latitude,
                                Longitude = viewModel.Longitude
                            };

                            var loc = _context.Locations.FirstOrDefault(l => l.Address == viewModel.Address);

                            if (loc == null)
                            {
                                _context.Add(location);
                                await _context.SaveChangesAsync();

                                var locId = _context.Locations.FirstOrDefault(l => l.Address == viewModel.Address).Id;
                                _user.LocationId = locId;

                            }
                            else
                            {
                                _user.LocationId = loc.Id;
                            }
                        }

                        var result = await _userManager.UpdateAsync(_user);
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(viewModel.UserId))
                        {
                            _logger.LogError($"Pracownik {_user.FullName} został zmieniony przez innego użytkownika");
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    GlobalAlert.SendGlobalAlert($"Pracownik {_user.FullName} został zmieniony!", "success");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogDebug($"Pracownik {_user.FullName} został już wprowadzony do systemu!");
                    ModelState.AddModelError(string.Empty, $"Pracownik {_user.FullName} został już wprowadzony do systemu!");
                }
            }
            else
            {
                _logger.LogError($"Walidacja modelu nie powiodła się");
            }

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", _user.DepartmentId);

            return View(viewModel);
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
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var user = await _context.Users
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie pracownika o podanym id = {user.Id}!");
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
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var user = await _context.Users.FindAsync(id);
            try
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                GlobalAlert.SendGlobalAlert($"Pracownik {user.FullName} został usunięty!", "danger");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania pracownika wystąpił błąd!";
                ViewBag.ErrorMessage = $"Istnieje przedmiot przypisany do tego pracownika.<br>" +
                    $"Przed usunięciem pracownika upewnij się, że zdał wszystkie przedmioty.<br>" +
                    $"Odznacz je w dziale \"Majątek\" i ponów próbę.";
                _logger.LogError("Podczas usuwania pracownika wystąpił błąd! Istnieje przedmiot przypisany do tego pracownika. Przed usunięciem pracownika upewnij się, że zdał wszystkie przedmioty. Odznacz je w dziale \"Majątek\" i ponów próbę.");
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