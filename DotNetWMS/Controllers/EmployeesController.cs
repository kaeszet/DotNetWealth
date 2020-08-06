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
    [Authorize(Roles = "Kadry,Moderator")]
    public class EmployeesController : Controller
    {
        private readonly DotNetWMSContext _context;
        private static string Pesel;

        public EmployeesController(DotNetWMSContext context)
        {
            _context = context;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        /// <param name="search"></param>
        /// <returns></returns>
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

        public IActionResult Create()
        {
            
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Name");
            
            return View();
        }

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

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
