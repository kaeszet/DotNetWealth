using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;

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

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning a Home's Index view
        /// </summary>
        /// <returns>Returns an Home's Index view</returns>
        public IActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// GET method responsible for returning a Home's AboutUs view
        /// </summary>
        /// <returns>Returns an Home's AboutUs view</returns>
        public IActionResult AboutUs()
        {
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
