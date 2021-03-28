using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Authorization;
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
    /// Controller class responsible for <c>Location</c> functionality
    /// </summary>
    
    [Authorize(Roles = "Moderator,Admin")]
    public class LocationsController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(DotNetWMSContext context, ILogger<LocationsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning Location's Index view and supports a search engine
        /// </summary>
        /// <param name="order">Sort names in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Location's Index view with list of locations in the order set by the user</returns>
        public IActionResult Index(string order, string search)
        {
            ViewData["SortByAddress"] = string.IsNullOrEmpty(order) ? "address_desc" : "";
            ViewData["Search"] = search;

            var locations = _context.Locations.Select(e => e);

            List<LocationListViewModel> locationList = new List<LocationListViewModel>();

            foreach (var item in locations)
            {
                List<string> listOfOccurences = IsLocationInUse(item.Id);

                LocationListViewModel location = new LocationListViewModel()
                {
                    Id = item.Id,
                    Address = item.Address,
                    Latitude = item.Latitude,
                    Longitude = item.Longitude,
                    IsInUse = listOfOccurences.Any(),
                    Records = listOfOccurences
                };

                locationList.Add(location);
            }

            if (!string.IsNullOrEmpty(search))
            {
                locationList = locationList.Where(e => e.Address.Contains(search)).ToList();
            }

            locationList = order switch
            {
                "address_desc" => locationList.OrderByDescending(x => x.Address).ToList(),
                _ => locationList.OrderBy(x => x.Address).ToList(),
            };

            return View(locationList);
        }
        /// <summary>
        /// GET method to show Google map and markup with given coordinates
        /// </summary>
        /// <param name="id">ID of location in DB</param>
        /// <returns>ShowMap view with <c>Location</c> data</returns>
        public async Task<IActionResult> ShowMap(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie lokalizacji o podanym id = {id}");
                return NotFound();
            }

            return View(location);
        }
        /// <summary>
        /// GET method responsible for returning an Location's Edit view
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <returns>Edit view with <c>Location</c> data</returns>
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie lokalizacji o podanym id = {id}");
                return NotFound();
            }

            return View(location);
        }
        /// <summary>
        /// POST method to accept changes in Edit View and save them in DB
        /// </summary>
        /// <param name="id">Location ID</param>
        /// <param name="location"><c>Location</c> model class object</param>
        /// <returns>If succeded redirect user to Location's Index view, otherwise - show error message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Location location)
        {
            if (id != location.Id)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie lokalizacji o podanym id = {id}");
                return NotFound();
            }

            var oldLocationAddress = _context.Locations.AsNoTracking().FirstOrDefault(l => l.Id == location.Id)?.Address;

            bool isLocationExists = _context.Locations.Any(l => l.Address == location.Address
            && l.Address != oldLocationAddress
            && !string.IsNullOrEmpty(oldLocationAddress));

            if (ModelState.IsValid)
            {
                if (!isLocationExists)
                {
                    try
                    {
                        _context.Update(location);
                        await _context.SaveChangesAsync();
                        GlobalAlert.SendGlobalAlert($"Dane lokalizacji {location.Address} zostały zaktualizowane!", "success");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!LocationExists(location.Id))
                        {
                            _logger.LogError($"Lokalizacja {location.Address} została zmieniona przez innego użytkownika");
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
                    _logger.LogDebug($"Lokalizacja \"{location.Address}\" jest już w systemie!");
                    ModelState.AddModelError(string.Empty, $"Lokalizacja \"{location.Address}\" jest już w systemie!");
                }

            }
            return View(location);
        }
        /// <summary>
        /// GET method responsible for returning an Location's Delete view
        /// </summary>
        /// <param name="id">ID of <c>Location</c> to delete</param>
        /// <returns>DeleteConfirmation View with <c>Location</c> data</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var location = await _context.Locations.FirstOrDefaultAsync(m => m.Id == id);

            if (location == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie lokalizacji o podanym id = {id}");
                return NotFound();
            }

            return View(location);
        }
        /// <summary>
        /// POST method responsible for removing <c>Location</c> from DB if the user confirms this action
        /// </summary>
        /// <param name="id">ID of <c>Location</c> to delete</param>
        /// <returns>If succeeded returns Location's Index view, otherwise - return DbUpdateException view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var location = await _context.Locations.FindAsync(id);

            try
            {
                _context.Locations.Remove(location);
                await _context.SaveChangesAsync();
                GlobalAlert.SendGlobalAlert($"Lokalizacja \"{location.Address}\" została usunięta!", "danger");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania wystąpił błąd!";
                ViewBag.ErrorMessage = $"Ta lokalizacja została przypisana do jednego z obiektów.<br>" +
                    $"Przed usunięciem upewnij się, że nie jest ona wykorzystywana<br>" +
                    $"Sprawdź działy \"Magazyny\", \"Kontrahenci\", \"Pracownicy\" itp. i ponów próbę.";
                _logger.LogError($"Podczas usuwania wystąpił błąd! Ta lokalizacja została przypisana do jednego z obiektów.");
                return View("DbExceptionHandler");
            }

        }
        /// <summary>
        /// Method to check is <c>Location</c> with given ID exists in DB
        /// </summary>
        /// <param name="id"><c>Location</c> ID to search</param>
        /// <returns>True if any location has been found, otherwise - false</returns>
        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }
        /// <summary>
        /// Method to check is <c>Location</c> with given ID connected by FK with other DB tables and give back list of occurences
        /// </summary>
        /// <param name="id"><c>Location</c> ID to check</param>
        /// <returns>List of records where <c>Location</c> is connected by FK</returns>
        private List<string> IsLocationInUse(int id)
        {
            List<string> listOfOccurrences = new List<string>();

            var findInWarehouses = _context.Warehouses.Where(x => x.LocationId == id)?.Select(x => x.AssignFullName);

            if (findInWarehouses.Any())
            {
                listOfOccurrences.AddRange(findInWarehouses.ToList());
            }

            var findInExternals = _context.Externals.Where(x => x.LocationId == id)?.Select(x => x.FullName);

            if (findInExternals.Any())
            {
                listOfOccurrences.AddRange(findInExternals.ToList());
            }

            var findInEmployees = _context.Users.Where(x => x.LocationId == id)?.Select(x => x.FullName);

            if (findInEmployees.Any())
            {
                listOfOccurrences.AddRange(findInEmployees.ToList());
            }

            return listOfOccurrences;
        }

    }
}
