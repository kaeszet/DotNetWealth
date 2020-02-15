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
    public class StatePreviewController : Controller
    {
        private readonly DotNetWMSContext _context;

        public StatePreviewController(DotNetWMSContext context)
        {
            _context = context;
        }

        // GET: StatePreview
        public async Task<IActionResult> Index()
        {
            var dotNetWMSContext = _context.Items.Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
            return View(await dotNetWMSContext.ToListAsync());
        }

        // GET: StatePreview/Details/5
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

        // GET: StatePreview/Create
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "City");
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Name");
            return View();
        }

        // POST: StatePreview/Create
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

        // GET: StatePreview/Edit/5
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

        // POST: StatePreview/Edit/5
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

        // GET: StatePreview/Delete/5
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

        // POST: StatePreview/Delete/5
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
    }
}
