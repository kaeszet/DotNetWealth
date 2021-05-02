using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Security.Principal;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    class DotNetWMSTests_Kamil_Assignments : DotNetWMSTests_Base
    {
        private ILogger<ItemsController> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<ItemsController>>().Object;
        }
        [Test]
        public async Task AssignToEmployee_Get_WhenCalledWithCorrectId_ReturnModelWhichIsNotNull()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_employee(1) as ViewResult;
            Assert.IsNotNull(result.Model);


        }
        [Test]
        public async Task AssignToWarehouse_Get_WhenCalledWithCorrectId_ReturnModelWhichIsNotNull()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_warehouse(1) as ViewResult;
            Assert.IsNotNull(result.Model);


        }
        [Test]
        public async Task AssignToExternal_Get_WhenCalledWithCorrectId_ReturnModelWhichIsNotNull()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_external(1) as ViewResult;
            Assert.IsNotNull(result.Model);


        }
        [Test]
        public async Task AssignToEmployee_Get_WhenCalledWithIncorrectId_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_employee(99);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToWarehouse_Get_WhenCalledWithIncorrectId_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_warehouse(99);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToExternal_Get_WhenCalledWithIncorrectId_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_external(99);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToEmployee_Get_WhenCalledWithNull_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_employee(null);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
    
        }
        [Test]
        public async Task AssignToWarehouse_Get_WhenCalledWithNull_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_warehouse(null);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToExternal_Get_WhenCalledWithNull_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_external(null);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenSaveWithNoChangesInView_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_employee(1, item);

            Assert.That(result, Is.InstanceOf(typeof(IActionResult)));

        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenSaveWithNoChangesInView_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_warehouse(1, item);

            Assert.That(result, Is.InstanceOf(typeof(IActionResult)));

        }
        [Test]
        public async Task AssignToExternal_Post_WhenSaveWithNoChangesInView_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_external(1, item);

            Assert.That(result, Is.InstanceOf(typeof(IActionResult)));

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenDeliveredIdIsDifferent_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_employee(99, item);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenDeliveredIdIsDifferent_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_warehouse(99, item);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToExternal_Post_WhenDeliveredIdIsDifferent_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_external(99, item);

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenModelIsInvalid_ReturnSameView()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            controller.ModelState.AddModelError("", "Testowy błąd");
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_employee(1, item) as ViewResult;
            
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.AreEqual(result.Model, item);
            Assert.IsTrue(result.ViewData.ContainsKey("UserId"));

        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenModelIsInvalid_ReturnSameView()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            controller.ModelState.AddModelError("", "Testowy błąd");
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_warehouse(1, item) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.AreEqual(result.Model, item);
            Assert.IsTrue(result.ViewData.ContainsKey("WarehouseId"));

        }
        [Test]
        public async Task AssignToExternal_Post_WhenModelIsInvalid_ReturnSameView()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            controller.ModelState.AddModelError("", "Testowy błąd");
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_external(1, item) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.AreEqual(result.Model, item);
            Assert.IsTrue(result.ViewData.ContainsKey("ExternalId"));

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenAttemptToSaveItemWithNegativeQuantity_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            item.Quantity = -1.0M;
            var result = await controller.Assign_to_employee(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Nie można przekazać {item.Quantity} sztuk");

        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenAttemptToSaveItemWithNegativeQuantity_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            item.Quantity = -1.0M;
            var result = await controller.Assign_to_warehouse(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Nie można przekazać {item.Quantity} sztuk");

        }
        [Test]
        public async Task AssignToExternal_Post_WhenAttemptToSaveItemWithNegativeQuantity_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            item.Quantity = -1.0M;
            var result = await controller.Assign_to_external(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Nie można przekazać {item.Quantity} sztuk");

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenAttemptToSaveItemWithDifferentQuantityAndSameEmpoloyee_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            
            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_employee(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Nie można przekazać przedmiotu temu samemu pracownikowi!");

        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenAttemptToSaveItemWithDifferentQuantityAndSameEmpoloyee_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_warehouse(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Nie można przekazać przedmiotu do tego samego magazynu!");

        }
        [Test]
        public async Task AssignToExternal_Post_WhenAttemptToSaveItemWithDifferentQuantityAndSameEmpoloyee_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_external(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Nie można ponownie przekazać przedmiotu temu samemu podmiotowi!");

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenAttemptToSaveItemWithHigherQuantityThanBefore_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            
            var item = _context.Items.Find(1);
            item.Quantity = 4.0M;
            var result = await controller.Assign_to_employee(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");

        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenAttemptToSaveItemWithHigherQuantityThanBefore_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 4.0M;
            var result = await controller.Assign_to_warehouse(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");

        }
        [Test]
        public async Task AssignToExternal_Post_WhenAttemptToSaveItemWithHigherQuantityThanBefore_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 4.0M;
            var result = await controller.Assign_to_external(1, item) as ViewResult;

            Assert.AreEqual(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");

        }
        [Test]
        public async Task AssignToEmployee_Post_WhenSaveAndUsingCorrectData_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_employee(1, item);

            Assert.That(result, Is.InstanceOf(typeof(ActionResult)));


        }
        [Test]
        public async Task AssignToWarehouse_Post_WhenSaveAndUsingCorrectData_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_warehouse(1, item);

            Assert.That(result, Is.InstanceOf(typeof(ActionResult)));


        }
        [Test]
        public async Task AssignToExternal_Post_WhenSaveAndUsingCorrectData_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeContextAccessor = new Mock<IHttpContextAccessor>();
            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_external(1, item);

            Assert.That(result, Is.InstanceOf(typeof(ActionResult)));


        }
        [Test]
        public void ItemAssignment_WhenCalled_GetCollectionOfItems()
        {

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            controller.ItemAssignment(string.Empty, string.Empty, null, null, null);
            var itemsCollection = (ICollection<ItemsAssignmentViewModel>)controller.ViewData.Model;
            Assert.That(itemsCollection, Is.InstanceOf(typeof(ICollection<ItemsAssignmentViewModel>)));


        }
        [Test]
        public void ItemAssignment_WhenCalled_CreateViewResultObject()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignment(string.Empty, string.Empty, null, null, null) as ViewResult;
            Assert.IsAssignableFrom<List<ItemsAssignmentViewModel>>(result.Model);
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public void ItemAssignment_WhenCalledWithSearchParameter_CreateViewResultObjectWithSearchedItem()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignment(string.Empty, "C-64", null, null, null) as ViewResult;
            var model = (List<ItemsAssignmentViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model[0].Model == "Commodore 64");


        }
        [Test]
        public void ItemAssignment_WhenCalledWithOrderParameter_CreateViewResultObjectWithOrderedList()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignment("name_desc", string.Empty, null, null, null) as ViewResult;
            var model = (List<ItemsAssignmentViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model[0].Model == "Legion 7");


        }
        [Test]
        public void ItemAssignmentConfirmation_IfViewModelQuantityIsHigherThanDBValue_ReturnViewResultWithError()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1, IsChecked = true, Quantity = 5.0M },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3 },
            };

            var item = _context.Items.Find(1);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignmentConfirmation("", vm) as ViewResult;
            var model = (List<ItemsAssignmentViewModel>)result.ViewData.Model;
            

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignment");
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage == $"Nie można przekazać więcej sztuk niż jest na stanie. Wprowadzono: {vm[0].Quantity}, stan: {item.Quantity}{item.Units}, {item.Assign}");


        }
        [Test]
        public void ItemAssignmentConfirmation_IfViewModelQuantityIsLowerThanZero_ReturnViewResultWithError()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1, IsChecked = true, Quantity = -1.0M },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3 }
            };

            var item = _context.Items.Find(1);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignmentConfirmation("", vm) as ViewResult;


            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignment");
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage == $"Nie można przekazać 0 sztuk. Wprowadzono: {vm[0].Quantity}, {item.Assign}");


        }
        [Test]
        public void ItemAssignmentConfirmation_IfItemIsAssignedToExternal_ReturnViewResultWithError()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1 },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3, IsChecked = true, Quantity = 1.0M }
            };

            var item = _context.Items.Find(3);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignmentConfirmation("ToUser", vm) as ViewResult;


            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignment");
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage == $"Przedmiot \"{item.FullName}\" jest w posiadaniu zewnętrznej firmy. Przedmiot można przypisać do pracownika, gdy zostanie dostarczony/zwrócony");


        }
        [Test]
        public void ItemAssignmentConfirmation_IfNoItemIsChecked_ReturnViewResultWithError()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1 },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3 }
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignmentConfirmation("ToUser", vm) as ViewResult;


            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignment");
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0]
                .ErrorMessage == "Nie wybrano żadnego przedmiotu do przekazania");


        }
        [Test]
        public void ItemAssignmentConfirmation_IfCorrectValueAndCorrectOptionToUser_ReturnConfirmationViewWithModel()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1, IsChecked = true, Quantity = 1.0M },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3 }
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignmentConfirmation("ToUser", vm) as ViewResult;
            var model = (ItemAssignmentConfirmationViewModel)result.ViewData.Model;
            var checkedVm = vm.FindAll(i => i.IsChecked);


            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");
            Assert.IsTrue(checkedVm[0] == model.Items[0] && checkedVm.Count == 1 && model.Items.Count == 1);

        }
        [Test]
        public void ItemAssignmentConfirmation_IfCorrectValueAndCorrectOptionToWarehouse_ReturnConfirmationViewWithModel()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1 },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3 },
                new ItemsAssignmentViewModel() { Id = 4, IsChecked = true, Quantity = 1.0M }
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignmentConfirmation("ToWarehouse", vm) as ViewResult;
            var model = (ItemAssignmentConfirmationViewModel)result.ViewData.Model;
            var checkedVm = vm.FindAll(i => i.IsChecked);

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");
            Assert.IsTrue(checkedVm[0] == model.Items[0] && checkedVm.Count == 1 && model.Items.Count == 1);

        }
        [Test]
        public void ItemAssignmentConfirmation_IfCorrectValueAndCorrectOptionToExternalAuthorizedUser_ReturnConfirmationViewWithModel()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1 },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3, IsChecked = true, Quantity = 1.0M  },
                new ItemsAssignmentViewModel() { Id = 4 }
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.IsInRole("Admin")).Returns(true);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger)
            {
                ControllerContext = context
            };

            var result = controller.ItemAssignmentConfirmation("ToExternal", vm) as ViewResult;
            var model = (ItemAssignmentConfirmationViewModel)result.ViewData.Model;
            var checkedVm = vm.FindAll(i => i.IsChecked);

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");
            Assert.IsTrue(checkedVm[0] == model.Items[0] && checkedVm.Count == 1 && model.Items.Count == 1);

        }
        [Test]
        public void ItemAssignmentConfirmation_IfCorrectValueAndCorrectOptionToExternalUnauthorizedUser_ReturnConfirmationViewWithModel()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1 },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3, IsChecked = true, Quantity = 1.0M  },
                new ItemsAssignmentViewModel() { Id = 4 }
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.IsInRole("Admin")).Returns(false);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger)
            {
                ControllerContext = context
            };

            var result = controller.ItemAssignmentConfirmation("ToExternal", vm) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ControllerName == "Administration" && result.ActionName == "AccessDenied");

        }
        [Test]
        public void ItemAssignmentConfirmation_IfModelIsInvalid_ReturnSameView()
        {
            List<ItemsAssignmentViewModel> vm = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1 },
                new ItemsAssignmentViewModel() { Id = 2 },
                new ItemsAssignmentViewModel() { Id = 3 },
                new ItemsAssignmentViewModel() { Id = 4 }
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            controller.ModelState.AddModelError("", "test");

            var result = controller.ItemAssignmentConfirmation("", vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewName == "ItemAssignment");

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToUserOptionAndUserIsDifferent_RedirectToIndex()
        {
            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1, IsChecked = true, Quantity = 3.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToUser", vm) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");

            //reset data
            _context.Items.Find(1).UserId = "1";

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToWarehouseOptionAndWarehouseIsDifferent_RedirectToIndex()
        {
            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 4, IsChecked = true, Quantity = 2.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToWarehouse", vm) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");

            //reset data
            _context.Items.Find(4).WarehouseId = 1;

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToExternalOptionAndExternalIsDifferent_RedirectToIndex()
        {
            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 3, IsChecked = true, Quantity = 1.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToExternal", vm) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");

            //reset data
            _context.Items.Find(3).ExternalId = 2;

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToUserOptionAndUsersAreSame_ReturnViewResult()
        {

            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            int itemId = 1;
            string userId = "1";

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = itemId, IsChecked = true, Quantity = 3.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
                UserId = userId
            };

            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToUser", vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage == $"Przedmiot {item.FullName} jest już na stanie o wybranego pracownika");

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToWarehouseOptionAndWarehousesAreSame_ReturnViewResult()
        {

            int itemId = 4;
            int warehouseId = 1;

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = itemId, IsChecked = true, Quantity = 2.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
                WarehouseId = warehouseId
            };

            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToWarehouse", vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage == $"Przedmiot {item.FullName} jest już w wybranym magazynie");

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToExternalOptionAndExternalsAreSame_ReturnViewResult()
        {

            int itemId = 3;
            int externalId = 2;

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = itemId, IsChecked = true, Quantity = 1.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
                ExternalId = externalId
            };

            var item = _context.Items.Find(itemId);

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToExternal", vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage == $"Przedmiot {item.FullName} jest już u wybranego kontahenta");

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToUserOptionAndUserIsSameAndDocNeeded_ReturnViewResult()
        {

            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1, IsChecked = true, Quantity = 3.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
                IsDocumentNeeded = true,
                UserId = "1"
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns("TESTOJAN9012");

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger)
            {
                ControllerContext = context
            };

            var result = await controller.ItemAssignmentSaveInDb("ToUser", vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ItemAssignmentConfirmation");

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToUserOptionAndUserAndValueAreDifferent_RedirectToIndex()
        { 

            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 1, IsChecked = true, Quantity = 2.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToUser", vm) as RedirectToActionResult;

            var item = _context.Items.Find(1);

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");
            Assert.IsNull(item);

            _context.Database.EnsureDeleted();
            _context.Dispose();

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToWarehouseOptionAndWarehouseAndValueAreDifferent_RedirectToIndex()
        {

            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 4, IsChecked = true, Quantity = 1.0M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToWarehouse", vm) as RedirectToActionResult;

            var item = _context.Items.Find(4);

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");
            Assert.IsNull(item);

        }
        [Test]
        public async Task ItemAssignmentSaveInDb_WhenCalledWithToExternalOptionAndExternalAndValueAreDifferent_RedirectToIndex()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
              .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
              .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            List<ItemsAssignmentViewModel> items = new List<ItemsAssignmentViewModel>()
            {
                new ItemsAssignmentViewModel() { Id = 3, IsChecked = true, Quantity = 0.5M },
            };

            ItemAssignmentConfirmationViewModel vm = new ItemAssignmentConfirmationViewModel()
            {
                Items = items,
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var result = await controller.ItemAssignmentSaveInDb("ToExternal", vm) as RedirectToActionResult;

            var item = _context.Items.Find(3);

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");
            Assert.IsNull(item);

        }

    }
}
