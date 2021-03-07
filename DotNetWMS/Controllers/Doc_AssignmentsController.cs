using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
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

        public async Task<IActionResult> Index(string search, string order)
        {
            ViewData["SortById"] = string.IsNullOrEmpty(order) ? "id_desc" : "";
            ViewData["Search"] = search;

            var docs = _context.Doc_Assignments.Select(d => d);

            if (!string.IsNullOrEmpty(search))
            {
                docs = docs.Where(d => d.DocumentId == search || d.CreationDate.ToString() == search);
            }

            switch (order)
            {
                case "id_desc":
                    docs = docs.OrderByDescending(d => d.DocumentId);
                    break;
                default:
                    docs = docs.OrderBy(d => d.DocumentId);
                    break;
            }

            return View(await docs.AsNoTracking().ToListAsync());
        }
        [HttpGet]
        public IActionResult ConfigureDocument()
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["Externals"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["Warehouses"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
            return View();
        }
        [HttpPost]
        public IActionResult ConfigureDocument(string from, string to, int fromIndex, int toIndex)
        {
            IQueryable<Item> items;

            switch (toIndex)
            {
                case 0:
                    items = _context.Items.Where(i => i.UserId == to);
                    break;
                case 1:
                    items = _context.Items.Where(i => i.WarehouseId == Convert.ToInt32(to));
                    break;
                case 2:
                    items = _context.Items.Where(i => i.ExternalId == Convert.ToInt32(to));
                    break;
                default:
                    items = _context.Items.Where(i => i.User.NormalizedUserName == User.Identity.Name);
                    break;
            }

            List<Doc_ConfigureDocumentViewModel> viewModel = new List<Doc_ConfigureDocumentViewModel>();

            foreach (var item in items)
            {
                Doc_ConfigureDocumentViewModel position = new Doc_ConfigureDocumentViewModel()
                {
                    From = from,
                    To = to,
                    FromIndex = fromIndex,
                    ToIndex = toIndex,
                    Id = item.Id,
                    Type = item.Type,
                    Name = item.Name,
                    Model = item.Model,
                    Producer = item.Producer,
                    Code = item.ItemCode,
                    Quantity = item.Quantity.ToString(),
                    Unit = item.Units

                };

                viewModel.Add(position);
            }

            return PartialView("_Doc_AssignmentConfDocPartial", viewModel);
        }

        public IActionResult GenerateDocument(List<Doc_ConfigureDocumentViewModel> viewModel)
        {
            DateTime currentDate = DateTime.Now;

            List<string> itemIds = new List<string>();
            Doc_Titles title = DocumentTitleGenerator(viewModel[0].FromIndex, viewModel[0].ToIndex);

            if (viewModel != null)
            {
                foreach (var item in viewModel)
                {
                    if (item.AddToDocument) itemIds.Add(item.Id.ToString());
                }
            }

            var items = _context.Items.Where(i => itemIds.Contains(i.Id.ToString())).ToList();

            Doc_Assignment doc = new Doc_Assignment
            {
                DocumentId = DocumentIdGenerator(currentDate, title),
                Title = title.ToString(),
                CreationDate = currentDate,
                UserFrom = viewModel[0].FromIndex == 0 ? viewModel[0].From : "",
                UserFromName = viewModel[0].FromIndex == 0 ? _context.Users.Find(viewModel[0].From).FullNameForDocumentation : "",
                UserTo = viewModel[0].ToIndex == 0 ? viewModel[0].To : "",
                UserToName = viewModel[0].ToIndex == 0 ? _context.Users.Find(viewModel[0].To).FullNameForDocumentation : "",
                WarehouseFrom = viewModel[0].FromIndex == 1 ? Convert.ToInt32(viewModel[0].From) : 0,
                WarehouseFromName = viewModel[0].FromIndex == 1 ? _context.Warehouses.Find(Convert.ToInt32(viewModel[0].From)).FullNameForDocumentation : "",
                WarehouseTo = viewModel[0].ToIndex == 1 ? Convert.ToInt32(viewModel[0].To) : 0,
                WarehouseToName = viewModel[0].ToIndex == 1 ? _context.Warehouses.Find(Convert.ToInt32(viewModel[0].To)).FullNameForDocumentation : "",
                ExternalFrom = viewModel[0].FromIndex == 2 ? Convert.ToInt32(viewModel[0].From) : 0,
                ExternalFromName = viewModel[0].FromIndex == 2 ? _context.Externals.Find(Convert.ToInt32(viewModel[0].From)).FullNameForDocumentation : "",
                ExternalTo = viewModel[0].ToIndex == 2 ? Convert.ToInt32(viewModel[0].To) : 0,
                ExternalToName = viewModel[0].ToIndex == 2 ? _context.Externals.Find(Convert.ToInt32(viewModel[0].To)).FullNameForDocumentation : "",
                Items = items
            };

            ViewData["DocumentTitle"] = title.GetType().GetMember(title.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name;
            return View(doc);
        }

        public async Task<IActionResult> SaveDocument(Doc_Assignment doc)
        {
            if (ModelState.IsValid)
            {
                AddToInfobox(doc);
                _context.Add(doc);
                await _context.SaveChangesAsync();
                ViewBag.ExceptionTitle = "Dodano dokument do bazy!";
                ViewBag.ExceptionMessage = $"{doc.DocumentId}";
                return View("GlobalExceptionHandler");
            }

            return NotFound();
        }

        public async Task<IActionResult> ShowDocument(string id)
        {
            string decodedId = WebUtility.UrlDecode(id);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var doc = await _context.Doc_Assignments.FirstOrDefaultAsync(d => d.DocumentId == decodedId);

            if (doc == null)
            {
                return NotFound();
            }
            Doc_Titles title = (Doc_Titles)Enum.Parse(typeof(Doc_Titles), doc.Title);
            ViewData["DocumentTitle"] = title.GetType().GetMember(title.ToString()).First().GetCustomAttribute<DisplayAttribute>().Name;
            return View(doc);
        }

        public async Task<IActionResult> DeleteDocument(string id)
        {
            string decodedId = WebUtility.UrlDecode(id);

            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var doc = await _context.Doc_Assignments.FirstOrDefaultAsync(d => d.DocumentId == decodedId);

            if (doc == null)
            {
                return NotFound();
            }

            return View(doc);

        }
        [HttpPost, ActionName("DeleteDocument")]
        public async Task<IActionResult> DeleteDocumentConfirm(string id)
        {
            string decodedId = WebUtility.UrlDecode(id);

            var doc = await _context.Doc_Assignments.FindAsync(decodedId);

            _context.Doc_Assignments.Remove(doc);

            await _context.SaveChangesAsync();
            GlobalAlert.SendGlobalAlert($"Dokument {doc.DocumentId} został usunięty!", "danger");
            return RedirectToAction(nameof(Index));
        }
        private string DocumentIdGenerator(DateTime time, Doc_Titles title)
        {
            string docCount = (_context.Doc_Assignments.Count() + 1).ToString();
            StringBuilder sb = new StringBuilder();
            string divider = "/";

            sb.Append(title);
            sb.Append(divider);
            sb.Append(time.Year);
            sb.Append(divider);
            sb.Append(time.Month);
            sb.Append(divider);
            sb.Append(time.Day);
            sb.Append(divider);

            switch (docCount.Length)
            {
                case 1:
                    sb.Append("0000");
                    sb.Append(docCount);
                    break;
                case 2:
                    sb.Append("000");
                    sb.Append(docCount);
                    break;
                case 3:
                    sb.Append("00");
                    sb.Append(docCount);
                    break;
                case 4:
                    sb.Append("0");
                    sb.Append(docCount);
                    break;
                default:
                    sb.Append(docCount);
                    break;
            }

            sb.Append(divider);
            sb.Append(MillisOfDay(time));

            return sb.ToString();
        }

        private Doc_Titles DocumentTitleGenerator(int? from, int? to)
        {

            Doc_Titles[,] docTitlesArray = new Doc_Titles[,]
            {
                { Doc_Titles.P, Doc_Titles.PW, Doc_Titles.PW, Doc_Titles.WZ },
                { Doc_Titles.ZW, Doc_Titles.PW, Doc_Titles.ZW, Doc_Titles.WZ },
                { Doc_Titles.ZW, Doc_Titles.RW, Doc_Titles.MM, Doc_Titles.WZ },
                { Doc_Titles.PZ, Doc_Titles.PZ, Doc_Titles.PZ, Doc_Titles.WZ },
            };

            return docTitlesArray[(int)from+1,(int)to+1];
        }

        private long MillisOfDay(DateTime date)
        {
            DateTime today = new DateTime(date.Year, date.Month, date.Day);
            return (long)(DateTime.Now - today).TotalMilliseconds;
        }

        private void AddToInfobox(Doc_Assignment doc)
        {
            Infobox info = new Infobox();
            string UserIdentityName = !string.IsNullOrEmpty(User?.Identity?.Name) ? User.Identity.Name : "";
            string loggedUserId = _context.Users.FirstOrDefault(u => u.NormalizedUserName == UserIdentityName)?.Id;

            if (doc.UserTo != null)
            {
                var user = _context.Users.FirstOrDefault(u => u.Id == doc.UserTo);

                if (user != null)
                {
                    info.Title = "Nowy dokument";
                    info.Message = $"Wygenerowano dokument nr {doc.DocumentId}, który wymaga potwierdzenia. Aby to zrobić, kliknij \"Potwierdź\"";
                    info.ReceivedDate = doc.CreationDate;
                    info.UserId = string.IsNullOrEmpty(user.Id) ? loggedUserId : user.Id;
                    info.DocumentId = doc.DocumentId;

                    _context.Infoboxes.Add(info);
                }
            }
            else
            {
                info.Title = "Nowy dokument";
                info.Message = $"Wygenerowano dokument nr {doc.DocumentId}, który wymaga potwierdzenia. Aby to zrobić, kliknij \"Potwierdź\"";
                info.ReceivedDate = doc.CreationDate;
                info.UserId = loggedUserId;
                info.DocumentId = doc.DocumentId;
            }
        }

    }
}
