using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetWMS.Controllers
{
    public class Doc_AssignmentsController : Controller
    {
        private readonly DotNetWMSContext _context;

        public Doc_AssignmentsController(DotNetWMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string search)
        {
            ViewData["Search"] = search;

            var docs = _context.Doc_Assignments.Select(d => d);

            if (!string.IsNullOrEmpty(search))
            {
                docs = docs.Where(d => d.DocumentId == search || d.CreationDate.ToString() == search);
            }

            return View(await docs.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> GenerateDocument(Doc_Assignment doc)
        {

            Doc_Assignment document;

            if (ModelState.IsValid)
            {
                document = new Doc_Assignment()
                {
                    DocumentId = DocumentIdGenerator(),
                    Title = DocumentTitleGenerator(),
                    CreationDate = DateTime.Now,
                    UserFrom = doc.UserFrom,
                    UserTo = doc.UserTo,
                    WarehouseFrom = doc.WarehouseFrom,
                    WarehouseTo = doc.WarehouseTo,
                    ExternalFrom = doc.ExternalFrom,
                    ExternalTo = doc.ExternalTo,
                    ItemsToString = doc.Items.ToString()

                };


            }

            
            

            return View();
        }

        public async Task<IActionResult> SaveDocument(Doc_Assignment doc)
        {
            if (ModelState.IsValid)
            {
                _context.Add(doc);
                await _context.SaveChangesAsync();
                ViewBag.ExceptionTitle = "Dodano dokument do bazy!";
                ViewBag.ExceptionMessage = $"{doc.DocumentId}";
                return View("GlobalExceptionHandler");
            }

            return View();
        }

        public async Task<IActionResult> ShowDocument(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var doc = _context.Doc_Assignments.FirstOrDefaultAsync(d => d.DocumentId == id);

            return View();
        }

        public async Task<IActionResult> DeleteDocument()
        {


            return View();
        }
        private string DocumentIdGenerator()
        {
            return "";
        }

        private string DocumentTitleGenerator()
        {
            return "";
        }

        // GET: Doc_AssignmentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Doc_AssignmentsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Doc_AssignmentsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Doc_AssignmentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Doc_AssignmentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Doc_AssignmentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Doc_AssignmentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
