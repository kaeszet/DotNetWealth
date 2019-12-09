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
    public class ExternalsController : Controller
    {
        private readonly DotNetWMSContext _context;

        public ExternalsController(DotNetWMSContext context)
        {
            _context = context;
        }

        // GET: Externals
        public async Task<IActionResult> Index()
        {
            return View(await _context.Externals.ToListAsync());
        }

        // GET: Externals/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Externals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Externals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Type,Name,TaxId,Street,ZipCode,City")] External external)
        {
            if (ModelState.IsValid)
            {
                _context.Add(external);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(external);
        }

        // GET: Externals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var external = await _context.Externals.FindAsync(id);
            if (external == null)
            {
                return NotFound();
            }
            return View(external);
        }

        // POST: Externals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Type,Name,TaxId,Street,ZipCode,City")] External external)
        {
            if (id != external.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(external);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExternalExists(external.Id))
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
            return View(external);
        }

        // GET: Externals/Delete/5
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

        // POST: Externals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var external = await _context.Externals.FindAsync(id);
            _context.Externals.Remove(external);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExternalExists(int id)
        {
            return _context.Externals.Any(e => e.Id == id);
        }
    }
}
