using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;
using DotNetWMS.Resources;
using Microsoft.Extensions.Logging;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class to support the CRUD process for the Warehouse model
    /// </summary>
    [Authorize]
    public class WarehousesController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<WarehousesController> _logger;

        public WarehousesController(DotNetWMSContext context, ILogger<WarehousesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Index view and supports a search engine
        /// </summary>
        /// <param name="order">Sort names in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Warehouse's Index view with list of warehouses in the order set by the user</returns>
        public async Task<IActionResult> Index(string order, string search)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["Search"] = search;

            var warehouses = _context.Warehouses.Select(w => w);

            if (!string.IsNullOrEmpty(search))
            {
                _logger.LogDebug($"DEBUG: Zwrócono widok magazynów!");
                warehouses = warehouses.Where(w => w.Name.Contains(search) || w.Street.Contains(search));
            }

            switch (order)
            {
                case "name_desc":
                    _logger.LogDebug($"DEBUG: Sortowanie magazynów po nazwie!");
                    warehouses = warehouses.OrderByDescending(w => w.Name);
                    break;
                default:
                    warehouses = warehouses.OrderBy(w => w.Name);
                    break;
            }
            return View(await warehouses.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Details view
        /// </summary>
        /// <param name="id">Warehouse ID which should be returned</param>
        /// <returns>Warehouse's Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FirstOrDefaultAsync(m => m.Id == id);

            if (warehouse == null)
            {
                _logger.LogDebug($"DEBUG: Magazyn nie istnieje!");
                return NotFound();
            }

            var location = await _context.Locations.FirstOrDefaultAsync(l => l.Id == warehouse.LocationId);

            WarehouseAndLocationViewModel viewModel = new WarehouseAndLocationViewModel()
            {
                WarehouseId = warehouse.Id,
                Name = warehouse.Name,
                Street = warehouse.Street,
                ZipCode = warehouse.ZipCode,
                City = warehouse.City,
                LocationId = location?.Id,
                Address = location?.Address,
                Latitude = location?.Latitude,
                Longitude = location?.Longitude

            };

            return View(viewModel);
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Create view
        /// </summary>
        /// <returns>Returns Warehouse's Create view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="viewModel"><c>WarehouseAndLocationViewModel</c> object contains <c>Warehouse</c> and <c>Location</c> model classes</param>
        /// <returns>If succeed, returns Warehouse's Index view. Otherwise - show error message</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WarehouseAndLocationViewModel viewModel)
        {
            bool isWarehouseExists = _context.Warehouses.Any(w => w.Name == viewModel.Name);

            if (ModelState.IsValid)
            {
                if (!isWarehouseExists)
                {
                    Warehouse warehouse = new Warehouse()
                    {
                        Name = viewModel.Name,
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
                        warehouse.LocationId = locId;

                    }
                    else
                    {
                        warehouse.LocationId = loc.Id;
                    }

                    _context.Add(warehouse);
                    await _context.SaveChangesAsync();

                    GlobalAlert.SendGlobalAlert($"Magazyn {warehouse.AssignFullName} został dodany do bazy!", "success");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogDebug($"DEBUG: Magazyn {viewModel.Name} został już wprowadzony do systemu! Wybierz inną nazwę.");
                    ModelState.AddModelError(string.Empty, $"Magazyn {viewModel.Name} został już wprowadzony do systemu! Wybierz inną nazwę.");
                }
                
            }
            return View(viewModel);
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Edit view
        /// </summary>
        /// <param name="id">Warehouse ID which should be returned</param>
        /// <returns>Returns Warehouse's Edit view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            
            if (warehouse == null)
            {
                _logger.LogDebug($"Magazyn nie istnieje!");
                return NotFound();
            }

            var location = await _context.Locations.FindAsync(warehouse.LocationId);

            WarehouseAndLocationViewModel viewModel = new WarehouseAndLocationViewModel()
            {
                WarehouseId = (int)id,
                Name = warehouse.Name,
                Street = warehouse.Street,
                ZipCode = warehouse.ZipCode,
                City = warehouse.City,
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
        /// <param name="id">Warehouse ID to edit</param>
        /// <param name="viewModel"><c>WarehouseAndLocationViewModel</c> object contains <c>Warehouse</c> and <c>Location</c> model classes</param>
        /// <returns>If succeed, returns Warehouse's Index view, data validation on the model side</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WarehouseAndLocationViewModel viewModel)
        {
            
            if (id != viewModel.WarehouseId)
            {
                _logger.LogDebug($"DEBUG: Takie id = {id} nie istnieje w bazie magazynów!");
                return NotFound();
            }

            var oldWarehouseNumber = _context.Warehouses.AsNoTracking().FirstOrDefault(w => w.Id == viewModel.WarehouseId)?.Name;

            bool isWarehouseExists = _context.Warehouses.Any(w => w.Name == viewModel.Name 
            && w.Name != oldWarehouseNumber 
            && !string.IsNullOrEmpty(oldWarehouseNumber));

            if (ModelState.IsValid)
            {
                if (!isWarehouseExists)
                {
                    try
                    {
                        Warehouse warehouse = new Warehouse()
                        {
                            Id = viewModel.WarehouseId,
                            Name = viewModel.Name,
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
                                warehouse.LocationId = locId;

                            }
                            else
                            {
                                warehouse.LocationId = loc.Id;
                            }
                        }
                        
                        _context.Update(warehouse);
                        await _context.SaveChangesAsync();
                        GlobalAlert.SendGlobalAlert($"Magazyn {warehouse.AssignFullName} został zmieniony!", "success");
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WarehouseExists(viewModel.WarehouseId))
                        {
                            _logger.LogError($"Magazyn {viewModel.Name} została zmieniona przez innego użytkownika");
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
                    _logger.LogDebug($"DEBUG: Magazyn \"{viewModel.Name}\" został już wprowadzony do systemu! Wybierz inną nazwę.");
                    ModelState.AddModelError(string.Empty, $"Magazyn \"{viewModel.Name}\" został już wprowadzony do systemu! Wybierz inną nazwę.");
                }
                
            }
            _logger.LogDebug($"DEBUG: Pomyślnie zedytowano magazyn!");
            return View(viewModel);
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Delete view
        /// </summary>
        /// <param name="id">Warehouse ID to delete</param>
        /// <returns>Returns Warehouse's Delete view if exists</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                _logger.LogDebug($"DEBUG: Magazyn {warehouse?.Name} nie istnieje!");
                return NotFound();
            }

            _logger.LogDebug($"DEBUG: Znaleziono magazyn {warehouse.Name}!");
            return View(warehouse);
        }
        /// <summary>
        /// POST method responsible for removing warehouse from DB if the user confirms this action
        /// </summary>
        /// <param name="id">Warehouse ID to delete</param>
        /// <returns>Returns Warehouse's Index view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            try
            {
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
                GlobalAlert.SendGlobalAlert($"Magazyn {warehouse.AssignFullName} został usunięty!", "danger");
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania magazynu wystąpił błąd!";
                ViewBag.ErrorMessage = $"Istnieje przedmiot przypisany do tego magazynu.<br>" +
                    $"Przed usunięciem magazynu upewnij się, że wszystkie przedmioty zostały usunięte.<br>" +
                    $"Odznacz je w dziale \"Majątek\" i ponów próbę.";
                _logger.LogError($"Podczas usuwania magazynu wystąpił błąd! Istnieje przedmiot przypisany do tego magazynu.");
                return View("DbExceptionHandler");
            }
            
        }

        public async Task<IActionResult> StocktakingIndex(StocktakingStartViewModel model)
        {
            var warehouses = await _context.Warehouses.ToListAsync();
            ViewData["WarehouseList"] = new SelectList(warehouses, "Id", "AssignFullName");
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> StocktakingIndex(string warehouseFullName)
        {
            int id = Convert.ToInt32(warehouseFullName);
            ViewBag.warehouseId = id;
            var itemsInWarehouse = await _context.Items.Select(i => i).Where(i => i.WarehouseId == id).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse).ToListAsync();
            if (itemsInWarehouse.Count == 0)
            {
                ViewData["InfoMessage"] = "W wybranym magazynie nie ma przedmiotów!";
            }

            var model = new List<StocktakingTableViewModel>();
            foreach (var item in itemsInWarehouse)
            {
                var stocktakingViewModel = new StocktakingTableViewModel()
                {
                    ItemType = item.Type,
                    ItemModel = item.Model,
                    ItemName = item.Name,
                    ItemCode = item.ItemCode,
                    ItemQuantity = item.Quantity.ToString(),
                    ItemUnit = item.Units
                    
                };

                model.Add(stocktakingViewModel);
            }

            return PartialView("_StocktakingIndexTable", model);
        }
        public async Task<IActionResult> StocktakingBegin(List<StocktakingTableViewModel> model, int? id)
        {
            ViewBag.warehouseId = id;

            foreach (var item in model)
            {
                if (item.ToDelete)
                {
                    var itemToDelete = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode);
                    _context.Remove(itemToDelete);
                }
                if (item.IsDamaged)
                {
                    string UserIdentityName = !string.IsNullOrEmpty(User?.Identity?.Name) ? User.Identity.Name : "";
                    string loggedUserId = _context.Users.AsNoTracking().FirstOrDefault(u => u.NormalizedUserName == UserIdentityName)?.Id;

                    var itemDamaged = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode);
                    itemDamaged.State = ItemState.Damaged;
                    itemDamaged.ItemCode = ItemCodeGenerator.Generate(itemDamaged, UserIdentityName);
                    _context.Update(itemDamaged);

                    Infobox info = new Infobox
                    {
                        Title = "Przedmiot uszkodzony",
                        Message = $"Przedmiot \"{itemDamaged.Name}\" w ilości {itemDamaged.Quantity} {itemDamaged.Units} został oznaczony jako uszkodzony",
                        ReceivedDate = DateTime.Now,
                        UserId = string.IsNullOrEmpty(itemDamaged.UserId) ? loggedUserId : itemDamaged.UserId
                    };

                    if (!string.IsNullOrEmpty(info.Title))
                    {
                        _context.Infoboxes.Add(info);
                    }

                    break;
                }
            }

            await _context.SaveChangesAsync();

            if (model.Count == 0)
            {
                ViewBag.ExceptionTitle = "Zakończono inwentaryzację!";
                ViewBag.ExceptionMessage = "Poprawność można zweryfikować w zakładce przedmioty";
                return View("GlobalExceptionHandler");
            }

            return View(model);
        }

        public async Task<IActionResult> StocktakingEnd(List<StocktakingTableViewModel> model)
        {
            
            foreach (var item in model)
            {
                var itemToUpdate = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode);

                if (itemToUpdate != null)
                {
                    itemToUpdate.Quantity = Convert.ToDecimal(item.ItemQuantity);
                    itemToUpdate.Units = item.ItemUnit;
                    _context.Update(itemToUpdate);
                }
            }
            await _context.SaveChangesAsync();

            ViewBag.ExceptionTitle = "Zakończono inwentaryzację!";
            ViewBag.ExceptionMessage = "Poprawność można zweryfikować w zakładce przedmioty";
            return View("GlobalExceptionHandler");
        }
        /// <summary>
        /// A private method responsible for checking if there is a warehouse with the given id
        /// </summary>
        /// <param name="id">Warehouse ID to check</param>
        /// <returns>Returns true if the warehouse exists. Otherwise - false.</returns>
        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }
    }
}
