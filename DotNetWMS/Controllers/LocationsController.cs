using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Controllers
{
    public class LocationsController : Controller
    {

        private readonly DotNetWMSContext _context;

        public LocationsController(DotNetWMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string order, string search)
        {
            ViewData["SortByAddress"] = string.IsNullOrEmpty(order) ? "address_desc" : "";
            ViewData["Search"] = search;

            var locations = _context.Locations.Select(e => e);

            if (!string.IsNullOrEmpty(search))
            {
                locations = locations.Where(e => e.Address.Contains(search));
            }

            locations = order switch
            {
                "name_desc" => locations.OrderByDescending(w => w.Address),
                _ => locations.OrderBy(e => e.Address),
            };

            return View(await locations.AsNoTracking().ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(id);

            if (location == null)
            {
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
                    ModelState.AddModelError(string.Empty, $"Lokalizacja \"{location.Address}\" jest już w systemie! Wybierz inną nazwę.");
                }

            }
            return View(location);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var location = await _context.Locations.FirstOrDefaultAsync(m => m.Id == id);

            if (location == null)
            {
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
                return View("DbExceptionHandler");
            }

        }

        private bool LocationExists(int id)
        {
            return _context.Locations.Any(e => e.Id == id);
        }

    }
}
