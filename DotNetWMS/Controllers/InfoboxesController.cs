﻿using System;
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
    /// Controller class responsible for <c>Infobox</c> functionality
    /// </summary>
    [Authorize]
    public class InfoboxesController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<InfoboxesController> _logger;

        public InfoboxesController(DotNetWMSContext context, ILogger<InfoboxesController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning an Infobox's Index view
        /// </summary>
        /// <param name="isChecked">Sort names in ascending or descending order</param>
        /// <returns>Returns Warehouse's Index view with list of warehouses in the order set by the user</returns>
        public async Task<IActionResult> Index(bool isChecked)
        {
            ViewData["IsChecked"] = !isChecked;
            
            var infos = _context.Infoboxes.Where(i => i.User.NormalizedUserName == User.Identity.Name).OrderByDescending(i => i.ReceivedDate).Include(u => u.User);
            GlobalAlert.SendQuantity(infos.Count(i => i.IsChecked == false));
            return View(await infos.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// Remove message from infobox
        /// </summary>
        /// <param name="id">ID of infobox to delete</param>
        /// <returns>Infobox's Index View</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return View("NotFound");
            }

            var info = await _context.Infoboxes.FindAsync(id);

            if (info == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono wiadomości o ID = {id}");
                return View("NotFound");
            }

            _context.Infoboxes.Remove(info);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> DeleteAllChecked()
        {
            var infosToDelete = _context.Infoboxes.Where(i => i.User.NormalizedUserName == User.Identity.Name && i.IsChecked == true);

            if (infosToDelete.Count() == 0)
            {
                GlobalAlert.SendGlobalAlert($"Brak wiadomości do usunięcia...", "info");
                _logger.LogInformation($"INFO: Brak wiadomości do usunięcia");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _context.Infoboxes.RemoveRange(infosToDelete);
                await _context.SaveChangesAsync();
                GlobalAlert.SendGlobalAlert($"Usunięto wszystkie przeczytane wiadomości!", "info");
                return RedirectToAction(nameof(Index));
            }
           

        }
        /// <summary>
        /// Method to mark messages as checked or documents as accepted
        /// </summary>
        /// <param name="id">ID of infobox to mark</param>
        /// <returns>Infobox's Index View</returns>
        public async Task<IActionResult> Check(int? id)
        {

            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return View("NotFound");
            }

            var info = await _context.Infoboxes.FindAsync(id);

            if (info == null)
            {
                _logger.LogDebug($"DEBUG: Nie odnaleziono wiadomości o podanym ID = {id}");

                ViewBag.ErrorMessage = $"Nie odnaleziono wiadomości o podanym ID = {id}";

                return View("NotFound");
            }

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
