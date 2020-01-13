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
    public class ItemsController : Controller
    {
        private readonly DotNetWMSContext _context;
        private static string ItemCode;


        public ItemsController(DotNetWMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Assign_test()
        {
            var dotNetWMSContext = _context.Items.Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
            return View(await dotNetWMSContext.ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Assign_save(int? id)
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_save(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
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
                return RedirectToAction("Assign_test");
            }
            ViewData["ItemAssign"] = new SelectList(_context.Items, "Id", "Assign");
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View(item);
        }


        // GET: Items
        public async Task<IActionResult> Index()
        {
            var dotNetWMSContext = _context.Items.Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
            return View(await dotNetWMSContext.ToListAsync());
        }

        // GET: Items/Details/5
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

        // GET: Items/Create
        public IActionResult Create()
        {
            
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {
            bool isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode);
            if (ModelState.IsValid)
            {
                if (!isItemExists)
                {
                    _context.Add(item);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                }
                
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsItemExists(string itemCode)
        {
            bool isItemExists = _context.Items.Any(i => i.ItemCode == itemCode);

            if (!isItemExists)
            {
                return Json(true);
            }
            else
            {
                return Json($"Przedmiot o kodzie ({itemCode}) został już wprowadzony!");
            }
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            ItemCode = item.ItemCode;
            if (item == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }
            bool isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode && i.ItemCode != ItemCode);
            if (ModelState.IsValid)
            {
                if (!isItemExists)
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
                else
                {
                    ModelState.AddModelError(string.Empty, $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                }
                
                
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }

        // GET: Items/Delete/5
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

        // POST: Items/Delete/5
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
