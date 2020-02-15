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
    public class StocktakingController : Controller
    {
        private readonly DotNetWMSContext _context;

        public StocktakingController(DotNetWMSContext context)
        {
            _context = context;
        }

        // GET: Stocktaking
        public async Task<IActionResult> Index()
        {
            var dotNetWMSContext = _context.Items.Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
            return View(await dotNetWMSContext.ToListAsync());
        }

        // GET: Stocktaking/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Employee)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Stocktaking/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "City");
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name");
            return View();
        }

        // POST: Stocktaking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Name,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {
            if (ModelState.IsValid)
            {
                _context.Add(item);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "City", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name", item.WarehouseId);
            return View(item);
        }

        // GET: Stocktaking/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "City", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name", item.WarehouseId);
            return View(item);
        }

        // POST: Stocktaking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Name,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(item);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ItemExists(item.Id))
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "City", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name", item.WarehouseId);
            return View(item);
        }

        // GET: Stocktaking/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Employee)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Stocktaking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
     
        }
        public IActionResult StocktakingIndex()
        {
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
            return View();
        }
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
    }
}
