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
    [Authorize]
    public class WarehousesController : Controller
    {
        private readonly DotNetWMSContext _context;
        private static string Name;
        private static int? wrhId;

        public WarehousesController(DotNetWMSContext context)
        {
            _context = context;
        }
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> StocktakingIndex(string search)
        {
            wrhId = Convert.ToInt32(search);
            ViewData["Hide"] = wrhId == 0 ? "d-none" : "";
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", wrhId);
            ViewData["Search"] = search;

            var items = _context.Items.Select(e => e).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);


            if (!string.IsNullOrEmpty(search))
            {
                
                items = items.Where(i => i.WarehouseId.ToString().Contains(search)).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
            }

            return View(await items.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Test_st(string warehouseId, string searchString)
        {
            // Use LINQ to get list of genres.
            IQueryable<int?> ItemQuery = from i in _context.Items
                                            orderby i.WarehouseId
                                            select i.WarehouseId;

            //IQueryable<string> WarehouseQuery = _context.Warehouses.Where(w => w.Id == ItemQuery).Select(w => w.AssignFullName);



            var items = _context.Items.Select(i => i);

            if (!string.IsNullOrEmpty(searchString))
            {
                items = items.Where(i => i.ItemCode.Contains(searchString));
            }

            if (!string.IsNullOrEmpty(warehouseId))
            {
                items = items.Where(i => i.WarehouseId == Convert.ToInt32(warehouseId));
            }

            var stocktakingVM = new StocktakingViewModel
            {
                Warehouses = new SelectList(await ItemQuery.Distinct().ToListAsync()),
                Items = await items.ToListAsync()
            };

            return View(stocktakingVM);
        }
        //[Authorize(Roles = "StandardPlus,Moderator")]
        //[HttpGet]
        //public IActionResult Stocktaking(string filter)
        //{
        //    ViewData["Filter"] = filter;

        //    var items = from i in _context.Items select i;
        //    //var items = _context.Items.Select(i => i);
        //    var wrh = _context.Warehouses.FirstOrDefault(w => w.Id == Convert.ToInt32(filter));

        //    if (!string.IsNullOrEmpty(filter))
        //    {
        //        items = items.Where(i => i.WarehouseId == wrh.Id && i.ExternalId == null);
        //    }
        //    return View(items.AsNoTracking().ToList());
        //}
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
        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Create()
        {
            return View();
        }
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

        private bool WarehouseExists(int id)
        {
            return _context.Warehouses.Any(e => e.Id == id);
        }
    }
}
