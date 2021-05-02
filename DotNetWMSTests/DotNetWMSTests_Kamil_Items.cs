using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    public class DotNetWMSTests_Kamil_Items : DotNetWMSTests_Base
    {
        private ILogger<ItemsController> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ItemsController>>().Object;
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
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
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

            var emp = new Item() { Id = 5, Type = "Komputer osobisty", Name = "Laptop", Producer = "Lenovo", Model = "X1 Carbon", ItemCode = "2", Quantity = 1, Units = 0, WarrantyDate = DateTime.Now, State = 0, UserId = "", WarehouseId = 2, ExternalId = 1 };
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Create(emp) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public async Task Details_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            fakeContextAccessor.Setup(s => s.HttpContext.Request.Path).Returns("/test");

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Details(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Details(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            fakeContextAccessor.Setup(s => s.HttpContext.Request.Path).Returns("/test");

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Details(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Details(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IsReturnedModelHasSameValues_ReturnTrue()
        {
            var emp = _context.Items.Find(1);
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            fakeContextAccessor.Setup(s => s.HttpContext.Request.Path).Returns("/test");

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var vr = await controller.Details(1) as ViewResult;
            var result = vr.ViewData.Model as Item;
            Assert.IsTrue(result.Id == emp.Id && result.Name == emp.Name);



        }
        [Test]
        public async Task EditGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task EditGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Edit(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_EditRecordWithNoExistingId_ReturnNotFoundResult()
        {
            var emp = _context.Items.Find(1);
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Edit(99, emp);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_EditRecordWithDifferentId_ReturnNotFoundResult()
        {
            var emp = _context.Items.Find(2);
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Edit(1, emp);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Delete(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Delete(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Delete(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeleteGet_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
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

            var emp = new Item() { Id = 5, Type = "Komputer osobisty", Name = "Laptop", Producer = "Lenovo", Model = "X1 Carbon", ItemCode = "2", Quantity = 1, Units = 0, WarrantyDate = DateTime.Now, State = 0, UserId = "", WarehouseId = 2, ExternalId = 1 };
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            await controller.Create(emp);
            var result = await controller.DeleteConfirmed(4) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public async Task Index_WhenCalled_ReturnViewWithListOfItems()
        { 

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Index("", "") as ViewResult;

            var model = (List<Item>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Count == _context.Items.Local.Count);


        }
        [Test]
        public async Task Index_WhenCalledWithSearchParameter_ReturnViewWithFilteredList()
        {
            string search = "Komputer";

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Index("", search) as ViewResult;

            var model = (List<Item>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model[0].Name == search);


        }
        [Test]
        public async Task Index_WhenCalledWithOrderParameter_ReturnViewWithListNameDescending()
        {
            string order = "name_desc";

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Index(order , "") as ViewResult;

            var model = (List<Item>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model[^1].Name == "Komputer");


        }
        [Test]
        public async Task Index_WhenCalledWithOrderParameter_ReturnViewWithListDateDescending()
        {
            string order = "date_desc";

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Index(order, "") as ViewResult;

            var model = (List<Item>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model[0].Name == "Komputer");


        }
        [Test]
        public async Task Index_WhenCalledWithOrderParameterAndUserIsNotNull_ReturnViewWithListNameAscending()
        {
            string order = "WarrantyDate";

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns("TESTOJAN9012");
            httpContext.Setup(h => h.User.IsInRole("Admin")).Returns(true);

            var controllerContext = new ControllerContext() { HttpContext = httpContext.Object, ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() };

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger) { ControllerContext = controllerContext };

            var result = await controller.Index(order, "") as ViewResult;

            var model = (List<Item>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model[^1].Name == "Komputer");


        }
        [Test]
        public void ShowQRCode_WhenCalled_ReturnPartialView()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new DefaultHttpContext();
            var tempData = new TempDataDictionary(httpContext, Mock.Of<ITempDataProvider>())
            {
                ["QRCode"] = "test"
            };

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger)
            {
                TempData = tempData
            };

            var result = controller.ShowQrCode("") as PartialViewResult;

            Assert.That(result, Is.InstanceOf(typeof(PartialViewResult)));
            Assert.IsTrue(result.ViewName == "_QrCodePartial");

        }
        [Test]
        public async Task Create_WhenCalledWithUserId_SaveItemInDbAndReturnView()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var item = new Item() 
            { 
                Id = 5, 
                Type = "Komputer osobisty", 
                Name = "Laptop", 
                Producer = "Lenovo", 
                Model = "X1 Carbon", 
                ItemCode = "2", 
                Quantity = 1, 
                Units = 0, 
                WarrantyDate = DateTime.Now, 
                State = 0, 
                UserId = "1", 
                WarehouseId = 2, 
                ExternalId = 1 
            };

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Create(item) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == nameof(Index));

            //reset data

            var deleteItem = _context.Items.Find(5);
            _context.Items.Remove(deleteItem);
            await _context.SaveChangesAsync();

        }
        [Test]
        public async Task Create_WhenCalledAndItemIsInDatabase_ReturnSameViewWithModelError()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var item = _context.Items.Find(1);

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Create(item) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage == $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");


        }
        [Test]
        public void IsItemExists_ItemCode_IfItemIsNotExists_ReturnJsonTrue()
        {

            string itemCode = "invalidItemCode";

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = controller.IsItemExists(itemCode) as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.ToString() == "True");


        }
        [Test]
        public void IsItemExists_ItemCode_IfItemExists_ReturnJsonString()
        {

            string itemCode = _context.Items.Find(1).ItemCode;

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = controller.IsItemExists(itemCode) as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.ToString() == $"Przedmiot o kodzie ({itemCode}) został już wprowadzony!");


        }
        [Test]
        public void IsItemExists_ItemObj_IfItemIsNotExists_ReturnJsonTrue()
        {

            Item item = new Item()
            {
                Name = "test"
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = controller.IsItemExists(item) as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.ToString() == "True");


        }
        [Test]
        public void IsItemExists_ItemObj_IfItemExists_ReturnJsonString()
        {

            var item = _context.Items.Find(1);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = controller.IsItemExists(item) as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.ToString() == $"Przedmiot został już wprowadzony do systemu!");


        }
        [Test]
        public async Task EditGet_WhenCalledWithInvalidId_ReturnNotFound()
        {

            int wrongId = 99;

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Edit(wrongId) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
            


        }
        [Test]
        public async Task EditPost_WhenItemIdAndItemObjectAreValidAndNoChange_EditSuccessAndRedirect()
        {
            int itemId = 1;
            var item = _context.Items.Find(itemId);


            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Edit(itemId, item) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");



        }
        [Test]
        public async Task EditPost_WhenItemIdAndItemObjectAreValidAndItemCodeIsInUse_EditFailedAndReturnError()
        {
            int itemId = 1;
            var item = _context.Items.Find(itemId);
            item.ItemCode = "H-H";


            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Edit(itemId, item) as ViewResult;

            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage == $"Przedmiot o numerze seryjnym {item.ItemCode} został już wprowadzony do systemu!");

            //reset data

            item.ItemCode = "C-64";
            await _context.SaveChangesAsync();


        }
        [Test]
        public async Task AssignToEmployeeGet_WhenCalledAndItemIsInExternal_ReturnModelStateError()
        {
            int itemId = 3;

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Assign_to_employee(itemId) as ViewResult;

            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage.Contains("Przedmiot w posiadaniu zewnętrznej firmy:"));


        }
        [Test]
        public async Task AssignToEmployeePost_WhenCalledAndItemIsInExternal_ReturnModelStateError()
        {
            int itemId = 3;
            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Assign_to_employee(itemId, item) as ViewResult;

            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage.Contains("Przedmiot w posiadaniu zewnętrznej firmy:"));


        }
        [Test]
        public async Task AssignToEmployeePost_WhenCalledAndItemsQuantitiesAreEqual_UpdateItemAndReturnRedirectToActionResult()
        {
            int itemId = 1;
            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Assign_to_employee(itemId, item) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Index");


        }
        [Test]
        public async Task AssignToWarehousePost_WhenCalledAndItemsQuantitiesAreEqual_UpdateItemAndReturnRedirectToActionResult()
        {
            int itemId = 1;
            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Assign_to_warehouse(itemId, item) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Index");


        }
        [Test]
        public async Task AssignToExternalPost_WhenCalledAndItemsQuantitiesAreEqual_UpdateItemAndReturnRedirectToActionResult()
        {
            int itemId = 1;
            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.Assign_to_external(itemId, item) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Index");


        }
    }
}

