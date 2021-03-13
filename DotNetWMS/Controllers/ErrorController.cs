using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotNetWMS.Controllers
{
    public class ErrorController : Controller
    {
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger;
        }
        /// <summary>
        /// The method responsible for displaying the error message depending on the error number
        /// </summary>
        /// <param name="statusCode">Error code</param>
        /// <returns>Returns an error code dependent view</returns>
        [Route("Error/{statusCode}")]
        public IActionResult HttpStatusCodeHandler(int statusCode)
        {
            var statusCodeResult = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    ViewBag.ErrorMessage = "Strona pod podanym adresem nie istnieje!";
                    ViewBag.Path = statusCodeResult.OriginalPath;
                    ViewBag.QS = statusCodeResult.OriginalQueryString;
                    _logger.LogWarning($"Błąd 404. Ścieżka = {statusCodeResult.OriginalPath}, QueryString = {statusCodeResult.OriginalQueryString}");
                    break;
            }

            return View("NotFound");
        }
        /// <summary>
        /// The method responsible for displaying the error message if the error is of a global type
        /// </summary>
        /// <returns>Returns a view with an error message containing the data captured in the method</returns>
        [Route("Error")]
        public IActionResult GlobalExceptionHandler()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            _logger.LogError($"Ścieżka {exceptionHandlerPathFeature.Path} zwróciła błąd {exceptionHandlerPathFeature.Error}");

            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            ViewBag.ContactEmail = "Skontaktuj się: kamil.szydlowski@microsoft.wsei.edu.pl";

            return View();
        }
    }
}