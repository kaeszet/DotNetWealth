using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    public class DotNetWMSTests_Adam_Warehouses : DotNetWMSTests_Base
    {
        private Mock<ILogger<WarehousesController>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<WarehousesController>>();
        }

        [Test]
        public void WarehouseModel_CheckIsModelValidIfFilledWithLenghtException_ReturnFalse()
        {

            var emp = new Warehouse() { Id = 01, Name = "Magazyn główny_Magazyn główny_Magazyn główny", Street = "ul. Św Filipa 1x", ZipCode = "31-001", City = "Kraków" };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);


        }
        [Test]
        public void WarehouseModel_CheckIsModelValidIfFilledWithRegexException_ReturnFalse()
        {

            var emp = new Warehouse() { Id = 1, Name = "Magazyn główny", Street = "ul. Św Filipa 1x", ZipCode = "31001", City = "Kraków" };
            ICollection<ValidationResult> results;
            var isModelValid = TryValidate(emp, out results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.AreEqual(arr[0].ErrorMessage, "Nieprawidłowy format kodu pocztowego xx-xxx");
            Assert.IsFalse(isModelValid, arr[0].ErrorMessage);


        }
        [Test]
        public void WarehouseModel_CheckIsModelValidIfRequiredFieldAreNotFilled_ReturnFalse()
        {
            var emp = new Warehouse();
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void WarehouseModel_CheckIsModelValidIfRequiredFieldAreFilled_ReturnTrue()
        {
            var emp = new Warehouse() { Id = 01, Name = "Magazyn główny", Street = "ul. Św Filipa 1x", ZipCode = "31-001", City = "Kraków" };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsTrue(isModelValid);



        }
       
        [Test]
        public async Task Index_GetListOfWarehouses_ReturnList()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var items = await _context.Warehouses.ToListAsync();

            Assert.AreEqual(0, items.Count);

        }
        
        [Test]
        public void WarehouseCreateGet_GetCreateView_ReturnViewObjectWithoutInjectedData()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);


        }
        [Test]
        public async Task WarehouseCreatePost_AddNewRecordToDb_RecordAdded()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var emp = new Warehouse() { Id = 01, Name = "Magazyn główny", Street = "ul. Św Filipa 1x", ZipCode = "31-001", City = "Kraków" };
            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Create(emp) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public async Task WarehouseDetails_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Details(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task WarehouseDetails_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Details(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task WarehouseDetails_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Details(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        
        [Test]
        public async Task WarehouseEditGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Edit(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task WarehouseDeleteGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Delete(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task WarehouseDeleteGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Delete(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task WarehouseDeleteGet_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new WarehousesController(_context, _logger.Object);
            var result = await controller.Delete(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
    }
}
