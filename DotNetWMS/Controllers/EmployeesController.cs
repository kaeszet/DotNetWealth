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
    /// <summary>
    /// Controller class to support the CRUD process for the Employee model
    /// </summary>
    [Authorize(Roles = "Kadry,Moderator")]
    public class EmployeesController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Universal Electronic System for Registration of the Population number
        /// </summary>
        private static string Pesel;

        public EmployeesController(DotNetWMSContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET method responsible for returning an Employee's Index view and supports a search engine
        /// </summary>
        /// <param name="order">Sort surnames in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Employee's Index view with list of employees in the order set by the user</returns>
        public async Task<IActionResult> Index(string order, string search)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["Search"] = search;
            
            var employees = _context.Employees.Select(e => e);

            if (!string.IsNullOrEmpty(search))
            {
                employees = employees.Where(e => e.Surname.Contains(search) || e.Name.Contains(search) || e.Pesel.Contains(search));
            }

            switch (order)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(e => e.Surname).Include(e => e.Department);
                    break;
                default:
                    employees = employees.OrderBy(e => e.Surname).Include(e => e.Department);
                    break;
            }
            return View(await employees.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an Employee's Details view
        /// </summary>
        /// <param name="id">Employee ID which should be returned</param>
        /// <returns>Employee's Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        /// <summary>
        /// GET method responsible for returning an Employee's Create view
        /// </summary>
        /// <returns>Returns Employee's Create view</returns>
        public IActionResult Create()
        {
            
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="employee">Employee model class with binding DB attributes</param>
        /// <returns>If succeed, returns Employee's Index view. Otherwise - show error message</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Surname,Pesel,DepartmentId,Street,ZipCode,City")] Employee employee)
        {
            bool isEmployeeExists = _context.Employees.Any(i => i.Pesel == employee.Pesel);
            if (ModelState.IsValid)
            {
                if (!isEmployeeExists)
                {
                    _context.Add(employee);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Pracownik {employee.FullName} został już wprowadzony do systemu!");
                }
               
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            
            return View(employee);
        }
        /// <summary>
        /// GET method responsible for returning an Employee's Edit view
        /// </summary>
        /// <param name="id">Employee ID which should be returned</param>
        /// <returns>Returns Employee's Edit view</returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var employee = await _context.Employees.FindAsync(id);
            
            if (employee == null)
            {
                return NotFound();
            }
            Pesel = employee.Pesel;
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="id">Employee ID to edit</param>
        /// <param name="employee">Employee model class with binding DB attributes</param>
        /// <returns>If succeed, returns Department's Index view, data validation on the model side</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Surname,Pesel,DepartmentId,Street,ZipCode,City")] Employee employee)
        {
            if (id != employee.Id || !EmployeeExists(employee.Id))
            {
                return NotFound();
            }

            bool isEmployeeExists = _context.Employees.Any(e => e.Pesel == employee.Pesel && e.Pesel != Pesel && !string.IsNullOrEmpty(Pesel));
            if (ModelState.IsValid)
            {
                if (!isEmployeeExists)
                {
                    try
                    {
                        _context.Update(employee);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!EmployeeExists(employee.Id))
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
                    ModelState.AddModelError(string.Empty, $"Pracownik {employee.FullName} został już wprowadzony do systemu!");
                }
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name", employee.DepartmentId);
            return View(employee);
        }
        /// <summary>
        /// GET method responsible for returning an Employee's Delete view
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        /// <returns>Returns Employee's Delete view if exists</returns>
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }
        /// <summary>
        /// POST method responsible for removing employee from DB if the user confirms this action
        /// </summary>
        /// <param name="id">Employee ID to delete</param>
        /// <returns>Returns Employee's Index view</returns>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            try
            {
                _context.Employees.Remove(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ViewBag.ErrorTitle = "Podczas usuwania pracownika wystąpił błąd!";
                ViewBag.ErrorMessage = $"Istnieje przedmiot przypisany do tego pracownika.<br>" +
                    $"Przed usunięciem pracownika upewnij się, że zdał wszystkie przedmioty.<br>" +
                    $"Odznacz je w dziale \"Majątek\" i ponów próbę.";
                return View("DbExceptionHandler");
            }
            
        }
        /// <summary>
        /// A private method responsible for checking if there is a employee with the given id
        /// </summary>
        /// <param name="id">Employee ID to check</param>
        /// <returns>Returns true if the employee exists. Otherwise - false.</returns>
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
