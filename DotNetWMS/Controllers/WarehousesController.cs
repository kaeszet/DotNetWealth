using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetWMS.Data;
using DotNetWMS.Models;

namespace DotNetWMS.Controllers
{
    public class WarehousesController : Controller
    {
        private readonly DotNetWMSContext _context;
        private static string Name;

        public WarehousesController(DotNetWMSContext context)
        {
            _context = context;
        }
        public IActionResult StocktakingIndex()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Stocktaking(string filter)
        {
            ViewData["Filter"] = filter;

            //var items = from i in _context.Items select i;
            var items = _context.Items.Select(i => i);
            var wrh = _context.Warehouses.FirstOrDefaultAsync(w => w.AssignFullName == filter);

            if (!string.IsNullOrEmpty(filter))
            {
                items = items.Where(i => i.WarehouseId == wrh.Id);
            }
            return PartialView("Stocktaking_warehouse_list", await items.AsNoTracking().ToListAsync());
            //var wrh = _context.Warehouses.FirstOrDefaultAsync(w => w.AssignFullName == filter);
            //if (wrh != null)
            //{
            //    return PartialView();
            //};
        }
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
        // GET: Warehouses
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Warehouses.ToListAsync());
        //}

        // GET: Warehouses/Details/5
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

        // GET: Warehouses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Warehouses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Warehouses/Edit/5
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

        // POST: Warehouses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
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

        // GET: Warehouses/Delete/5
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

        // POST: Warehouses/Delete/5
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
