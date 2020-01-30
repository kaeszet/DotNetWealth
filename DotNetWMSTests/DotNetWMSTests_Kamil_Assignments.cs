using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
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
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public async Task Assign_WhenCalled_GetCollectionOfItems()
        {

            var controller = new ItemsController(_context);
            await controller.Assign_to_employee(string.Empty, string.Empty);
            var itemsCollection = (ICollection<Item>)controller.ViewData.Model;
            Assert.That(itemsCollection, Is.InstanceOf(typeof(ICollection<Item>)));


        }
        [Test]
        public async Task Assign_WhenCalled_CreateViewResultObject()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Assign_to_employee(string.Empty, string.Empty) as ViewResult;
            Assert.IsAssignableFrom<List<Item>>(result.Model);
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public async Task Assign_Confirm_WhenCalledWithCorrectId_ReturnModelWhichIsNotNull()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Assign_to_employee_confirm(1) as ViewResult;
            Assert.IsNotNull(result.Model);


        }
        [Test]
        public async Task Assign_Confirm_WhenCalledWithIncorrectId_ReturnNotFoundResult()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Assign_to_employee_confirm(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
            result = result as ViewResult;
            Assert.IsNull(result);

        }
        [Test]
        public async Task Assign_Confirm_WhenCalledWithNull_ReturnNotFoundResult()
        {
            var controller = new ItemsController(_context);
            var result = await controller.Assign_to_employee_confirm(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
    
        }
        [Test]
        public async Task Assign_Confirm_WhenSaveWithNoChangesInView_ReturnActionResult()
        {
            var controller = new ItemsController(_context);
            FieldInfo info = controller.GetType().GetField("ItemQuantity", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 3.0M);
            var item = _context.Items.Find(1);
            var result = await controller.Assign_to_employee_confirm(1, item);
            Assert.That(result, Is.InstanceOf(typeof(IActionResult)));

        }
        [Test]
        public async Task Assign_Confirm_WhenAttemptToSaveItemWithNegativeQuantity_ReturnErrorMessage()
        {
            var controller = new ItemsController(_context);
            FieldInfo info = controller.GetType().GetField("ItemQuantity", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 3.0M);
            var item = _context.Items.Find(1);
            item.Quantity = -1.0M;
            var result = await controller.Assign_to_employee_confirm(1, item) as ViewResult;
            
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
            var controller = new ItemsController(_context);
            FieldInfo info = controller.GetType().GetField("ItemQuantity", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 3.0M);
            info = controller.GetType().GetField("ItemEmployeeId", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 1);
            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_employee_confirm(1, item) as ViewResult;

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
            var controller = new ItemsController(_context);
            FieldInfo info = controller.GetType().GetField("ItemQuantity", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 3.0M);
            var item = _context.Items.Find(1);
            item.Quantity = 4.0M;
            var result = await controller.Assign_to_employee_confirm(1, item) as ViewResult;

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
            var controller = new ItemsController(_context);
            FieldInfo info = controller.GetType().GetField("ItemQuantity", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 3.0M);
            info = controller.GetType().GetField("ItemEmployeeId", BindingFlags.NonPublic | BindingFlags.Static);
            info.SetValue(null, 2);
            var item = _context.Items.Find(1);
            item.Quantity = 2.0M;
            var result = await controller.Assign_to_employee_confirm(1, item);
            Assert.That(result, Is.InstanceOf(typeof(ActionResult)));


        }

    }
}
