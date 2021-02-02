using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    public class DotNetWMSTests_Adam_Items : DotNetWMSTests_Base
    {
        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public void Model_CheckIsModelValidIfFilledWithLenghtException_ReturnFalse()
        {

            var emp = new Item() { Id = 4, Type = "Komputer osobisty_Komputer osobisty", Name = "Laptop", Producer = "Lenovo", Model = "X1 Carbon", ItemCode = "2", Quantity = 1, Units = 0, WarrantyDate = DateTime.Now, State = 0, UserId = "", WarehouseId = 2, ExternalId = 1 };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);


        }

        [Test]
        public void Model_CheckIsModelValidIfRequiredFieldAreNotFilled_ReturnFalse()
        {
            var emp = new Item();
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void Model_CheckIsModelValidIfRequiredFieldAreFilled_ReturnTrue()
        {
            var emp = new Item() { Id = 4, Type = "Komputer osobisty", Name = "Laptop", Producer = "Lenovo", Model = "X1 Carbon", ItemCode = "2", Quantity = 1, Units = 0, WarrantyDate = DateTime.Now, State = 0, UserId = "", WarehouseId = 2, ExternalId = 1 };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsTrue(isModelValid);



        
        }
        [Test]
        public void CreateGet_GetCreateView_ReturnViewObjectWithoutInjectedData()
        {

            var controller = new ItemsController(_context);
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);


        }
        [Test]
        public async Task CreatePost_AddNewRecordToDb_RecordAdded()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var emp = new Item() { Id = 4, Type = "Komputer osobisty", Name = "Laptop", Producer = "Lenovo", Model = "X1 Carbon", ItemCode = "2", Quantity = 1, Units = 0, WarrantyDate = DateTime.Now, State = 0, UserId = "", WarehouseId = 2, ExternalId = 1 };
            var controller = new ItemsController(_context);
            var result = await controller.Create(emp) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public async Task Details_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Details(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Details(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Details(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Details(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IsReturnedModelHasSameValues_ReturnTrue()
        {
            var emp = _context.Items.Find(1);
            var controller = new ItemsController(_context);
            var vr = await controller.Details(1) as ViewResult;
            var result = vr.ViewData.Model as Item;
            Assert.IsTrue(result.Id == emp.Id && result.Name == emp.Name);



        }
        [Test]
        public async Task EditGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task EditGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Edit(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_EditRecordWithNoExistingId_ReturnNotFoundResult()
        {
            var emp = _context.Items.Find(1);
            var controller = new ItemsController(_context);
            var result = await controller.Edit(99, emp);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_EditRecordWithDifferentId_ReturnNotFoundResult()
        {
            var emp = _context.Items.Find(2);
            var controller = new ItemsController(_context);
            var result = await controller.Edit(1, emp);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Delete(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Delete(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Delete(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeleteGet_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new ItemsController(_context);
            var result = await controller.Delete(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeletePost_RedirectToIndexIfCorrectData_ReturnTrue()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var emp = new Item() { Id = 4, Type = "Komputer osobisty", Name = "Laptop", Producer = "Lenovo", Model = "X1 Carbon", ItemCode = "2", Quantity = 1, Units = 0, WarrantyDate = DateTime.Now, State = 0, UserId = "", WarehouseId = 2, ExternalId = 1 };
            var controller = new ItemsController(_context);
            await controller.Create(emp);
            var result = await controller.DeleteConfirmed(4) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
    }
}
