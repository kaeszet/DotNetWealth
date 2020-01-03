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
    public class DotNetWMSTests_Kamil_Departments : DotNetWMSTests_Base
    {
        [SetUp]
        public void Setup()
        {  
            
        }
        
       
        [Test]
        public void Model_CheckIsModelValidIfRequiredFieldAreNotFilled_ReturnFalse()
        {
            var emp = new Department();
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void Model_CheckIsModelValidIfRequiredFieldAreFilled_ReturnTrue()
        {
            var emp = new Department() { Id = 6, Name = "Przedstawiciel" };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsTrue(isModelValid);



        }
        [Test]
        public async Task Model_CheckIsCorrectModelAssignedtoViewData_ReturnTrue()
        {
            var controller = new DepartmentsController(_context);
            await controller.Index();
            var deptCollection = (ICollection<Department>)controller.ViewData.Model;
            Assert.That(deptCollection, Is.InstanceOf(typeof(ICollection<Department>)));



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
        public async Task Details_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new DepartmentsController(_context);
            var result = await controller.Details(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNotExistingKey_ReturnNull()
        {
            
            var controller = new DepartmentsController(_context);
            var result = await controller.Details(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Details(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Details(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IsReturnedModelHasSameValues_ReturnTrue()
        {
            var dept = new Department() { Id = 1, Name = "Sprzedawca" };
            var controller = new DepartmentsController(_context);
            var vr = await controller.Details(1) as ViewResult;
            var result = vr.ViewData.Model as Department;
            Assert.IsTrue(result.Id == dept.Id && result.Name == dept.Name);
            


        }
        [Test]
        public async Task EditGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task EditGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task EditGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditGet_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_RedirectToIndexIfCorrectData_ReturnTrue()
        {
            
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var dept = _context.Departments.Find(3);
            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(3, dept) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public async Task EditPost_EditRecordWithNoExistingId_ReturnNotFoundResult()
        {
            var dept = _context.Departments.Find(1);
            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(99, dept);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_EditRecordWithDifferentId_ReturnNotFoundResult()
        {
            var dept = _context.Departments.Find(2);
            var controller = new DepartmentsController(_context);
            var result = await controller.Edit(1, dept);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public void EditPost_DbUpdateConcurrencyException_ThrowsException()
        {
           
            //TODO: DbUpdateConcurrencyException test


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new DepartmentsController(_context);
            var result = await controller.Delete(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Delete(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new DepartmentsController(_context);
            var result = await controller.Delete(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeleteGet_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new DepartmentsController(_context);
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

            var dept = new Department() { Id = 5, Name = "Przestawiciel" };
            var controller = new DepartmentsController(_context);
            await controller.Create(dept);
            var result = await controller.DeleteConfirmed(5) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        



    }
}