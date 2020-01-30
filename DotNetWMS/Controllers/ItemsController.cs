using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetWMS.Data;
using DotNetWMS.Models;

namespace DotNetWMS.Controllers
{
    public class ItemsController : Controller
    {
        private readonly DotNetWMSContext _context;
        private static string ItemCode;
        private static decimal ItemQuantity;
        private static int? ItemEmployeeId;
        private static int? ItemWarehouseId;
        private static int? ItemExternalId;


        public ItemsController(DotNetWMSContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Assign_to_employee(string search, string order)
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
                    items = items.OrderByDescending(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "WarrantyDate":
                    items = items.OrderBy(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                default:
                    items = items.OrderBy(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
            }
            return View(await items.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Assign_to_warehouse(string search, string order)
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
                    items = items.OrderByDescending(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "WarrantyDate":
                    items = items.OrderBy(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                default:
                    items = items.OrderBy(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
            }
            return View(await items.AsNoTracking().ToListAsync());
        }
        public async Task<IActionResult> Assign_to_external(string search, string order)
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
                    items = items.OrderByDescending(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "date_desc":
                    items = items.OrderByDescending(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "WarrantyDate":
                    items = items.OrderBy(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                default:
                    items = items.OrderBy(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
            }
            return View(await items.AsNoTracking().ToListAsync());
        }
        [HttpGet]
        public async Task<IActionResult> Assign_to_employee_confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            var item = await _context.Items.FindAsync(id);
            //
            if (item == null)
            {
                return NotFound();
            }
            //
            ItemQuantity = item.Quantity;
            ItemEmployeeId = item.EmployeeId;

            if (item.ExternalId != null)
            {
                var ext = await _context.Externals.FindAsync(item.ExternalId);
                ModelState.AddModelError(string.Empty, $"Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
            }

            if (item == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> Assign_to_warehouse_confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            ItemQuantity = item.Quantity;
            ItemWarehouseId = item.WarehouseId;

            if (item == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        [HttpGet]
        public async Task<IActionResult> Assign_to_external_confirm(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items.FindAsync(id);
            ItemQuantity = item.Quantity;
            ItemExternalId = item.ExternalId;

            if (item == null)
            {
                return NotFound();
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_employee_confirm(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {
            
            if (id != item.Id)
            {
                return NotFound();
            }
           
            if (ModelState.IsValid)
            {
               
                if (item.Quantity <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Nie można przekazać {item.Quantity} sztuk");
                }
                else if (item.ExternalId != null)
                {
                    var ext = await _context.Externals.FindAsync(item.ExternalId);
                    ModelState.AddModelError(string.Empty, $"Przedmiot w posiadaniu zewnętrznej firmy: \"{ext.Name}\". Przedmiot można przypisać do pracownika, gdy zostanie zwrócony");
                }
                else if (item.Quantity < ItemQuantity && item.Quantity > 0)
                {
                    if (ItemEmployeeId != item.EmployeeId)
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
                            EmployeeId = ItemEmployeeId,
                            WarehouseId = item.WarehouseId,
                            ExternalId = item.ExternalId
                        };

                        _context.Add(newItem);
                        _context.Update(item);
                        MergeSameItems(item, typeof(Employee));
                        ItemStatusCheck(item, ItemExternalId);
                        await _context.SaveChangesAsync();
                        return RedirectToAction("Assign_to_employee");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, $"Nie można przekazać przedmiotu temu samemu pracownikowi!");
                    }
                    
                }
                else if (item.Quantity > ItemQuantity)
                {
                    ModelState.AddModelError(string.Empty, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
                }
                else
                {
                    try
                    {
                        if (ItemEmployeeId != item.EmployeeId)
                        {
                            _context.Update(item);
                            MergeSameItems(item, typeof(Employee));
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
                }

               
            }
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            //ViewData["ItemAssign"] = new SelectList(_context.Items, "Id", "Assign");
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_warehouse_confirm(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {

            if (id != item.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                if (item.Quantity <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Nie można przekazać {item.Quantity} sztuk");
                }
                else if (item.Quantity < ItemQuantity && item.Quantity > 0)
                {
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
                            EmployeeId = item.EmployeeId,
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
                    }

                }
                else if (item.Quantity > ItemQuantity)
                {
                    ModelState.AddModelError(string.Empty, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
                }
                else
                {
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
                }


            }
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "AssignFullName", item.WarehouseId);
            //ViewData["ItemAssign"] = new SelectList(_context.Items, "Id", "Assign");
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View(item);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Assign_to_external_confirm(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
        {

            if (id != item.Id)
            {
                return NotFound();
            }


            if (ModelState.IsValid)
            {
                if (item.Quantity <= 0)
                {
                    ModelState.AddModelError(string.Empty, $"Nie można przekazać {item.Quantity} sztuk");
                }
                else if (item.Quantity < ItemQuantity && item.Quantity > 0)
                {
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
                            EmployeeId = item.EmployeeId,
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
                    }

                }
                else if (item.Quantity > ItemQuantity)
                {
                    ModelState.AddModelError(string.Empty, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
                }
                else
                {
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
                }


            }
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            //ViewData["ItemAssign"] = new SelectList(_context.Items, "Id", "Assign");
            //ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            return View(item);
        }
        public async Task<IActionResult> Index(string order, string search)
        {
            ViewData["SortByName"] = string.IsNullOrEmpty(order) ? "name_desc" : "";
            ViewData["SortByWarrantyDate"] = order == "WarrantyDate" ? "date_desc" : "WarrantyDate";
            ViewData["Search"] = search;

            var items = _context.Items.Select(e => e);
            

            if (!string.IsNullOrEmpty(search))
            {
                //CheckEnum(search);
                items = items.Where(i => i.Name.Contains(search) || i.ItemCode.Contains(search));
            }

            switch (order)
            {
                case "name_desc": items = items.OrderByDescending(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "date_desc": items = items.OrderByDescending(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                case "WarrantyDate": items = items.OrderBy(i => i.WarrantyDate).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
                default: items = items.OrderBy(i => i.Name).Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
                    break;
            }
            return View(await items.AsNoTracking().ToListAsync());
        }
        //private bool CheckEnum(string s)
        //{
        //    foreach (string name in Enum.GetNames(typeof(ItemState)))
        //    {
        //        if (name.Contains(s))
        //        {
        //            return true;
        //        }
        //    }
        //    return false;
        //}
        // GET: Items
        //public async Task<IActionResult> Index()
        //{
        //    var dotNetWMSContext = _context.Items.Include(i => i.Employee).Include(i => i.External).Include(i => i.Warehouse);
        //    return View(await dotNetWMSContext.ToListAsync());
        //}

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Employee)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName");
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name");
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street");
            return View();
        }

        // POST: Items/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }
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

        // GET: Items/Edit/5
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }

        // POST: Items/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Producer,Model,ItemCode,Quantity,Units,WarrantyDate,State,EmployeeId,WarehouseId,ExternalId")] Item item)
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
            ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "FullName", item.EmployeeId);
            ViewData["ExternalId"] = new SelectList(_context.Externals, "Id", "Name", item.ExternalId);
            ViewData["WarehouseId"] = new SelectList(_context.Warehouses, "Id", "Street", item.WarehouseId);
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var item = await _context.Items
                .Include(i => i.Employee)
                .Include(i => i.External)
                .Include(i => i.Warehouse)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var item = await _context.Items.FindAsync(id);
            _context.Items.Remove(item);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ItemExists(int id)
        {
            return _context.Items.Any(e => e.Id == id);
        }
        private void MergeSameItems(Item item, Type obj)
        {
            Item sameItem = null;
            switch (obj.Name)
            {
                case "Employee":
                    sameItem = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode && i.EmployeeId == item.EmployeeId);
                    break;
                case "Warehouse":
                    sameItem = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode && i.WarehouseId == item.WarehouseId);
                    break;
                case "External":
                    sameItem = _context.Items.FirstOrDefault(i => i.ItemCode == item.ItemCode && i.ExternalId == item.ExternalId);
                    break;

            }
            
            
            if (sameItem != null && sameItem.Id != item.Id)
            {
                ItemQuantity = sameItem.Quantity;
                _context.Remove(sameItem);
                item.Quantity += ItemQuantity;
                _context.Update(item);
            }
        }
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
                if (item.WarehouseId == null && item.EmployeeId != null)
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
                else if (item.WarehouseId == null && item.EmployeeId != null)
                {
                    item.State = ItemState.InEmployee;
                }
                else
                {
                    item.State = ItemState.Unknown;
                }
            }
            
        }
    }
}
