using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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
    /// <summary>
    /// Controller class responsible for <c>Doc_Assignments</c> functionality
    /// </summary>
    /// 
    [Authorize(Roles = "Standard,StandardPlus,Moderator")]
    public class Doc_AssignmentsController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<Doc_AssignmentsController> _logger;

        public Doc_AssignmentsController(DotNetWMSContext context, ILogger<Doc_AssignmentsController> logger)
        {
            _context = context;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning Doc_Assignment's Index view and supports a search engine
        /// </summary>
        /// <param name="order">Sort names in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Doc_Assignment's Index view with list of documents in the order set by the user</returns>
        public async Task<IActionResult> Index(string search, string order)
        {
            ViewData["SortById"] = string.IsNullOrEmpty(order) ? "id_desc" : "";
            ViewData["Search"] = search;

            //var docs = _context.Doc_Assignments.Select(d => d);
            IQueryable<Doc_Assignment> docs;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.NormalizedUserName == User.Identity.Name);

            if (User.IsInRole("Moderator"))
            {
                docs = _context.Doc_Assignments.Select(d => d);
            }
            else
            {
                docs = _context.Doc_Assignments.Where(d => d.UserTo == user.Id);
            }

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
        /// <summary>
        /// GET method responsible for returning Doc_Assignment's ConfigureDocument view, contains selectlists of users, externals and warehouses
        /// </summary>
        /// <returns>Returns Doc_Assignment's ConfigureDocument view</returns>
        [HttpGet]
        public IActionResult ConfigureDocument()
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["Externals"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["Warehouses"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
            return View();
        }
        /// <summary>
        /// POST method connected with jquery method. It uses chosen data from selectlists to configure _Doc_AssignmentConfDocPartial view
        /// </summary>
        /// <param name="from">User, Warehouse or External FROM whom the item is transferred</param>
        /// <param name="to">User, Warehouse or External TO whom the item is transferred</param>
        /// <param name="fromIndex">Index connected with FROM side of contracting party</param>
        /// <list type="bullet">
        /// <item>
        /// <term>0</term>
        /// <description>User</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>Warehouse</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>External (contractor)</description>
        /// </item>
        /// </list>
        /// <param name="toIndex">Index connected with TO side of contracting party</param>
        /// <list type="bullet">
        /// <item>
        /// <term>0</term>
        /// <description>User</description>
        /// </item>
        /// <item>
        /// <term>1</term>
        /// <description>Warehouse</description>
        /// </item>
        /// <item>
        /// <term>2</term>
        /// <description>External (contractor)</description>
        /// </item>
        /// </list>
        /// <returns>Returns _Doc_AssignmentConfDocPartial view contains list of <c>Doc_ConfigureDocumentViewModel</c> objects</returns>
        [HttpPost]
        public IActionResult ConfigureDocument(string from, string to, int fromIndex, int toIndex)
        {
            IQueryable<Item> items;
            string infoMessage = "";

            switch (toIndex)
            {
                case 0:
                    items = _context.Items.Where(i => i.UserId == to);

                    if (!items.Any())
                    {
                        infoMessage = "Użytkownik nie ma przypisanych przedmiotów";
                    }

                    break;
                case 1:
                    items = _context.Items.Where(i => i.WarehouseId == Convert.ToInt32(to));

                    if (!items.Any())
                    {
                        infoMessage = "W wybranym magazynie nie ma przedmiotów!";
                    }
                    break;
                case 2:
                    items = _context.Items.Where(i => i.ExternalId == Convert.ToInt32(to));

                    if (!items.Any())
                    {
                        infoMessage = "Kontrahent nie ma przypisanych przedmiotów";
                    }
                    break;
                default:
                    items = _context.Items.Where(i => i.User.NormalizedUserName == User.Identity.Name);

                    if (!items.Any())
                    {
                        infoMessage = "Użytkownik nie ma przypisanych przedmiotów";
                    }
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

            ViewData["InfoMessage"] = infoMessage;
            return PartialView("_Doc_AssignmentConfDocPartial", viewModel);
        }
        /// <summary>
        /// POST method for generating document summary with print and save document in DB options
        /// </summary>
        /// <param name="viewModel">List of <c>Doc_ConfigureDocumentViewModel</c> objects</param>
        /// <returns>Returns GenerateDocument view contains <c>Doc_Assignment</c> object</returns>
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
        /// <summary>
        /// Save document in dbo.Doc_Assignments
        /// </summary>
        /// <param name="doc"><c>Doc_Assignment</c> object</param>
        /// <returns>Returns confirmation view</returns>
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
            _logger.LogDebug($"Nie odnaleziono dokumentu {doc.DocumentId}");
            return NotFound();
        }
        /// <summary>
        /// Get and show document from dbo.Doc_Assignments
        /// </summary>
        /// <param name="id">ID of searched document</param>
        /// <returns>ShowDocument view with document data</returns>
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
        /// <summary>
        /// Get and delete document from dbo.Doc_Assignments
        /// </summary>
        /// <param name="id">Document ID to delete</param>
        /// <returns>DeleteDocument view with document data</returns>
        [Authorize(Roles = "Moderator")]
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
        /// <summary>
        /// POST method to delete document from DB after user confirmation
        /// </summary>
        /// <param name="id">Document ID to delete</param>
        /// <returns>If succeeded returns Doc_Assignment's Index, otherwise - NotFound view</returns>
        [Authorize(Roles = "Moderator")]
        [HttpPost, ActionName("DeleteDocument")]
        public async Task<IActionResult> DeleteDocumentConfirm(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string decodedId = WebUtility.UrlDecode(id);
                var doc = await _context.Doc_Assignments.FindAsync(decodedId);

                if (doc != null)
                {
                    _context.Doc_Assignments.Remove(doc);
                    await _context.SaveChangesAsync();
                }

                var info = await _context.Infoboxes.FirstOrDefaultAsync(i => i.DocumentId == decodedId);

                if (info != null)
                {
                    _context.Infoboxes.Remove(info);
                    await _context.SaveChangesAsync();
                }

                GlobalAlert.SendGlobalAlert($"Dokument {doc.DocumentId} został usunięty!", "danger");
            }
            else
            {
                return View("NotFound");
            }
            
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// Method with logic for generating DocumentId which is necessary to save new record in DB
        /// </summary>
        /// <param name="time">Time of generating the document</param>
        /// <param name="title">Document title received from enum list</param>
        /// <returns>Returns documentID as a string</returns>
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
        /// <summary>
        /// Method with logic for checking correct document title which is necessary to generate documentId
        /// </summary>
        /// <param name="from">ID of User, Warehouse or External FROM whom the item is transferred</param>
        /// <param name="to">ID of User, Warehouse or External TO whom the item is transferred</param>
        /// <returns>Returns correct title from enum two dimensional array</returns>
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
        /// <summary>
        /// Method for calculating millisecond after today 0:00:00
        /// </summary>
        /// <param name="date">Time of generating the document</param>
        /// <returns>Amount of milliseconds</returns>
        private long MillisOfDay(DateTime date)
        {
            DateTime today = new DateTime(date.Year, date.Month, date.Day);
            return (long)(DateTime.Now - today).TotalMilliseconds;
        }
        /// <summary>
        /// Method for adding info about new document to infobox
        /// </summary>
        /// <param name="doc"><c>Doc_Assignment</c> object</param>
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

                _context.Infoboxes.Add(info);
            }
        }

    }
}
