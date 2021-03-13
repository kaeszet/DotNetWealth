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
using Microsoft.Extensions.Logging;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class to support the CRUD process for the Department model
    /// </summary>
    [Authorize(Roles = "Kadry,Moderator")]
    public class DepartmentsController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<DepartmentsController> _logger;

        public DepartmentsController(DotNetWMSContext context, ILogger<DepartmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning an Department's Index view
        /// </summary>
        /// <returns>Returns Department's Index view</returns>
        public async Task<IActionResult> Index() => View(await _context.Departments.ToListAsync());
        /// <summary>
        /// GET method responsible for returning an Department's Details view
        /// </summary>
        /// <param name="id">Department ID which should be returned</param>
        /// <returns>Department's Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null");
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie stanowiska o podanym id = {id}");
                return NotFound();
            }

            return View(department);
        }
        /// <summary>
        /// GET method responsible for returning an Department's Create view
        /// </summary>
        /// <returns>Returns Department's Create view</returns>
        public IActionResult Create()
        {
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="department">Department model class with binding DB attributes</param>
        /// <returns>Returns Department's Create view, data validation on the model side</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Department department)
        {

            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        /// <summary>
        /// GET/POST method responsible for checking whether the department with the given name exists in the DB
        /// </summary>
        /// <param name="name">Department name to be checked</param>
        /// <returns>Returns true in JSON format if the department does not exist. Otherwise - a message with an error</returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsDepartmentExists(string name)
        {
            bool isDeptExists = _context.Departments.Any(d => d.Name == name);

            if (!isDeptExists)
            {
                return Json(true);
            }
            else
            {
                _logger.LogDebug($"DEBUG: Stanowisko {name} jest już w bazie danych");
                return Json($"Podane stanowisko ({name}) już istnieje!");
            }
        }
        /// <summary>
        /// GET method responsible for returning an Department's Edit view
        /// </summary>
        /// <param name="id">Department ID which should be returned</param>
        /// <returns>Returns Department's Edit view</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("DEBUG: Wprowadzony idetyfikator ma wartość null");
                return NotFound();
            }

            var department = await _context.Departments.FindAsync(id);

            if (department == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie stanowiska o podanym id = {id}");
                return NotFound();
            }
            return View(department);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="id">Department ID to edit</param>
        /// <param name="department">Department model class with binding DB attributes</param>
        /// <returns>If succeed, returns Department's Index view, data validation on the model side</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Department department)
        {
            if (id != department.Id)
            {
                _logger.LogDebug("DEBUG: Wprowadzony idetyfikator ma wartość null");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(department);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentExists(department.Id))
                    {
                        _logger.LogError($"Stanowisko {department.Name} zostało zmienione przez innego użytkownika");
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }
        /// <summary>
        /// GET method responsible for returning an Department's Delete view
        /// </summary>
        /// <param name="id">Department ID to delete</param>
        /// <returns>Returns Department's Delete view if exists</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug("DEBUG: Wprowadzony idetyfikator ma wartość null");
                return NotFound();
            }

            var department = await _context.Departments.FirstOrDefaultAsync(m => m.Id == id);

            if (department == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie stanowiska o podanym id = {id}");
                return NotFound();
            }

            return View(department);
        }
        /// <summary>
        /// POST method responsible for removing the department from DB if the user confirms this action
        /// </summary>
        /// <param name="id">Department ID to delete</param>
        /// <returns>Returns Department's Index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var department = await _context.Departments.FindAsync(id);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Private method responsible for checking if there is a department with the given id
        /// </summary>
        /// <param name="id">Department ID to check</param>
        /// <returns>Returns true if the department exists. Otherwise - false.</returns>
        private bool DepartmentExists(int id)
        {
            return _context.Departments.Any(e => e.Id == id);
        }
    }
}
