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
    public class DotNetWMSTests_Kamil : DotNetWMSTests_Base
    {
        [SetUp]
        public void Setup()
        {                  
        }

        [Test]
        public async Task Index_GetListOfDepartments_ReturnCorrectType()
        {
            var controller = new DepartmentsController(_context);
            var result = await controller.Index();
            var objectResult = result as ViewResult;
            Assert.IsAssignableFrom<List<Department>>(objectResult.Model);
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public async Task Index_GetListOfDepartments_ReturnList()
        {
           
            var itemsCount = await _context.Departments.CountAsync();
            var items = await _context.Departments.ToListAsync();
            //Assert.AreEqual(4, items.Count); TODO - inmemory, nowy guid
            Assert.AreEqual(itemsCount, items.Count);
            
   

        }
        [Test]
        public async Task Index_CheckAreViewResultAndModelObjectNotNull_ReturnTrue()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);

        }
        [Test]
        public async Task Index_IsViewNameReturnEmptyString_ReturnTrue()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Index() as ViewResult;
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));


        }
        [Test]
        public void Create_GetCreateView_ReturnViewObjectWithoutInjectedData()
        {

            var controller = new DepartmentsController(_context);
            var result = controller.Create() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);


        }
        [Test]
        public async Task Create_AddNewRecordToDb_RecordAdded()
        {
            var dept = new Department() { Id = 5, Name = "Kierowca" };
            var controller = new DepartmentsController(_context);
            var result = await controller.Create(dept) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public void Create_TryToAddRecordWithExistingId_ThrowsException()
        {
            var dept = new Department() { Id = 1, Name = "Kierowca" };
            var controller = new DepartmentsController(_context);
            Assert.That(async () => await controller.Create(dept), Throws.InvalidOperationException);
            


        }
        
        [Test]
        public void Create_CheckIsModelValidIfSendValidInstance_ReturnTrue()
        {
            var dept = new Department() { Id = 6, Name = "Przedstawiciel"};
            var context = new ValidationContext(dept, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Department), typeof(Department)), typeof(Department));

            var isModelStateValid = Validator.TryValidateObject(dept, context, results, true);
            Assert.IsTrue(isModelStateValid);



        }
        [Test]
        public void Create_CheckIsModelValidIfSendNull_ReturnFalse()
        {
            var dept = new Department();
            var context = new ValidationContext(dept, null, null);
            var results = new List<ValidationResult>();
            TypeDescriptor.AddProviderTransparent(new AssociatedMetadataTypeTypeDescriptionProvider(typeof(Department), typeof(Department)), typeof(Department));

            var isModelStateValid = Validator.TryValidateObject(dept, context, results, true);
            Assert.IsFalse(isModelStateValid);



        }
        [Test]
        public async Task Details_1()
        {
            var controller = new DepartmentsController(_context);
            var result = await controller.Details(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task Details_2()
        {
            
            var controller = new DepartmentsController(_context);
            var result = await controller.Details(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task Details_3()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Details(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_4()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Details(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_5()
        {
            var dept = new Department() { Id = 1, Name = "Sprzedawca" };
            var controller = new DepartmentsController(_context);
            var vr = await controller.Details(1) as ViewResult;
            var result = vr.ViewData.Model as Department;
            Assert.IsTrue(result.Id == dept.Id && result.Name == dept.Name);
            


        }
        [Test]
        public async Task Edit_1()
        {
            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task Edit_2()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task Edit_3()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Edit_4()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Edit_5()
        {
            var dept = _context.Departments.Find(1);
            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(1, dept) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }




    }
}