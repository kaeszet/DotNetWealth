﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Authorization;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Http;
using DotNetWMS.Resources;
using Microsoft.Extensions.Logging;

namespace DotNetWMS.Controllers
{
    /// <summary>
    /// Controller class to support the CRUD process for the Item model
    /// </summary>
    [Authorize]
    public class ItemsController : Controller
    {
        /// <summary>
        /// A field for handling the delivery of information to the DB associated with the Entity Core framework
        /// </summary>
        private readonly DotNetWMSContext _context;
        /// <summary>
        /// Implementation of the WMSIdentityUser class in the UserManager class to maintain the user account
        /// </summary>
        private readonly UserManager<WMSIdentityUser> _userManager;
        /// <summary>
        /// Field of object which implements interface <c>IHttpContextAccessor</c> to get displayed url whoch is necessary to generate QR Code
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;
        /// <summary>
        /// Log4net library field
        /// </summary>
        private readonly ILogger<ItemsController> _logger;

        public ItemsController(DotNetWMSContext context, UserManager<WMSIdentityUser> userManager, IHttpContextAccessor httpContextAccessor, ILogger<ItemsController> logger)
        {
            _context = context;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_employee view for the selected id
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Assign_to_employee view</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        [HttpGet]
        public async Task<IActionResult> Assign_to_employee(int? id)
        {
            var item = await CreateAssignItemConfirmationView(id, "Assign_to_employee");
            if (item == null) return NotFound();

            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_warehouse view for the selected id
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Assign_to_warehouse view</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        [HttpGet]
        public async Task<IActionResult> Assign_to_warehouse(int? id)
        {
            var item = await CreateAssignItemConfirmationView(id, "Assign_to_warehouse");
            if (item == null) return NotFound();

            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_external view for the selected id
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Assign_to_external view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpGet]
        public async Task<IActionResult> Assign_to_external(int? id)
        {
            var item = await CreateAssignItemConfirmationView(id, "Assign_to_external");
            if (item == null) return NotFound();
            return View(item);
        }
        /// <summary>
        /// POST method responsible for processing the completed form on Assign_to_employee view
        /// </summary>
        /// <param name="id">Item ID which assignment was edited</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, used is returned to the previous view where entered changes are visible. Also the database is updated. Otherwise - an error message is generated</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_employee(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {

            if (id != item.Id)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {

                return await CreateAssignItemConfirmationView(item, "Assign_to_employee");

            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            return View(item);
        }

        /// <summary>
        /// POST method responsible for processing the completed form on Assign_to_warehouse view
        /// </summary>
        /// <param name="id">Item ID which assignment was edited</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, used is returned to the previous view where entered changes are visible. Also the database is updated. Otherwise - an error message is generated</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_warehouse(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            if (id != item.Id)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await CreateAssignItemConfirmationView(item, "Assign_to_warehouse");
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// POST method responsible for processing the completed form on Assign_to_external view
        /// </summary>
        /// <param name="id">Item ID which assignment was edited</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, used is returned to the previous view where entered changes are visible. Also the database is updated. Otherwise - an error message is generated</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_external(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            if (id != item.Id)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await CreateAssignItemConfirmationView(item, "Assign_to_external");
            }
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Index view and supports a search engine
        /// </summary>
        /// <param name="order">Sort names or warranty date in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Item's Index view with list of items in the order set by the user</returns>
        public async Task<IActionResult> Index(string order, string search)
        {
            
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["SortByWarrantyDate"] = order == "WarrantyDate" ? "date_desc" : "WarrantyDate";
            ViewData["Search"] = search;

            var items = _context.Items.Select(e => e);
            

            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.Name.Contains(search) || i.ItemCode.Contains(search));
            }

