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

namespace DotNetWMS.Controllers
{
    public class InfoboxesController : Controller
    {
        private readonly DotNetWMSContext _context;

        public InfoboxesController(DotNetWMSContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(bool isChecked)
        {
            ViewData["IsChecked"] = !isChecked;
            
            var infos = _context.Infoboxes.Where(i => i.User.NormalizedUserName == User.Identity.Name).OrderByDescending(i => i.ReceivedDate).Include(u => u.User);
            GlobalAlert.SendQuantity(infos.Count(i => i.IsChecked == false));
            return View(await infos.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                NotFound();
            }

            var info = await _context.Infoboxes.FindAsync(id);

            _context.Infoboxes.Remove(info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Check(int? id)
        {

            if (id == null)
            {
                return View("NotFound");
            }

            var info = await _context.Infoboxes.FindAsync(id);

            if (!string.IsNullOrEmpty(info.DocumentId))
            {
                var doc = _context.Doc_Assignments.Find(info.DocumentId);

                if (doc != null)
                {
                    doc.IsConfirmed = true;
                    info.IsChecked = true;
                    doc.ConfirmationDate = DateTime.Now;
                    _context.Update(doc);
                    await _context.SaveChangesAsync();
                }
            }
            else
            {
                info.IsChecked = !info.IsChecked;
            }
            
            _context.Update(info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
        
}
