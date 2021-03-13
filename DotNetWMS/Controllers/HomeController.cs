using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;
using DotNetWMS.Data;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class to support displaying the home page
    /// </summary>
    [AllowAnonymous]
    public class HomeController : Controller
    {
        /// <summary>
        /// The field responsible for communication with Nlog
        /// </summary>
        private readonly ILogger<HomeController> _logger;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly DotNetWMSContext _context;

        public HomeController(ILogger<HomeController> logger, DotNetWMSContext context)
        {
            _logger = logger;
            _context = context;
        }
        /// <summary>
        /// GET method responsible for returning a Home's Index view
        /// </summary>
        /// <returns>Returns an Home's Index view</returns>
        public async Task<IActionResult> Index()
        {
            if (User?.Identity?.Name != null)
            {
                ViewData["isAnyNewMessages"] = _context.Infoboxes.Any(m => m.IsChecked == false && m.User.NormalizedUserName == User.Identity.Name);
            }
            
            await SeedDatabase.InitializeUsers(_context);

            ViewData["ExternalsCount"] = _context.Externals.Count();
            ViewData["WarehousesCount"] = _context.Warehouses.Count();
            ViewData["RegisteredUsers"] = _context.Users.Count();
            ViewData["OutOfWarranty"] = _context.Items.Where(i => i.WarrantyDate < DateTime.Now).Count();
            int newMessages = _context.Infoboxes.Where(i => i.User.NormalizedUserName == User.Identity.Name && i.IsChecked == false).Count();
            GlobalAlert.SendQuantity(newMessages);

            return View();
        }
        /// <summary>
        /// GET method responsible for returning a Home's AboutUs view
        /// </summary>
        /// <returns>Returns an Home's AboutUs view</returns>
        public async Task<IActionResult> AboutUs()
        {
            await SeedDatabase.InitializeData(_context);
            return View();
        }
        /// <summary>
        /// GET method responsible for returning an Error view
        /// </summary>
        /// <returns>Returns a Error view</returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
