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
        private readonly ILogger _logger;
        /// <summary>
        /// A static field for handling Warehouse's name for properly creation of Stocktaking view
        /// </summary>
        private static string Name;

        public WarehousesController(DotNetWMSContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Stocktaking view
        /// </summary>
        /// <param name="model">StocktakingViewModel class object to start the inventory procedure</param>
        /// <returns>Returns Warehouse's Stocktaking view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Stocktaking(StocktakingViewModel model)
        {
            model.Items = _context.Items.Select(i => i).ToList();
            ViewData["WarehouseList"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
            return View(model);
        }
        /// <summary>
        /// POST method to display the list of items existing in the previously selected warehouse
        /// </summary>
        /// <param name="warehouseFullName">Defined the full name of the warehouse</param>
        /// <returns>Returns Warehouse's Stocktaking view and items existing in the previously selected warehouse</returns>
        [HttpPost]
        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Stocktaking(string warehouseFullName)
        {
            //var warehouse = _context.Warehouses.FirstOrDefault(w => w.AssignFullName == warehouseFullName);
            if (string.IsNullOrEmpty(warehouseFullName))
            {
                return NotFound();
            }
            int warehouseId = Convert.ToInt32(warehouseFullName);
            var items = _context.Items.Select(i => i).Where(i => i.WarehouseId == warehouseId).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
            ViewData["ItemList"] = items;

            return PartialView("_StocktakingTable", items);
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
                _logger.LogDebug($"DEBUG: Magazyn o podanym id = {id} nie istnieje!");
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.Id == id);

            if (warehouse == null)
            {
                _logger.LogDebug($"DEBUG: Magazyn {warehouse} nie istnieje!");
                return NotFound();
            }
            GoogleMapsGenerator gmg = new GoogleMapsGenerator();
            TempData["Adress"] = gmg.PrepareAdressToGeoCode(warehouse);
            _logger.LogDebug($"DEBUG: Pomyślnie zwrócono widok szczegółów!");
            return View(warehouse);
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Create view
        /// </summary>
        /// <returns>Returns Warehouse's Create view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="warehouse">Warehouse model class with binding DB attributes</param>
        /// <returns>If succeed, returns Warehouse's Index view. Otherwise - show error message</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Street,ZipCode,City")] Warehouse warehouse)
        {
            bool isWarehouseExists = _context.Warehouses.Any(w => w.Name == warehouse.Name);

            if (ModelState.IsValid)
            {
                if (!isWarehouseExists)
                {
                    _logger.LogDebug($"DEBUG: Dodano magazyn!");

                    _context.Add(warehouse);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogDebug($"DEBUG: Taka nazwa magazynu już istnieje!");
                    ModelState.AddModelError(string.Empty, $"Magazyn {warehouse.Name} został już wprowadzony do systemu! Wybierz inną nazwę.");
                }
                
            }
            return View(warehouse);
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Edit view
        /// </summary>
        /// <param name="id">Warehouse ID which should be returned</param>
        /// <returns>Returns Warehouse's Edit view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Takie id = {id} nie istniej!");
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            Name = warehouse.Name;

            if (warehouse == null)
            {
                _logger.LogDebug($"DEBUG: Taki magazyn = {warehouse} nie istniej!");
                return NotFound();
            }
            TempData["Adress"] = GoogleMapsGenerator.PrepareAdressToGeoCode(warehouse);
            return View(warehouse);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="id">Warehouse ID to edit</param>
        /// <param name="warehouse">Warehouse model class with binding DB attributes</param>
        /// <returns>If succeed, returns Warehouse's Index view, data validation on the model side</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Street,ZipCode,City")] Warehouse warehouse)
        {
            if (id != warehouse.Id)
            {
                _logger.LogDebug($"DEBUG: Takie id = {id} nie istniej w bazie magazynów!");
                return NotFound();
            }
            bool isWarehouseExists = _context.Warehouses.Any(w => w.Name == warehouse.Name && w.Name != Name);
            if (ModelState.IsValid)
            {
                if (!isWarehouseExists)
                {
                    try
                    {
                        _context.Update(warehouse);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!WarehouseExists(warehouse.Id))
                        {
                            _logger.LogDebug($"DEBUG: Taki magazyn nie istniej w bazie magazynów!");
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
                    _logger.LogDebug($"DEBUG: Takie nazwa magazynu zostałą jużwprowadzona do systemu!");
                    ModelState.AddModelError(string.Empty, $"Magazyn \"{warehouse.Name}\" został już wprowadzony do systemu! Wybierz inną nazwę.");
                }
                
            }
            _logger.LogDebug($"DEBUG: Pomyślnie zedytowano magazyn!");
            return View(warehouse);
        }
        /// <summary>
        /// GET method responsible for returning an Warehouse's Delete view
        /// </summary>
        /// <param name="id">Warehouse ID to delete</param>
        /// <returns>Returns Warehouse's Delete view if exists</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Magazyn o podanym id = {id} nie istnieje!");
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                _logger.LogDebug($"DEBUG: Magazyn {warehouse} nie istnieje!");
                return NotFound();
            }

            _logger.LogDebug($"DEBUG: Znaleziono magazyn {warehouse}!");
            return View(warehouse);
        }
        /// <summary>
        /// POST method responsible for removing employee from DB if the user confirms this action
        /// </summary>
        /// <param name="id">Warehouse ID to delete</param>
        /// <returns>Returns Warehouse's Index view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            try
            {
                _context.Warehouses.Remove(warehouse);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania magazynu wystąpił błąd!";
                ViewBag.ErrorMessage = $"Istnieje przedmiot przypisany do tego magazynu.<br>" +
                    $"Przed usunięciem magazynu upewnij się, że wszystkie przedmioty zostały usunięte.<br>" +
                    $"Odznacz je w dziale \"Majątek\" i ponów próbę.";
                return View("DbExceptionHandler");
            }
            
        }

        public async Task<IActionResult> StocktakingIndex(StocktakingViewModel model)
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

            var model = new List<StocktakingNewViewModel>();
            foreach (var item in itemsInWarehouse)
            {
                var stocktakingViewModel = new StocktakingNewViewModel()
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
        public async Task<IActionResult> StocktakingBegin(List<StocktakingNewViewModel> model, int? id)
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

            //model = model.Where(i => i.ToCorrect == true).ToList();

            if (model.Count == 0)
            {
                ViewBag.ExceptionTitle = "Zakończono inwentaryzację!";
                ViewBag.ExceptionMessage = "Poprawność można zweryfikować w zakładce przedmioty";
                return View("GlobalExceptionHandler");
            }

            return View(model);
        }

        public async Task<IActionResult> StocktakingEnd(List<StocktakingNewViewModel> model)
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
