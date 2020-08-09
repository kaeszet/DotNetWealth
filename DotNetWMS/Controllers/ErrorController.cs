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
        /// Controller class to support displaying error messages
        /// </summary>
        private readonly ILogger logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            this.logger = logger;
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
                    logger.LogWarning($"404 error occured. Path = {statusCodeResult.OriginalPath} and QueryString = {statusCodeResult.OriginalQueryString}");
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
            logger.LogError($"The path {exceptionHandlerPathFeature.Path} threw an exception {exceptionHandlerPathFeature.Error}");

            ViewBag.ExceptionPath = exceptionHandlerPathFeature.Path;
            ViewBag.ExceptionMessage = exceptionHandlerPathFeature.Error.Message;
            ViewBag.StackTrace = exceptionHandlerPathFeature.Error.StackTrace;
            ViewBag.ContactEmail = "Skontaktuj się: kamil.szydlowski@microsoft.wsei.edu.pl";

            return View();
        }
    }
}