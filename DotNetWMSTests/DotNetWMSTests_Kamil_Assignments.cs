using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
        public async Task Assign_WhenCalled_GetCollectionOfItems()
        {

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            controller.ItemAssignment(string.Empty, string.Empty, null, null, null);
            var itemsCollection = (ICollection<ItemsAssignmentViewModel>)controller.ViewData.Model;
            Assert.That(itemsCollection, Is.InstanceOf(typeof(ICollection<ItemsAssignmentViewModel>)));


        }
        [Test]
        public async Task Assign_WhenCalled_CreateViewResultObject()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = controller.ItemAssignment(string.Empty, string.Empty, null, null, null) as ViewResult;
            Assert.IsAssignableFrom<List<ItemsAssignmentViewModel>>(result.Model);
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public async Task Assign_Confirm_WhenCalledWithCorrectId_ReturnModelWhichIsNotNull()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_employee(1) as ViewResult;
            Assert.IsNotNull(result.Model);


        }
        [Test]
        public async Task Assign_Confirm_WhenCalledWithIncorrectId_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_employee(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
            result = result as ViewResult;
            Assert.IsNull(result);

        }
        [Test]
        public async Task Assign_Confirm_WhenCalledWithNull_ReturnNotFoundResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var result = await controller.Assign_to_employee(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
    
        }
        [Test]
        public async Task Assign_Confirm_WhenSaveWithNoChangesInView_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_employee(1, item);
            Assert.That(result, Is.InstanceOf(typeof(IActionResult)));

        }
        [Test]
        public async Task Assign_Confirm_WhenAttemptToSaveItemWithNegativeQuantity_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            var item = _context.Items.Find(1);
            item.Quantity = -1.0M;
            var result = await controller.Assign_to_employee(1, item) as ViewResult;
            
            foreach (ModelStateEntry modelState in result.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Assert.AreEqual(error.ErrorMessage, $"Nie można przekazać {item.Quantity} sztuk");
                }
            }

        }
        [Test]
        public async Task Assign_Confirm_WhenAttemptToSaveItemWithDifferentQuantityAndSameEmpoloyee_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            
            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_employee(1, item) as ViewResult;

            foreach (ModelStateEntry modelState in result.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Assert.AreEqual(error.ErrorMessage, $"Nie można przekazać przedmiotu temu samemu pracownikowi!");
                }
            }

        }
        [Test]
        public async Task Assign_Confirm_WhenAttemptToSaveItemWithHigherQuantityThanBefore_ReturnErrorMessage()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);
            
            var item = _context.Items.Find(1);
            item.Quantity = 4.0M;
            var result = await controller.Assign_to_employee(1, item) as ViewResult;

            foreach (ModelStateEntry modelState in result.ViewData.ModelState.Values)
            {
                foreach (ModelError error in modelState.Errors)
                {
                    Assert.AreEqual(error.ErrorMessage, $"Ilość przekazanego przedmiotu nie może być wyższa, niż stan w magazynie");
                }
            }

        }
        [Test]
        public async Task Assign_Confirm_WhenSaveAndUsingCorrectData_ReturnActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();

            var fakeContextAccessor = new Mock<IHttpContextAccessor>();

            var controller = new ItemsController(_context, fakeUserManager.Object, fakeContextAccessor.Object, _logger);

            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_employee(1, item);
            Assert.That(result, Is.InstanceOf(typeof(ActionResult)));


        }

    }
}
