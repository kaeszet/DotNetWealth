using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Controllers
{
    public class LocationsController : Controller
    {
        private readonly DotNetWMSContext _context;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(DotNetWMSContext context, ILogger<LocationsController> logger)
        {
            _context = context;
            _logger = logger;
        }

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

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }

        private List<string> IsLocationInUse(int id)
        {
            List<string> listOfOccurrences = new List<string>();

            var findInWarehouses = _context.Warehouses.Where(x => x.LocationId == id)?.Select(x => x.AssignFullName);

            if (findInWarehouses.Any())
            {
                listOfOccurrences.AddRange(findInWarehouses.ToList());
            }

            var findInExternals = _context.Externals.Where(x => x.LocationId == id)?.Select(x => x.FullName);

            if (findInWarehouses.Any())
            {
                listOfOccurrences.AddRange(findInExternals.ToList());
            }

            var findInEmployees = _context.Users.Where(x => x.LocationId == id)?.Select(x => x.FullName);

            if (findInWarehouses.Any())
            {
                listOfOccurrences.AddRange(findInEmployees.ToList());
            }

            return listOfOccurrences;
        }

    }
}
