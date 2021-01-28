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
using System.Runtime.InteropServices.WindowsRuntime;

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
        /// A static field for handling Item's ItemCode for properly creation of Assign-type views
        /// </summary>
        private static string ItemCode;
        /// <summary>
        /// A static field for handling Item's ItemQuantity for properly creation of Assign-type views
        /// </summary>
        private static decimal ItemQuantity;
        /// <summary>
        /// A static field for handling UserId for properly creation of Assign_to_employee view
        /// </summary>
        private static string ItemEmployeeId;
        /// <summary>
        /// A static field for handling WarehouseId for properly creation of Assign_to_warehouse view
        /// </summary>
        private static int? ItemWarehouseId;
        /// <summary>
        /// A static field for handling ExternalId for properly creation of Assign_to_external view
        /// </summary>
        private static int? ItemExternalId;


        public ItemsController(DotNetWMSContext context)
        {
            _context = context;
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_employee view and supports a search engine
        /// </summary>
        /// <param name="order">Sort by name or warranty date in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Item's Assign_to_employee view with list of items in the order set by the user</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator")]
        public async Task<IActionResult> Assign_to_employee(string search, string order)
        {
            var items = CreateAssignItemView(search, order);
            return View(await items.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_warehouse view and supports a search engine
        /// </summary>
        /// <param name="order">Sort by name or warranty date in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Item's Assign_to_warehouse view with list of items in the order set by the user</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator")]
        public async Task<IActionResult> Assign_to_warehouse(string search, string order)
        {
            var items = CreateAssignItemView(search, order);
            return View(await items.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_external view and supports a search engine
        /// </summary>
        /// <param name="order">Sort by name or warranty date in ascending or descending order</param>
        /// <param name="search">Search phrase in the search field</param>
        /// <returns>Returns Item's Assign_to_external view with list of items in the order set by the user</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Assign_to_external(string search, string order)
        {
            var items = CreateAssignItemView(search, order);
            return View(await items.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_employee_confirm view for the selected id
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Assign_to_employee_confirm view</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator")]
        [HttpGet]
        public async Task<IActionResult> Assign_to_employee_confirm(int? id)
        {
            var item = await CreateAssignItemConfirmationView(id, "Assign_to_employee_confirm");
            if (item == null) return NotFound();
            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_warehouse_confirm view for the selected id
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Assign_to_warehouse_confirm view</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator")]
        [HttpGet]
        public async Task<IActionResult> Assign_to_warehouse_confirm(int? id)
        {
            var item = await CreateAssignItemConfirmationView(id, "Assign_to_warehouse_confirm");
            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Assign_to_external_confirm view for the selected id
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Assign_to_external_confirm view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpGet]
        public async Task<IActionResult> Assign_to_external_confirm(int? id)
        {
            var item = await CreateAssignItemConfirmationView(id, "Assign_to_external_confirm");
            return View(item);
        }
        /// <summary>
        /// POST method responsible for processing the completed form on Assign_to_employee_confirm view
        /// </summary>
        /// <param name="id">Item ID which assignment was edited</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, used is returned to the previous view where entered changes are visible. Also the database is updated. Otherwise - an error message is generated</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_employee_confirm(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            
            if (id != item.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
                return await CreateAssignItemConfirmationView(item, "Assign_to_employee_confirm"); 

            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            return View(item);
        }
        /// <summary>
        /// POST method responsible for processing the completed form on Assign_to_warehouse_confirm view
        /// </summary>
        /// <param name="id">Item ID which assignment was edited</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, used is returned to the previous view where entered changes are visible. Also the database is updated. Otherwise - an error message is generated</returns>
        [Authorize(Roles = "Standard,StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_warehouse_confirm(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {

            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await CreateAssignItemConfirmationView(item, "Assign_to_warehouse_confirm");
            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// POST method responsible for processing the completed form on Assign_to_external_confirm view
        /// </summary>
        /// <param name="id">Item ID which assignment was edited</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, used is returned to the previous view where entered changes are visible. Also the database is updated. Otherwise - an error message is generated</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_external_confirm(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {

            if (id != item.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                return await CreateAssignItemConfirmationView(item, "Assign_to_external_confirm");
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

            switch (order)
            {
                case "name_desc": items = items.OrderByDescending(i => i.Name).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "date_desc": items = items.OrderByDescending(i => i.WarrantyDate).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "WarrantyDate": items = items.OrderBy(i => i.WarrantyDate).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                default: items = items.OrderBy(i => i.Name).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
            }
            return View(await items.AsNoTracking().ToListAsync());
        }
        /// <summary>
        /// GET method responsible for returning an Item's Details view
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Item's Details view</returns>
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.User)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Create view
        /// </summary>
        /// <returns>Returns Item's Create view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public IActionResult Create()
        {
            
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName");
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street");
            return View();
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, returns Item's Index view. Otherwise - show error message</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            bool isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode);
            if (ModelState.IsValid)
            {
                if (!isItemExists)
                {
                    _context.Add(item);
                    ItemStatusCheck(item, ItemExternalId);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                }
                
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
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
                return Json($"Przedmiot o kodzie ({itemCode}) został już wprowadzony!");
            }
        }
        /// <summary>
        /// GET method responsible for returning an Item's Edit view
        /// </summary>
        /// <param name="id">Item ID which should be returned</param>
        /// <returns>Returns Item's Edit view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            ItemCode = item.ItemCode;
            if (item == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// POST method responsible for checking and transferring information from the form to DB
        /// </summary>
        /// <param name="id">Item ID to edit</param>
        /// <param name="item">Item model class with binding DB attributes</param>
        /// <returns>If succeed, returns Item's Index view, data validation on the model side</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,UserId,WarehouseId,ExternalId")] Item item)
        {
            if (id != item.Id)
            {
                return NotFound();
            }

            bool isItemExists = _context.Items.Any(i => i.ItemCode == item.ItemCode && i.ItemCode != ItemCode);

            if (ModelState.IsValid)
            {
                if (!isItemExists)
                {
                    try
                    {
                        _context.Update(item);
                        ItemStatusCheck(item, ItemExternalId);
                        await _context.SaveChangesAsync();
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
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");
                }
                
                
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }
        /// <summary>
        /// GET method responsible for returning an Item's Delete view
        /// </summary>
        /// <param name="id">Item ID to delete</param>
        /// <returns>Returns Item's Delete view if exists</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.User)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }
        /// <summary>
        /// POST method responsible for removing employee from DB if the user confirms this action
        /// </summary>
        /// <param name="id">Item ID to delete</param>
        /// <returns>Returns Item's Index view</returns>
        [Authorize(Roles = "StandardPlus,Moderator")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
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
        /// <summary>
        /// Private method responsible for correctly combining the quantity of two identical items entered by the user
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="obj">Argument to check the class name of the object being passed</param>
        private void MergeSameItems(Item item, Type obj)
        {
            Item sameItem = null;
            switch (obj.Name)
            {
                case "User":
                    sameItem = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode && i.UserId == item.UserId);
                    break;
                case "Warehouse":
                    sameItem = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode && i.WarehouseId == item.WarehouseId);
                    break;
                case "External":
                    sameItem = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode && i.ExternalId == item.ExternalId);
                    break;

            }
            
            
            if (sameItem != null && sameItem.Id != item.Id && sameItem.ExternalId == null && sameItem.WarehouseId == item.WarehouseId)
            {
                ItemQuantity = sameItem.Quantity;
                _context.Remove(sameItem);
                item.Quantity += ItemQuantity;
                _context.Update(item);
            }
        }
        /// <summary>
        /// A private method to check who has been assigned the item specified in the argument of the function. Depending on who it currently belongs to, the condition of the item will be different.
        /// </summary>
        /// <param name="item">The Item class object to be checked by the method</param>
        /// <param name="extId">External ID to check</param>
        private void ItemStatusCheck(Item item, int? extId)
        {

            if (item.ExternalId != null)
            {
                var ext = _context.Externals.Find(item.ExternalId);

                switch (ext.Type)
                {
                    case ContractorType.Sklep: item.State = ItemState.Ordered;
                        break;
                    case ContractorType.Serwis: item.State = ItemState.InRepair;
                        break;
                    case ContractorType.Wypożyczający: item.State = ItemState.InLoan;
                        break;
                    case ContractorType.Podwykonawca: item.State = ItemState.InLoan;
                        break;
                    default:
                        break;
                }
            }
            else if (item.ExternalId == null && extId != null)
            {
                var ext = _context.Externals.Find(extId);

                switch (ext.Type)
                {
                    case ContractorType.Sklep: item.State = ItemState.New;
                        break;
                    case ContractorType.Serwis: item.State = ItemState.Repaired;
                        break;
                    case ContractorType.Wypożyczający: item.State = ItemState.Returned;
                        break;
                    case ContractorType.Podwykonawca: item.State = ItemState.Returned;
                        break;
                    default:
                        break;
                }
                if (item.WarehouseId != null)
                {
                    item.State = ItemState.InWarehouse;
                }
                if (item.WarehouseId == null && item.UserId != null)
                {
                    item.State = ItemState.InEmployee;
                }
            }
            else if (item.ExternalId == null && extId == null)
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
                    item.State = ItemState.Unknown;
                }
            }
            
        }
        /// <summary>
        /// A private method responsible for creating different AssignTo type views
        /// </summary>
        /// <param name="search">Search phrase in the search field</param>
        /// <param name="order">Sort names or warranty date in ascending or descending order</param>
        /// <returns>List of items as IQueryable of item type</returns>
        private IQueryable<Item> CreateAssignItemView(string search, string order)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["SortByWarrantyDate"] = order == "WarrantyDate" ? "date_desc" : "WarrantyDate";
            ViewData["Search"] = search;

            var items = _context.Items.Select(e => e);


            if (!string.IsNullOrEmpty(search))
            {
                items = items.Where(i => i.Name.Contains(search) || i.ItemCode.Contains(search));
            }

            switch (order)
            {
                case "name_desc":
                    items = items.OrderByDescending(i => i.Name).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(i => i.WarrantyDate).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "WarrantyDate":
                    items = items.OrderBy(i => i.WarrantyDate).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                default:
                    items = items.OrderBy(i => i.Name).Include(i => i.User).Include(i => i.External).Include(i => i.Warehouse);
                    break;
            }
            return items;
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
                    return null;
                }

                ItemQuantity = item.Quantity;

                switch (method)
                {
                    case "Assign_to_employee_confirm":
                        ItemEmployeeId = item.UserId;
                        if (item.ExternalId != null)
                        {
                            var ext = await _context.Externals.FindAsync(item.ExternalId);
                            ModelState.AddModelError(string.Empty, $"Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
                        }
                        break;
                    case "Assign_to_warehouse_confirm":
                        ItemWarehouseId = item.WarehouseId;
                        break;
                    case "Assign_to_external_confirm":
                        ItemExternalId = item.ExternalId;
                        break;

                    default:
                        break;
                }

                
                ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
                ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
                ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);

                return item;
            }

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
            if (item.Quantity <= 0)
            {
                ModelState.AddModelError(string.Empty, $"Nie można przekazać {item.Quantity} sztuk");
            }

            else if (method == "Assign_to_employee_confirm" && item.ExternalId != null)
            {
                var ext = await _context.Externals.FindAsync(item.ExternalId);
                ModelState.AddModelError(string.Empty, $"Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
            }

            else if (item.Quantity < ItemQuantity && item.Quantity > 0)
            {
                return await ItemPresentationInAssignViewWhenNotEqual(item, method);
            }

            else if (item.Quantity > ItemQuantity)
            {
                ModelState.AddModelError(string.Empty, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
            }

            else
            {
                return await ItemPresentationInAssignViewWhenEqual(item, method);
            }

            switch (method)
            {
                case "Assign_to_employee_confirm":
                    ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
                    break;
                case "Assign_to_warehouse_confirm":
                    ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
                    break;
                case "Assign_to_external_confirm":
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
            switch (method)
            {
                case "Assign_to_employee_confirm":
                    if (ItemEmployeeId != item.UserId)
                    {
                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = item.ItemCode,
                            Quantity = ItemQuantity - item.Quantity,
                            Units = item.Units,
                            WarrantyDate = item.WarrantyDate,
                            State = item.State,
                            UserId = ItemEmployeeId,
                            WarehouseId = item.WarehouseId,
                            ExternalId = item.ExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(WMSIdentityUser));
                        ItemStatusCheck(item, ItemExternalId);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Assign_to_employee");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Nie można przekazać przedmiotu temu samemu pracownikowi!");
                        ViewData["UserId"] = new SelectList(_context.Users, "Id", "FullName", item.UserId);
                        return View(item);
                    }
                case "Assign_to_warehouse_confirm":
                    if (ItemWarehouseId != item.WarehouseId)
                    {
                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = item.ItemCode,
                            Quantity = ItemQuantity - item.Quantity,
                            Units = item.Units,
                            WarrantyDate = item.WarrantyDate,
                            State = item.State,
                            UserId = item.UserId,
                            WarehouseId = ItemWarehouseId,
                            ExternalId = item.ExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(Warehouse));
                        ItemStatusCheck(item, ItemExternalId);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Assign_to_warehouse");

                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Nie można przekazać przedmiotu do tego samego magazynu!");
                        ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
                        return View(item);
                    }
                case "Assign_to_external_confirm":
                    if (ItemExternalId != item.ExternalId)
                    {
                        Item newItem = new Item()
                        {
                            Type = item.Type,
                            Name = item.Name,
                            Producer = item.Producer,
                            Model = item.Model,
                            ItemCode = item.ItemCode,
                            Quantity = ItemQuantity - item.Quantity,
                            Units = item.Units,
                            WarrantyDate = item.WarrantyDate,
                            State = item.State,
                            UserId = item.UserId,
                            WarehouseId = item.WarehouseId,
                            ExternalId = ItemExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(External));
                        ItemStatusCheck(item, ItemExternalId);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Assign_to_external");

                    }
                    else
                    {
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
            switch (method)
            {
                case "Assign_to_employee_confirm":

                    try
                    {
                        if (ItemEmployeeId != item.UserId)
                        {
                            _context.Update(item);
                            MergeSameItems(item, typeof(WMSIdentityUser));
                            ItemStatusCheck(item, ItemExternalId);
                            await _context.SaveChangesAsync();
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
                    return RedirectToAction("Assign_to_employee");

                case "Assign_to_warehouse_confirm":

                    try
                    {
                        if (ItemWarehouseId != item.WarehouseId)
                        {
                            _context.Update(item);
                            MergeSameItems(item, typeof(Warehouse));
                            ItemStatusCheck(item, ItemExternalId);
                            await _context.SaveChangesAsync();
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
                    return RedirectToAction("Assign_to_warehouse");

                case "Assign_to_external_confirm":

                    try
                    {
                        if (ItemExternalId != item.ExternalId)
                        {
                            _context.Update(item);
                            MergeSameItems(item, typeof(External));
                            ItemStatusCheck(item, ItemExternalId);
                            await _context.SaveChangesAsync();
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
                    return RedirectToAction("Assign_to_external");

                default:
                    break;
            }
            return NotFound();
        }
    }
}
