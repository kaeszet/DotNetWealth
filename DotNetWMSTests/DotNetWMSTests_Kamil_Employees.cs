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
    public class DotNetWMSTests_Kamil_Employees : DotNetWMSTests_Base
    {
        [SetUp]
        public void Setup()
        {

        }
        
        [Test]
        public void Model_CheckIsModelValidIfFilledWithLenghtException_ReturnFalse()
        {

            var emp = new Employee() { Id = 4, Name = "Jessica", Surname = "Testowa", City = "Kraków", DepartmentId = 2, Pesel = "123456789041111111111", Street = "św. Filipa 17", ZipCode = "30-000" };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);


        }
        [Test]
        public void Model_CheckIsModelValidIfFilledWithRegexException_ReturnFalse()
        {

            var emp = new Employee() { Id = 4, Name = "Jessica", Surname = "Testowa", City = "Kraków", DepartmentId = 2, Pesel = "12345678904", Street = "św. Filipa 17", ZipCode = "30000" };
            ICollection<ValidationResult> results;
            var isModelValid = TryValidate(emp, out results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.AreEqual(arr[0].ErrorMessage, "Nieprawidłowy format kodu pocztowego xx-xxx");
            Assert.IsFalse(isModelValid, arr[0].ErrorMessage);


        }
        [Test]
        public void Model_CheckIsModelValidIfRequiredFieldAreNotFilled_ReturnFalse()
        {
            var emp = new Employee();
            var isModelValid = TryValidate(emp, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void Model_CheckIsModelValidIfRequiredFieldAreFilled_ReturnTrue()
        {
            var emp = new Employee() { Id = 4, Name = "Jessica", Surname = "Testowa", City = "Kraków", DepartmentId = 2, Pesel = "12345678904", Street = "św. Filipa 17", ZipCode = "30-000" };
            var isModelValid = TryValidate(emp, out _);
            Assert.IsTrue(isModelValid);



        }
        [Test]
        public async Task Model_CheckIsCorrectModelAssignedtoViewData_ReturnTrue()
        {
            var controller = new EmployeesController(_context);
            await controller.Index();
            var empCollection = (ICollection<Employee>)controller.ViewData.Model;
            Assert.That(empCollection, Is.InstanceOf(typeof(ICollection<Employee>)));



        }
        [Test]
        public async Task Index_GetListOfEmployees_ReturnCorrectType()
        {
            var controller = new EmployeesController(_context);
            var result = await controller.Index() as ViewResult;
            Assert.IsAssignableFrom<List<Employee>>(result.Model);
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public async Task Index_GetListOfEmployees_ReturnList()
        {
            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var items = await _context.Employees.ToListAsync();
            
            Assert.AreEqual(3, items.Count);

        }
        [Test]
        public async Task Index_CheckAreViewResultAndModelObjectNotNull_ReturnTrue()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Index() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNotNull(result.Model);

        }
        [Test]
        public async Task Index_IsViewNameReturnEmptyString_ReturnTrue()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Index() as ViewResult;
            Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));


        }
        [Test]
        public void CreateGet_GetCreateView_ReturnViewObjectWithoutInjectedData()
        {

            var controller = new EmployeesController(_context);
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

            var emp = new Employee() { Id = 4, Name = "Jessica", Surname = "Testowa", City = "Kraków", DepartmentId = 2, Pesel = "12345678904", Street = "św. Filipa 17", ZipCode = "30-000" };
            var controller = new EmployeesController(_context);
            var result = await controller.Create(emp) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public void Create_TryToAddRecordWithExistingId_ThrowsException()
        {
            var emp = new Employee() { Id = 1, Name = "Jessica", Surname = "Testowa", City = "Kraków", DepartmentId = 2, Pesel = "12345678904", Street = "św. Filipa 17", ZipCode = "30-000" };
            var controller = new EmployeesController(_context);
            Assert.That(async () => await controller.Create(emp), Throws.InvalidOperationException);

        }
        [Test]
        public async Task Details_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new EmployeesController(_context);
            var result = await controller.Details(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Details(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task Details_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Details(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Details(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Details_IsReturnedModelHasSameValues_ReturnTrue()
        {
            var emp = _context.Employees.Find(1);
            var controller = new EmployeesController(_context);
            var vr = await controller.Details(1) as ViewResult;
            var result = vr.ViewData.Model as Employee;
            Assert.IsTrue(result.Id == emp.Id && result.Name == emp.Name);

        }
        [Test]
        public async Task EditGet_CheckRecordWithExistingKey_ReturnViewResult()
        {
            var controller = new EmployeesController(_context);
            var result = await controller.Edit(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task EditGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Edit(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task EditGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Edit(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditGet_IfIdDoesntExist_ReturnNotFoundResult()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Edit(99);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditAPost_RedirectToIndexIfCorrectData_ReturnTrue()
        {

            var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            DotNetWMSContext _context = new DotNetWMSContext(_options);
            _context.Database.EnsureCreated();
            Initialize(_context);

            var emp = _context.Employees.Find(3);
            var controller = new EmployeesController(_context);
            var result = await controller.Edit(3, emp) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
        [Test]
        public async Task EditPost_EditRecordWithNoExistingId_ReturnNotFoundResult()
        {
            var emp = _context.Employees.Find(1);
            var controller = new EmployeesController(_context);
            var result = await controller.Edit(99, emp);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task EditPost_EditRecordWithDifferentId_ReturnNotFoundResult()
        {
            var emp = _context.Employees.Find(2);
            var controller = new EmployeesController(_context);
            var result = await controller.Edit(1, emp);
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
            var controller = new EmployeesController(_context);
            var result = await controller.Delete(1) as ViewResult;
            Assert.IsNotNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNotExistingKey_ReturnNull()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Delete(99) as ViewResult;
            Assert.IsNull(result);


        }
        [Test]
        public async Task DeleteGet_CheckRecordWithNull_ReturnNotFoundResult()
        {

            var controller = new EmployeesController(_context);
            var result = await controller.Delete(null);
            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task DeleteGet_IfIdDoesntExist_ReturnNotFoudResult()
        {

            var controller = new EmployeesController(_context);
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

            var emp = new Employee() { Id = 4, Name = "Jessica", Surname = "Testowa", City = "Kraków", DepartmentId = 2, Pesel = "12345678904", Street = "św. Filipa 17", ZipCode = "30-000" };
            var controller = new EmployeesController(_context);
            await controller.Create(emp);
            var result = await controller.DeleteConfirmed(4) as RedirectToActionResult;
            Assert.IsTrue(result.ActionName == nameof(controller.Index));


        }
    }
}