            items = order switch
            {
                "name_desc" => items.OrderByDescending(i => i.Name).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse),
                "date_desc" => items.OrderByDescending(i => i.WarrantyDate).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse),
                "WarrantyDate" => items.OrderBy(i => i.WarrantyDate).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse),
                _ => items.OrderBy(i => i.Name).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse),
            };

            if (User != null)
            {
                int? newMessages = _context.Infoboxes.Where(i => i.User.NormalizedUserName == User.Identity.Name && i.IsChecked == false)?.Count();
                GlobalAlert.SendQuantity(newMessages);
            }

            return View(await items.AsNoTracking().ToListAsync());
        }
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        [HttpGet]
        public IActionResult ItemAssignment(string order, string search, string userId, int? warehouseId, int? externalId)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["SortByType"] = order == "type" ? "type_desc" : "type";
            ViewData["Search"] = search;

            var items = _context.Items.ToList();

            List<ItemsAssignmentViewModel> viewModelList = new List<ItemsAssignmentViewModel>();

            foreach (var item in items)
            {
                List<string> listOfOccurences = UsingEntitiesList(item);

                ItemsAssignmentViewModel viewModel = new ItemsAssignmentViewModel()
                {
                    Id = item.Id,
                    IsChecked = false,
                    ItemCode = item.ItemCode,
                    Model = item.Model,
                    Name = item.Name,
                    Producer = item.Producer,
                    Quantity = item.Quantity,
                    Type = item.Type,
                    Units = item.Units,
                    Records = listOfOccurences

                };

                viewModelList.Add(viewModel);
            }

            if (!string.IsNullOrEmpty(search))
            {
                //
                viewModelList = viewModelList.Where(i => i.Name.Contains(search) || i.ItemCode.Contains(search)).ToList();
            }

            viewModelList = order switch
            {
                //
                "name_desc" => viewModelList.OrderByDescending(i => i.Name).ThenByDescending(j => j.Producer).ThenByDescending(j => j.Model).ToList(),
                "type_desc" => viewModelList.OrderByDescending(i => i.Type).ToList(),
                "type" => viewModelList.OrderBy(i => i.Type).ToList(),
                _ => viewModelList.OrderBy(i => i.Name).ThenBy(j => j.Producer).ThenBy(j => j.Model).ToList(),
            };

            return View(viewModelList);
        }
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        public IActionResult ItemAssignmentConfirmation(string option, List<ItemsAssignmentViewModel> model)
        {

            var itemsChecked = model.Where(m => m.IsChecked == true).ToList();

            for (int i = 0; i < itemsChecked.Count; i++)
            {
                var itemToValidate = _context.Items.FirstOrDefault(v => v.Id == itemsChecked[i].Id);

                if (itemsChecked[i].Quantity > itemToValidate.Quantity)
                {
                    ModelState.AddModelError(string.Empty, $"Nie można przekazać więcej sztuk niż jest na stanie. Wprowadzono: {itemsChecked[i].Quantity}, stan: {itemToValidate.Quantity}{itemToValidate.Units}, {itemToValidate.Assign}");
                    return View("ItemAssignment", model);
                }

                if (itemsChecked[i].Quantity <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Nie można przekazać 0 sztuk. Wprowadzono: {itemsChecked[i].Quantity}, {itemToValidate.Assign}");
                    return View("ItemAssignment", model);
                }

                if (option == "ToUser" && itemToValidate.ExternalId != null)
                {
                    ModelState.AddModelError(string.Empty, $"Przedmiot \"{itemToValidate.FullName}\" jest w posiadaniu zewnętrznej firmy. Przedmiot można przypisać do pracownika, gdy zostanie dostarczony/zwrócony");
                    return View("ItemAssignment", model);
                }
            }
            
            if (ModelState.IsValid)
            {
                if (!itemsChecked.Any())
                {
                    ModelState.AddModelError(string.Empty, "Nie wybrano żadnego przedmiotu do przekazania");
                    return View("ItemAssignment", model);
                }

                var users = option == "ToUser" ? _context.Users.ToList() : null;
                var warehouses = option == "ToWarehouse" ? _context.Warehouses.ToList() : null;
                var externals = option == "ToExternal" ? _context.Externals.ToList() : null;

                ViewData["Option"] = option;

                ItemAssignmentConfirmationViewModel viewModel = new ItemAssignmentConfirmationViewModel()
                {
                    Items = itemsChecked
                };

                if (users != null)
                {
                    ViewData["Users"] = new SelectList(_context.Users, "Id", "FullName");
                    return View("ItemAssignmentConfirmation", viewModel);
                }

                if (warehouses != null)
                {
                    ViewData["Warehouses"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
                    return View("ItemAssignmentConfirmation", viewModel);
                }

                if (externals != null)
                {
                    if (User.IsInAnyRoles("StandardPlus,Moderator,Admin"))
                    {
                        ViewData["Externals"] = new SelectList(_context.Externals, "Id", "FullName");
                        return View("ItemAssignmentConfirmation", viewModel);
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Administration");
                    }
                    
                }
            }

            return View("ItemAssignment", model);
        }
        [Authorize(Roles = "Standard,StandardPlus,Moderator,Admin")]
        [HttpPost]
        public async Task<IActionResult> ItemAssignmentSaveInDb(string option, ItemAssignmentConfirmationViewModel model)
        {
            for (int i = 0; i < model.Items.Count; i++)
            {
                string userName = "";

                var item = _context.Items.Find(model.Items[i].Id);

                //string userId = item.UserId;
                //int? warehouseId = item.WarehouseId;
                int? externalId = item.ExternalId;
                

                if (item != null)
                {
                    if (item.Quantity == model.Items[i].Quantity)
                    {
                        switch (option)
                        {
                            case "ToUser":
                                if (item.UserId == model.UserId)
                                {
                                    ModelState.AddModelError(string.Empty, $"Przedmiot {item.FullName} jest już na stanie o wybranego pracownika");
                                    ViewData["Users"] = new SelectList(_context.Users, "Id", "FullName");
                                    ViewData["Option"] = option;
                                    return View("ItemAssignmentConfirmation", model);
                                }
                                item.UserId = model.UserId;
                                userName = _context.Users.Find(model.UserId)?.NormalizedUserName;
                                MergeSameItems(item, typeof(WMSIdentityUser));
                                break;
                            case "ToWarehouse":
                                if (item.WarehouseId == model.WarehouseId)
                                {
                                    ModelState.AddModelError(string.Empty, $"Przedmiot {item.FullName} jest już w wybranym magazynie");
                                    ViewData["Warehouses"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
                                    ViewData["Option"] = option;
                                    return View("ItemAssignmentConfirmation", model);
                                }
                                item.WarehouseId = model.WarehouseId;
                                MergeSameItems(item, typeof(Warehouse));
                                break;
                            case "ToExternal":
                                if (item.ExternalId == model.ExternalId)
                                {
                                    ModelState.AddModelError(string.Empty, $"Przedmiot {item.FullName} jest już u wybranego kontahenta");
                                    ViewData["Externals"] = new SelectList(_context.Externals, "Id", "FullName");
                                    ViewData["Option"] = option;
                                    return View("ItemAssignmentConfirmation", model);
                                }
                                item.ExternalId = model.ExternalId;
                                MergeSameItems(item, typeof(External));
                                break;
                            default:
                                break;
                        }

                        ItemStatusCheck(item, externalId, notifyUser: false);
                        item.ItemCode = ItemCodeGenerator.Generate(item, userName);
                        await _context.SaveChangesAsync();
                        
                    }
                    
                    if (item.Quantity > model.Items[i].Quantity)
                    {
                        item.Quantity -= model.Items[i].Quantity;

                        string guid = Guid.NewGuid().ToString();

                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = guid,
                            Quantity = model.Items[i].Quantity,
                            Units = model.Items[i].Units,
                            WarrantyDate = item.WarrantyDate,
                            State = ItemState.Unknown,
                            UserId = !string.IsNullOrEmpty(model.UserId) ? model.UserId : item.UserId,
                            WarehouseId = model.WarehouseId != null ? model.WarehouseId : item.WarehouseId,
                            ExternalId = model.ExternalId != null ? model.ExternalId : item.ExternalId

                        };

                        _context.Add(newItem);
                        var result = await _context.SaveChangesAsync();

                        if (result > 0)
                        {
                            var newItemWithId = _context.Items.FirstOrDefault(i => i.ItemCode == guid);
                            model.Items[i].Id = newItemWithId.Id;

                            switch (option)
                            {
                                case "ToUser":
                                    userName = _context.Users.FirstOrDefault(u => u.Id == newItemWithId.UserId)?.NormalizedUserName;
                                    MergeSameItems(newItemWithId, typeof(WMSIdentityUser));
                                    break;
                                case "ToWarehouse":
                                    MergeSameItems(newItemWithId, typeof(Warehouse));
                                    break;
                                case "ToExternal":
                                    MergeSameItems(newItemWithId, typeof(External));
                                    break;
                                default:
                                    break;
                            }

                            ItemStatusCheck(newItemWithId, externalId, notifyUser: false);
                            newItemWithId.ItemCode = ItemCodeGenerator.Generate(newItemWithId, userName);
                            await _context.SaveChangesAsync();
                        }
                        

                    }

                   
                }
            }

            await SendInfo(model);

            if (model.IsDocumentNeeded)
            {

                Doc_Titles docTitle = option switch
                {
                    "ToUser" => !string.IsNullOrEmpty(model.UserId) ? Doc_Titles.PW : Doc_Titles.ZW,
                    "ToWarehouse" => model.WarehouseId != null ? Doc_Titles.MM : Doc_Titles.RW,
                    "ToExternal" => model.ExternalId != null ? Doc_Titles.WZ : Doc_Titles.PZ,
                    _ => Doc_Titles.P,
                };

                Doc_Creator creator = new Doc_Creator();
                string username = User?.Identity?.Name;
                await creator.GenerateAndSaveDocument(model, _context, username, docTitle);

            }

            GlobalAlert.SendGlobalAlert($"Przedmioty ({model.Items.Count}) zostały przekazane!", "info");
            return RedirectToAction("Index");
        }
        private List<string> UsingEntitiesList(Item item)
        {
            List<string> listOfOccurrences = new List<string>();

            var findInWarehouses = _context.Warehouses.AsNoTracking().Where(x => x.Items.Any(i => i.Id == item.Id)).Select(x => x.AssignFullName).ToList();

            if (findInWarehouses.Any())
            {
                //
                listOfOccurrences.AddRange(findInWarehouses);
            }

            var findInExternals = _context.Externals.AsNoTracking().Where(x => x.Items.Any(i => i.Id == item.Id)).Select(x => x.FullName).ToList();

            if (findInExternals.Any())
            {
                listOfOccurrences.AddRange(findInExternals);
            }

            var findInUsers = _context.Users.AsNoTracking().Where(x => x.Items.Any(i => i.Id == item.Id)).Select(x => x.FullName).ToList();

            if (findInUsers.Any())
            {
                listOfOccurrences.AddRange(findInUsers);
            }

            return listOfOccurrences;
        }

        /// <summary>
        /// GET method responsible for returning an Item's Details view
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Item's Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            
            var url = _httpContextAccessor.HttpContext?.Request?.GetDisplayUrl();

            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.User)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }

            ViewBag.QrCode = QRCodeCreator.ShowQRCode(url);
            ViewData["url"] = url;

            return View(item);
        }
        public IActionResult ShowQrCode(string url)
        {
            TempData["QRCode"] = QRCodeCreator.ShowQRCode(url);
            return PartialView("_QrCodePartial");
        }
        /// <summary>
        /// GET method responsible for returning an Item's Create view
        /// </summary>
        /// <returns>Returns Item's Create view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName");
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, returns Item's Index view. Otherwise - show error message</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            //bool isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode);
            bool isItemExists = ItemExists(item);

            if (ModelState.IsValid)
            {
                if (!isItemExists)
                {
                    string user = "";
                    if (!string.IsNullOrEmpty(item.UserId))
                    {
                        user = _context.Users.FirstOrDefault(u => u.Id == item.UserId).NormalizedUserName;
                    }
                    var itemOld = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == item.Id);
                    item.ItemCode = ItemCodeGenerator.Generate(item, user);
                    _context.Add(item);
                    ItemStatusCheck(item, itemOld?.ExternalId);
                    await _context.SaveChangesAsync();
                    await UpdateItemCode(item);
                    GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został dodany do bazy!", "success");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    _logger.LogDebug($"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                    ModelState.AddModelError(string.Empty, $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                }
                
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// GET/POST method to responsible for checking whether the item has already been entered into the database 
        /// </summary>
        /// <param name="itemCode">Item code to be checked</param>
        /// <returns>If the item does not exist, it returns true (in JSON format). Otherwise - returns an error message</returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsItemExists(string itemCode)
        {
            bool isItemExists = _context.Items.Any(i => i.ItemCode == itemCode);

            if (!isItemExists)
            {
                return Json(true);
            }
            else
            {
                _logger.LogDebug($"Przedmiot o kodzie ({itemCode}) został już wprowadzony!");
                return Json($"Przedmiot o kodzie ({itemCode}) został już wprowadzony!");
            }
        }
        /// <summary>
        /// GET/POST method to responsible for checking whether the item has already been entered into the database 
        /// </summary>
        /// <param name="item">Item to be checked</param>
        /// <returns>If the item does not exist, it returns true (in JSON format). Otherwise - returns an error message</returns>
        [AcceptVerbs("Get", "Post")]
        public IActionResult IsItemExists(Item item)
        {
            bool isItemExists = _context.Items.Any(i => i.Producer == item.Producer && i.Model == item.Model && i.Name == item.Name && i.Type == item.Type && i.WarrantyDate == item.WarrantyDate);

            if (!isItemExists)
            {
                return Json(true);
            }
            else
            {
                _logger.LogDebug($"Przedmiot został już wprowadzony do systemu!");
                return Json($"Przedmiot został już wprowadzony do systemu!");
            }
        }
        /// <summary>
        /// GET method responsible for returning an Item's Edit view
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Edit view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);

            if (item == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="id">Item ID to edit</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, returns Item's Index view, data validation on the model side</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            if (id != item.Id)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }

            var itemOld = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == item.Id);

            //static
            //bool isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode && i.ItemCode != ItemCode);
            bool isItemExists = false;

            if (item.ItemCode != itemOld.ItemCode)
            {
                isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode && i.ItemCode != itemOld.ItemCode);
            }
            

            if (ModelState.IsValid)
            {
                if (!isItemExists)
                {
                    try
                    {
                        string user = "";
                        if (item.UserId != null)
                        {
                            user = _context.Users.FirstOrDefault(u => u.Id == item.UserId).NormalizedUserName;
                        }
                        item.ItemCode = ItemCodeGenerator.Generate(item, user);
                        _context.Update(item);
                        ItemStatusCheck(item, itemOld.ExternalId);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ItemExists(item.Id))
                        {
                            _logger.LogError($"Przedmiot {item.Assign} został zmieniony przez innego użytkownika");
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został zmieniony!", "success");
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                    _logger.LogDebug($"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                }
                
                
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Delete view
        /// </summary>
        /// <param name="id">Item ID to delete</param>
        /// <returns>Returns Item's Delete view if exists</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.User)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                return NotFound();
            }

            return View(item);
        }
        /// <summary>
        /// POST method responsible for removing employee from DB if the user confirms this action
        /// </summary>
        /// <param name="id">Item ID to delete</param>
        /// <returns>Returns Item's Index view</returns>
        [Authorize(Roles = "StandardPlus,Moderator,Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został usunięty!", "danger");
            return RedirectToAction(nameof(Index));
        }
        /// <summary>
        /// A private method responsible for checking if there is a item with the given id
        /// </summary>
        /// <param name="id">Item ID to check</param>
        /// <returns>Returns true if the item exists. Otherwise - false.</returns>
        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
        private bool ItemExists(Item item)
        {
            return _context.Items.Any(i => i.Producer == item.Producer && i.Model == item.Model && i.Name == item.Name && i.Type == item.Type && i.WarrantyDate == item.WarrantyDate);
        }
        /// <summary>
        /// Private method responsible for correctly combining the quantity of two identical items entered by the user
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="obj">Argument to check the class name of the object being passed</param>
        private void MergeSameItems(Item item, Type obj)
        {
            //Item sameItem = null;


            //switch (obj.Name)
            //{
            //    case "WMSIdentityUser":
            //        sameItem = _context.Items.FirstOrDefault(i => i.Producer == item.Producer && i.Model == item.Model && i.Name == item.Name && i.Type == item.Type && i.WarrantyDate == item.WarrantyDate && i.UserId == item.UserId);
            //        break;
            //    case "Warehouse":
            //        sameItem = _context.Items.FirstOrDefault(i => i.Producer == item.Producer && i.Model == item.Model && i.Name == item.Name && i.Type == item.Type && i.WarrantyDate == item.WarrantyDate && i.WarehouseId == item.WarehouseId);
            //        break;
            //    case "External":
            //        sameItem = _context.Items.FirstOrDefault(i => i.Producer == item.Producer && i.Model == item.Model && i.Name == item.Name && i.Type == item.Type && i.WarrantyDate == item.WarrantyDate && i.ExternalId == item.ExternalId);
            //        break;

            //}

            Item sameItem = _context.Items.FirstOrDefault(i => i.Producer == item.Producer
            && i.Model == item.Model
            && i.Name == item.Name
            && i.Type == item.Type
            && i.WarrantyDate == item.WarrantyDate
            && i.UserId == item.UserId
            & i.WarehouseId == item.WarehouseId
            & i.ExternalId == item.ExternalId);


            //if (sameItem != null && sameItem.Id != item.Id && sameItem.ExternalId == null && sameItem.WarehouseId == item.WarehouseId)
            //{
            //    _context.Remove(sameItem);
            //    item.Quantity += sameItem.Quantity;
            //    _context.Update(item);
            //}

            if (sameItem != null && sameItem.Id != item.Id)
            {
                _context.Remove(sameItem);
                item.Quantity += sameItem.Quantity;
                _context.Update(item);
            }


        }
        /// <summary>
        /// A private method to check who has been assigned the item specified in the argument of the function. Depending on who it currently belongs to, the condition of the item will be different.
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="extId">External ID to check</param>
        /// <param name="notifyUser">True if the user should be notified, otherwise - false. Default true</param>
        private void ItemStatusCheck(Item item, int? extId, bool notifyUser = true)
        {

            if (item.ExternalId != null)
            {
                var ext = _context.Externals.Find(item.ExternalId);

                if (item.State != ItemState.Damaged)
                {
                    switch (ext.Type)
                    {
                        case ContractorType.Sklep:
                            item.State = ItemState.Ordered;
                            break;
                        case ContractorType.Serwis:
                            item.State = ItemState.InRepair;
                            break;
                        case ContractorType.Wypożyczający:
                            item.State = ItemState.InLoan;
                            break;
                        case ContractorType.Podwykonawca:
                            item.State = ItemState.InLoan;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    if (ext.Type == ContractorType.Serwis)
                    {
                        item.State = ItemState.InRepair;
                    }
                }

                if (notifyUser) SendInfo(item);
     
            }
            else if (item.ExternalId == null && extId != null && (item.State == ItemState.Ordered || item.State == ItemState.InRepair || item.State == ItemState.InLoan) )
            {
                var ext = _context.Externals.Find(extId);

                switch (ext.Type)
                {
                    case ContractorType.Sklep:
                        item.State = ItemState.New;
                        break;
                    case ContractorType.Serwis:
                        item.State = ItemState.Repaired;
                        break;
                    case ContractorType.Wypożyczający:
                        item.State = ItemState.Returned;
                        break;
                    case ContractorType.Podwykonawca:
                        item.State = ItemState.Returned;
                        break;
                    default:
                        break;
                }

                if (notifyUser) SendInfo(item, ext.Name);

                if (item.WarehouseId != null)
                {
                    item.State = ItemState.InWarehouse;
                }
                if (item.WarehouseId == null && item.UserId != null)
                {
                    item.State = ItemState.InEmployee;
                }
            }
            else if (item.ExternalId == null && !(item.State == ItemState.Ordered || item.State == ItemState.InRepair || item.State == ItemState.InLoan))
            {
                if (item.State != ItemState.Damaged)
                {
                    if (item.WarehouseId != null)
                    {
                        item.State = ItemState.InWarehouse;
                    }
                    else if (item.WarehouseId == null && item.UserId != null)
                    {
                        item.State = ItemState.InEmployee;
                    }
                    else
                    {
                        item.State = ItemState.Other;
                    }
                }

                if (notifyUser) SendInfo(item);
            }
            else
            {
                item.State = ItemState.Unknown;
            }
            
        }
        /// <summary>
        /// A private method responsible for creating confirmation view for AssignTo type procedures
        /// </summary>
        /// <param name="id">Item ID to check</param>
        /// <param name="method">Name of the method which determines the behavior of the task</param>
        /// <returns>Item object to further processing</returns>
        private async Task<Item> CreateAssignItemConfirmationView(int? id, string method)
        {

            if (id != null)
            {
                var item = await _context.Items.FindAsync(id);
                
                if (item == null)
                {
                    _logger.LogDebug($"DEBUG: Nie znaleziono w bazie przedmiotu o podanym id = {id}");
                    return null;
                }

                if (method == "Assign_to_employee")
                {
                    if (item.ExternalId != null)
                    {
                        var ext = await _context.Externals.FindAsync(item.ExternalId);
                        ModelState.AddModelError(string.Empty, $"Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
                    }
                }

                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
                ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);

                return item;
            }
            _logger.LogDebug($"DEBUG: Wprowadzony idetyfikator ma wartość null lub jest pustym stringiem");
            return null;

        }
        /// <summary>
        /// A private method responsible for processing task after user's confirmation
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="method">Name of the method which determines the behavior of the task</param>
        /// <returns>If succeed, redirects to AssignTo type view. Otherwise - show 404 error view</returns>
        private async Task<IActionResult> CreateAssignItemConfirmationView(Item item, string method)
        {
            var itemOld = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == item.Id);

            if (item.Quantity <= 0)
            {
                _logger.LogDebug($"DEBUG: Nie można przekazać {item.Quantity} sztuk");
                ModelState.AddModelError(string.Empty, $"Nie można przekazać {item.Quantity} sztuk");
            }

            else if (method == "Assign_to_employee" && item.ExternalId != null)
            {
                var ext = await _context.Externals.FindAsync(item.ExternalId);
                _logger.LogDebug($"DEBUG: Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
                ModelState.AddModelError(string.Empty, $"Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
            }

            else if (item.Quantity < itemOld.Quantity && item.Quantity > 0)
            {
                return await ItemPresentationInAssignViewWhenNotEqual(item, method);
            }

            else if (item.Quantity > itemOld.Quantity)
            {
                _logger.LogDebug($"DEBUG: Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
                ModelState.AddModelError(string.Empty, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
            }

            else
            {
                return await ItemPresentationInAssignViewWhenEqual(item, method);
            }

            switch (method)
            {
                case "Assign_to_employee":
                    ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
                    break;
                case "Assign_to_warehouse":
                    ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
                    break;
                case "Assign_to_external":
                    ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
                    break;
                default:
                    return NotFound();
            }

            return View(item);

        }
        /// <summary>
        /// A private method responsible for items presentation in database when user assign part of the whole amount
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="method">Name of the method which determines the behavior of the task</param>
        /// <returns>If succeed, redirects to AssignTo type view. Otherwise - show 404 error view</returns>
        private async Task<IActionResult> ItemPresentationInAssignViewWhenNotEqual(Item item, string method)
        {
            var itemOld = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == item.Id);

            switch (method)
            {
                case "Assign_to_employee":
                    if (itemOld.UserId != item.UserId)
                    {
                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = item.ItemCode,
                            Quantity = itemOld.Quantity - item.Quantity,
                            Units = item.Units,
                            WarrantyDate = item.WarrantyDate,
                            State = item.State,
                            UserId = itemOld.UserId,
                            WarehouseId = item.WarehouseId,
                            ExternalId = item.ExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(WMSIdentityUser));
                        ItemStatusCheck(item, itemOld.ExternalId);
                        await _context.SaveChangesAsync();
                        await UpdateItemCode(item);
                        GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został przekazany!", "info");
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        _logger.LogDebug($"DEBUG: Nie można przekazać przedmiotu temu samemu pracownikowi!");
                        ModelState.AddModelError(string.Empty, $"Nie można przekazać przedmiotu temu samemu pracownikowi!");
                        ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
                        return View(item);
                    }
                case "Assign_to_warehouse":
                    if (itemOld.WarehouseId != item.WarehouseId)
                    {
                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = item.ItemCode,
                            Quantity = itemOld.Quantity - item.Quantity,
                            Units = item.Units,
                            WarrantyDate = item.WarrantyDate,
                            State = item.State,
                            UserId = item.UserId,
                            WarehouseId = itemOld.WarehouseId,
                            ExternalId = item.ExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(Warehouse));
                        ItemStatusCheck(item, itemOld.ExternalId);
                        await _context.SaveChangesAsync();
                        await UpdateItemCode(item);
                        GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został przekazany!", "info");
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        _logger.LogDebug($"DEBUG: Nie można przekazać przedmiotu do tego samego magazynu!");
                        ModelState.AddModelError(string.Empty, $"Nie można przekazać przedmiotu do tego samego magazynu!");
                        ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
                        return View(item);
                    }
                case "Assign_to_external":
                    if (itemOld.ExternalId != item.ExternalId)
                    {
                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = item.ItemCode,
                            Quantity = itemOld.Quantity - item.Quantity,
                            Units = item.Units,
                            WarrantyDate = item.WarrantyDate,
                            State = item.State,
                            UserId = item.UserId,
                            WarehouseId = item.WarehouseId,
                            ExternalId = itemOld.ExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(External));
                        ItemStatusCheck(item, itemOld.ExternalId);
                        await _context.SaveChangesAsync();
                        await UpdateItemCode(item);
                        GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został przekazany!", "info");
                        return RedirectToAction("Index");

                    }
                    else
                    {
                        _logger.LogDebug($"DEBUG: Nie można ponownie przekazać przedmiotu temu samemu podmiotowi!");
                        ModelState.AddModelError(string.Empty, $"Nie można ponownie przekazać przedmiotu temu samemu podmiotowi!");
                        ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
                        return View(item);
                    }
                default:
                    return NotFound();
                    
            }
            
        }
        /// <summary>
        /// A private method responsible for items presentation in database when user assign all of chosen items
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="method">Name of the method which determines the behavior of the task</param>
        /// <returns>If succeed, redirects to AssignTo type view. Otherwise - show 404 error view</returns>
        private async Task<IActionResult> ItemPresentationInAssignViewWhenEqual(Item item, string method)
        {
            string user = "";
            if (item.UserId != null)
            {
                user = _context.Users.FirstOrDefault(u => u.Id == item.UserId).NormalizedUserName;
            }
            var itemOld = await _context.Items.AsNoTracking().FirstOrDefaultAsync(i => i.Id == item.Id);

            switch (method)
            {
                case "Assign_to_employee":

                    try
                    {
                        if (itemOld.UserId != item.UserId)
                        {
                            Doc_Creator creator = new Doc_Creator();
                            item.ItemCode = ItemCodeGenerator.Generate(item, user);
                            _context.Update(item);
                            MergeSameItems(item, typeof(WMSIdentityUser));
                            ItemStatusCheck(item, itemOld.ExternalId);
                            await _context.SaveChangesAsync();
                            await UpdateItemCode(item);
                            await creator.GenerateAndSaveDocument(item, _context, User?.Identity?.Name, method, itemOld.UserId, item.UserId);
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ItemExists(item.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został przekazany!", "info");
                    return RedirectToAction("Index");

                case "Assign_to_warehouse":

                    try
                    {
                        if (itemOld.WarehouseId != item.WarehouseId)
                        {
                            Doc_Creator creator = new Doc_Creator();
                            item.ItemCode = ItemCodeGenerator.Generate(item, user);
                            _context.Update(item);
                            MergeSameItems(item, typeof(Warehouse));
                            ItemStatusCheck(item, itemOld.ExternalId);
                            await _context.SaveChangesAsync();
                            await UpdateItemCode(item);
                            await creator.GenerateAndSaveDocument(item, _context, User?.Identity?.Name, method, itemOld.WarehouseId.ToString(), item.WarehouseId.ToString());
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ItemExists(item.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został przekazany!", "info");
                    return RedirectToAction("Index");

                case "Assign_to_external":

                    try
                    {
                        if (itemOld.ExternalId != item.ExternalId)
                        {
                            Doc_Creator creator = new Doc_Creator();
                            item.ItemCode = ItemCodeGenerator.Generate(item, user);
                            _context.Update(item);
                            MergeSameItems(item, typeof(External));
                            ItemStatusCheck(item, itemOld.ExternalId);
                            await _context.SaveChangesAsync();
                            await UpdateItemCode(item);
                            await creator.GenerateAndSaveDocument(item, _context, User?.Identity?.Name, method, itemOld.ExternalId.ToString(), item.ExternalId.ToString());
                        }

                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ItemExists(item.Id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    GlobalAlert.SendGlobalAlert($"Przedmiot {item.Assign} został przekazany!", "info");
                    return RedirectToAction("Index");

                default:
                    break;
            }
            return NotFound();
        }
        /// <summary>
        /// Method for adding new message to infobox about status changes
        /// </summary>
        /// <param name="item"><c>Item</c> object which properties are necessary to create infos</param>
        /// <param name="externalOldName">Name of <c>External</c> object before change</param>
        private void SendInfo(Item item, string externalOldName = "")
        {
            string UserIdentityName = !string.IsNullOrEmpty(User?.Identity?.Name) ? User.Identity.Name : "";
            DateTime receivedDate = DateTime.Now;
            Infobox info = new Infobox();
            string loggedUserId = _context.Users.FirstOrDefault(u => u.NormalizedUserName == UserIdentityName)?.Id;

            switch (item.State)
            {
                case ItemState.Ordered:

                    var itemExt = item.External;
                    string itemExtName = itemExt != null ? itemExt.Name : "której nazwy nie podano";

                    info.Title = "Zamówiłeś przedmiot";
                    info.Message = $"Zamówiłeś \"{item.Name}\" w ilości {item.Quantity} {item.Units} od firmy {itemExtName}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.New:

                    info.Title = "Otrzymałeś zamówiony przedmiot";
                    info.Message = $"Otrzymałeś zamówienie \"{item.Name}\" w ilości {item.Quantity} {item.Units} od firmy {externalOldName}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.Damaged:

                    info.Title = "Przedmiot uszkodzony";
                    info.Message = $"Przedmiot \"{item.Name}\" w ilości {item.Quantity} {item.Units} został oznaczony jako uszkodzony";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.Repaired:

                    info.Title = "Przedmiot został naprawiony";
                    info.Message = $"Przedmiot \"{item.Name}\" w ilości {item.Quantity} {item.Units} został naprawiony przez {externalOldName}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.Returned:

                    info.Title = "Przedmiot został zwrócony";
                    info.Message = $"Przedmiot \"{item.Name}\" w ilości {item.Quantity} {item.Units} został zwrócony przez {externalOldName}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.InWarehouse:

                    var warehouseFullName = _context.Warehouses.Find(item.WarehouseId)?.Name;

                    info.Title = "Przedmiot w magazynie";
                    info.Message = $"Przedmiot \"{item.Name}\" w ilości {item.Quantity} {item.Units} został wysłany do magazynu {warehouseFullName}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.InEmployee:

                    info.Title = "Otrzymałeś przedmiot";
                    info.Message = $"Otrzymałeś \"{item.Name}\" w ilości {item.Quantity} {item.Units} od użytkownika {UserIdentityName}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = item.UserId;
                    break;

                case ItemState.InRepair:

                    info.Title = "Przedmiot w naprawie";
                    info.Message = $"Przekazałeś \"{item.Name}\" w ilości {item.Quantity} {item.Units} do firmy {item.External.Name}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.InLoan:

                    info.Title = "Wypożyczyłeś przedmiot";
                    info.Message = $"Przedmiot \"{item.Name}\" w ilości {item.Quantity} {item.Units} został wypożyczony podmiotowi {item.External.Name}";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.OutOfWarranty:

                    break;

                case ItemState.Other:
                    info.Title = "Przedmiot nieprzypisany";
                    info.Message = $"Przedmiot \"{item.Name}\" w ilości {item.Quantity} {item.Units} jest na stanie, ale aktualnie nie jest przypisany";
                    info.ReceivedDate = receivedDate;
                    info.UserId = string.IsNullOrEmpty(item.UserId) ? loggedUserId : item.UserId;
                    break;

                case ItemState.Unknown:
                    break;

                default:
                    break;
            }

            if (!string.IsNullOrEmpty(info.Title))
            {
                _context.Infoboxes.Add(info);
            }

        }

        private async Task SendInfo(ItemAssignmentConfirmationViewModel model)
        {
            string UserIdentityName = !string.IsNullOrEmpty(User?.Identity?.Name) ? User.Identity.Name : "";
            Infobox info = new Infobox();
            DateTime receivedDate = DateTime.Now;
            string loggedUserId = _context.Users.FirstOrDefault(u => u.NormalizedUserName == UserIdentityName)?.Id;

            if (!string.IsNullOrEmpty(model.UserId))
            {
                info.Title = "Otrzymałeś przedmioty";
                info.Message = $"Otrzymałeś na stan nowe przedmioty w liczbie: {model.Items.Count()}. Sprawdź szczegóły w zakładce \"Majątek\".";
                info.ReceivedDate = receivedDate;
                info.UserId = model.UserId;
            }

            if (model.WarehouseId != null)
            {
                var warehouseName = _context.Warehouses.FirstOrDefault(w => w.Id == model.WarehouseId)?.AssignFullName;
                warehouseName ??= "o nieznanej nazwie";

                info.Title = "Przekazałeś przedmioty do magazynu";
                info.Message = $"Przekazałeś do magazynu {warehouseName} przedmioty w liczbie: {model.Items.Count()}. Sprawdź szczegóły w zakładce \"Majątek\".";
                info.ReceivedDate = receivedDate;
                info.UserId = loggedUserId;
            }

            if (model.ExternalId != null)
            {
                var externalName = _context.Externals.FirstOrDefault(e => e.Id == model.ExternalId)?.FullName;
                externalName ??= "o nieznanej nazwie";

                info.Title = "Przekazałeś przedmioty kontrahentowi";
                info.Message = $"Przekazałeś kontrahentowi {externalName} przedmioty w liczbie: {model.Items.Count()}. Sprawdź szczegóły w zakładce \"Majątek\".";
                info.ReceivedDate = receivedDate;
                info.UserId = loggedUserId;
            }

            if (!string.IsNullOrEmpty(info.Title))
            {
                _context.Infoboxes.Add(info);
                await _context.SaveChangesAsync();
            }

        }
        /// <summary>
        /// Method responsible for updating ItemCode in case of changes in DB
        /// </summary>
        /// <param name="item"><c>Item</c> object which ItemCode parametrer will be changed</param>
        /// <returns></returns>
        private async Task UpdateItemCode(Item item)
        {
            string userName = _context.Users.FirstOrDefault(u => u.Id == item.UserId)?.NormalizedUserName;
            item.ItemCode = !string.IsNullOrEmpty(userName) ? ItemCodeGenerator.Generate(item, userName) : ItemCodeGenerator.Generate(item, User?.Identity?.Name);
            _context.Update(item);
            await _context.SaveChangesAsync();
        }
    }
}
