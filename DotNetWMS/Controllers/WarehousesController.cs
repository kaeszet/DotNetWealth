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
        /// A static field for handling Warehouse's name for properly creation of Stocktaking view
        /// </summary>
        private static string Name;
        /// <summary>
        /// A static field for handling Warehouse's ID for properly creation of Stocktaking view
        /// </summary>
        private static int? wrhId;

        public WarehousesController(DotNetWMSContext context)
        {
            _context = context;
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
                warehouses = warehouses.Where(w => w.Name.Contains(search) || w.Street.Contains(search));
            }

            switch (order)
            {
                case "name_desc":
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
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

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
                    _context.Add(warehouse);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
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
                return NotFound();
            }

            var warehouse = await _context.Warehouses.FindAsync(id);
            Name = warehouse.Name;

            if (warehouse == null)
            {
                return NotFound();
            }
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
                    ModelState.AddModelError(string.Empty, $"Magazyn \"{warehouse.Name}\" został już wprowadzony do systemu! Wybierz inną nazwę.");
                }
                
            }
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
                return NotFound();
            }

            var warehouse = await _context.Warehouses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (warehouse == null)
            {
                return NotFound();
            }

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
