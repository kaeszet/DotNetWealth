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
    [Authorize(Roles = "Standard,StandardPlus,Moderator")]
    public class ExternalsController : Controller
    {
        private readonly DotNetWMSContext _context;

        public ExternalsController(DotNetWMSContext context)
        {
            _context = context;
        }
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

            switch (order)
            {
                case "name_desc":
                    externals = externals.OrderByDescending(w => w.Name);
                    break;
                case "type_desc":
                    externals = externals.OrderByDescending(e => e.Type);
                    break;
                case "Type":
                    externals = externals.OrderBy(e => e.Type);
                    break;
                default:
                    externals = externals.OrderBy(e => e.Name);
                    break;
            }
            return View(await externals.AsNoTracking().ToListAsync());
        }
        public IActionResult StatusView(StatusViewViewModel model)
        {
            model.Items = _context.Items.Select(i => i).Where(i => i.State == model.State).ToList();
            return View(model);
        }
        [HttpPost]
        public IActionResult StatusView(ItemState state)
        {

            var items = _context.Items.Select(i => i).Where(i => i.State == state).ToList();
            ViewData["ItemList"] = items;

            return PartialView("_StatusViewTable", items);
        }
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

        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Create()
        {
            return View();
        }

        [Authorize(Roles = "StandardPlus,Moderator")]
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

        [Authorize(Roles = "StandardPlus,Moderator")]
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

        [Authorize(Roles = "StandardPlus,Moderator")]
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

        private bool ExternalExists(int id)
        {
            return _context.Externals.Any(e => e.Id == id);
        }
    }
}
