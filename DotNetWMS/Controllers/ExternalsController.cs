using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Authorization;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class to support the CRUD process for the External model
    /// </summary>
    [Authorize(Roles = "Standard,StandardPlus,Moderator")]
    public class ExternalsController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;

        public ExternalsController(DotNetWMSContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET method responsible for returning an External's Index view and supports a search engine
        /// </summary>
        /// <param name="order">Sort names or types in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns External's Index view with list of externals in the order set by the user</returns>
        public async Task<IActionResult> Index(string order, string search)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["SortByType"] = order == "Type" ? "type_desc" : "Type";
            ViewData["Search"] = search;

            var externals = _context.Externals.Select(e => e);

            if (!string.IsNullOrEmpty(search))
            {
                externals = externals.Where(e => e.Name.Contains(search) || e.TaxId.Contains(search));
            }

            externals = order switch
            {
                "name_desc" => externals.OrderByDescending(w => w.Name),
                "type_desc" => externals.OrderByDescending(e => e.Type),
                "Type" => externals.OrderBy(e => e.Type),
                _ => externals.OrderBy(e => e.Name),
            };

            return View(await externals.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an External's StatusView
        /// </summary>
        /// <param name="model">StatusViewViewModel class object used to check all items with the selected status</param>
        /// <returns>Returns External's StatusView</returns>
        public async Task<IActionResult> StatusView(StatusViewViewModel model)
        {
            var users = await _context.Users.ToListAsync();
            var warehouses = await _context.Warehouses.ToListAsync();

            ViewData["UserList"] = new SelectList(users, "Id", "FullName");
            ViewData["WarehouseList"] = new SelectList(warehouses, "Id", "AssignFullName");

            return View(model);
        }
        
        [HttpPost]
        public IActionResult StatusView(string u, int? w, ItemState state, string order, string search)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["Search"] = search;
            ViewData["InfoMessage"] = "Brak asortymentu spełniającego powyższe kryteria";
            DateTime dateTime = DateTime.Now;

            IQueryable<Item> items = _context.Items.AsNoTracking().Where(i => (u == "All" || i.UserId == u) && (w == 0 || i.WarehouseId == w));

            switch (state)
            {
                case ItemState.Unknown:
                    break;
                case ItemState.OutOfWarranty:
                    items = items.Where(i => i.WarrantyDate < dateTime);
                    break;
                default:
                    items = items.Where(i => i.State == state);
                    break;
            }

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.Type.Contains(search) || i.Producer.Contains(search) || i.Name.Contains(search) || i.Model.Contains(search) || i.ItemCode.Contains(search));
            }


            return PartialView("_StatusViewTable", items.ToList());
        }
        /// <summary>
        /// GET method responsible for returning an External's Details view
        /// </summary>
        /// <param name="id">External ID which should be returned</param>
        /// <returns>External's Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var external = await _context.Externals.FirstOrDefaultAsync(m => m.Id == id);
            var location = await _context.Locations.FindAsync(external.LocationId);

            if (external == null)
            {
                return NotFound();
            }

            ExternalAndLocationViewModel viewModel = new ExternalAndLocationViewModel()
            {
                Type = external.Type,
                Name = external.Name,
                TaxId = external.TaxId,
                Street = external.Street,
                ZipCode = external.ZipCode,
                City = external.City,
                LocationId = location?.Id,
                Address = location?.Address,
                Latitude = location?.Latitude,
                Longitude = location?.Longitude

            };

            return View(viewModel);
        }
        /// <summary>
        /// GET method responsible for returning an External's Create view
        /// </summary>
        /// <returns>Returns External's Create view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="external">External model class with binding DB attributes</param>
        /// <returns>If succeed, returns External's Index view. Otherwise - show error message</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ExternalAndLocationViewModel viewModel)
        {
            bool isExternalExists = _context.Warehouses.Any(w => w.Name == viewModel.Name);

            if (ModelState.IsValid)
            {
                if (!isExternalExists)
                {
                    External external = new External()
                    {
                        Type = viewModel.Type,
                        Name = viewModel.Name,
                        TaxId = viewModel.TaxId,
                        Street = viewModel.Street,
                        ZipCode = viewModel.ZipCode,
                        City = viewModel.City
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
                        external.LocationId = locId;

                    }
                    else
                    {
                        external.LocationId = loc.Id;
                    }

                    _context.Add(external);
                    await _context.SaveChangesAsync();

                    GlobalAlert.SendGlobalAlert($"Kontrahent {external.FullName} został dodany do bazy!", "success");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Podana nazwa kontrahenta: {viewModel.Name} jest już w systemie! Wybierz inną nazwę.");
                }
            }
            return View(viewModel);
        }
        /// <summary>
        /// GET method responsible for returning an External's Edit view
        /// </summary>
        /// <param name="id">External ID which should be returned</param>
        /// <returns>Returns External's Edit view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var external = await _context.Externals.FindAsync(id);
            var location = await _context.Locations.FindAsync(external.LocationId);

            if (external == null)
            {
                return NotFound();
            }

            ExternalAndLocationViewModel viewModel = new ExternalAndLocationViewModel()
            {
                ExternalId = (int)id,
                Type = external.Type,
                Name = external.Name,
                TaxId = external.TaxId,
                Street = external.Street,
                ZipCode = external.ZipCode,
                City = external.City,
                LocationId = location?.Id,
                Address = location?.Address,
                Latitude = location?.Latitude,
                Longitude = location?.Longitude

            };


            return View(viewModel);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="id">External ID to edit</param>
        /// <param name="external">External model class with binding DB attributes</param>
        /// <returns>If succeed, returns External's Index view, data validation on the model side</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ExternalAndLocationViewModel viewModel)
        {
            if (id != viewModel.ExternalId)
            {
                return NotFound();
            }

            var oldExternalName = _context.Externals.AsNoTracking().FirstOrDefault(w => w.Id == viewModel.ExternalId)?.Name;

            bool isExternalExists = _context.Externals.Any(w => w.Name == viewModel.Name
            && w.Name != oldExternalName
            && !string.IsNullOrEmpty(oldExternalName));

            if (ModelState.IsValid)
            {
                if (!isExternalExists)
                {
                    try
                    {
                        External external = new External()
                        {
                            Id = viewModel.ExternalId,
                            Type = viewModel.Type,
                            Name = viewModel.Name,
                            TaxId = viewModel.TaxId,
                            Street = viewModel.Street,
                            ZipCode = viewModel.ZipCode,
                            City = viewModel.City
                        };

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
                                external.LocationId = locId;

                            }
                            else
                            {
                                external.LocationId = loc.Id;
                            }
                        }

                        _context.Update(external);
                        await _context.SaveChangesAsync();
                        GlobalAlert.SendGlobalAlert($"Magazyn {external.FullName} został zmieniony!", "success");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ExternalExists(viewModel.ExternalId))
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
                    ModelState.AddModelError(string.Empty, $"Magazyn \"{viewModel.Name}\" został już wprowadzony do systemu! Wybierz inną nazwę.");
                }

            }
            return View(viewModel);
        }
        /// <summary>
        /// GET method responsible for returning an External's Delete view
        /// </summary>
        /// <param name="id">External ID to delete</param>
        /// <returns>Returns External's Delete view if exists</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var external = await _context.Externals
                .FirstOrDefaultAsync(m => m.Id == id);
            if (external == null)
            {
                return NotFound();
            }

            return View(external);
        }
        /// <summary>
        /// POST method responsible for removing external from DB if the user confirms this action
        /// </summary>
        /// <param name="id">External ID to delete</param>
        /// <returns>Returns External's Index view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var external = await _context.Externals.FindAsync(id);
            try
            {
                _context.Externals.Remove(external);
                await _context.SaveChangesAsync();
                GlobalAlert.SendGlobalAlert($"Kontrahent {external.Name} został usunięty!", "danger");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania klienta wystąpił błąd!";
                ViewBag.ErrorMessage = $"Istnieje przedmiot przypisany do tego klienta.<br>" +
                    $"Przed usunięciem klienta upewnij się, że wszystkie przedmioty zostały zwrócone.<br>" +
                    $"Odznacz je w dziale \"Majątek\" i ponów próbę.";
                return View("DbExceptionHandler");
            }
            
        }
        /// <summary>
        /// Private method responsible for checking if there is a external with the given id
        /// </summary>
        /// <param name="id">External ID to check</param>
        /// <returns>Returns true if the external exists. Otherwise - false.</returns>
        private bool ExternalExists(int id)
        {
            return _context.Externals.Any(e => e.Id == id);
        }
    }
}
