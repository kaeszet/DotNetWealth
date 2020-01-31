using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
	public class DotNetWMSTests_Krzysztof_Externals : DotNetWMSTests_Base
	{
		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void Model_External_CheckCharacterLengthCompatibility_ReturnFalse()
		{

			var emp = new External() { Id = 4, Type = ContractorType.Podwykonawca, Name = "Janusz", TaxId = "4445556677", Street = "św. Filipa 17", ZipCode = "30000", City = "Kraków" };
			var isModelValid = TryValidate(emp, out _);
			Assert.IsFalse(isModelValid);


		}

		[Test]
		public void Model_External_CheckValidOfFilledWithRegexException_ReturnFalse()
		{

			var ext = new External() { Id = 4, Type = ContractorType.Podwykonawca, Name = "Janusz", TaxId = "444-555-66-77", Street = "św. Filipa 17", ZipCode = "30-000", City = "Kraków" };
			ICollection<ValidationResult> results;
			var isModelValid = TryValidate(ext, out results);
			ValidationResult[] arr = new ValidationResult[5];
			results.CopyTo(arr, 0);
			Assert.AreEqual(arr[0].ErrorMessage, "The field NIP must be a string with a maximum length of 10.");
			Assert.IsFalse(isModelValid, arr[0].ErrorMessage);


		}

		[Test]
		public void Model_External_CheckModelValidIsEmpty_ReturnFalse()
		{
			var ext = new External();
			var isModelValid = TryValidate(ext, out _);
			Assert.IsFalse(isModelValid);
		}

		[Test]
		public void Model_External_CheckValidRequiredFieldAreFilled_ReturnTrue()
		{
			var ext = new External() { Id = 4, Type = ContractorType.Podwykonawca, Name = "Janusz", TaxId = "4445556677", Street = "św. Filipa 17", ZipCode = "30-000", City = "Kraków" };
			var isModelValid = TryValidate(ext, out _);
			Assert.IsTrue(isModelValid);
		}

		[Test]
		public async Task Model_External_CheckIsModelAssignedtoViewData_ReturnTrue()
		{
			var controller = new ExternalsController(_context);
			await controller.Index(string.Empty, string.Empty);
			var extCollection = (ICollection<External>)controller.ViewData.Model;
			Assert.That(extCollection, Is.InstanceOf(typeof(ICollection<External>)));
		}




		[Test]
		public async Task Index_Externals_ListOfExternals_ReturnCorrectType()
		{
			var controller = new ExternalsController(_context);
			var result = await controller.Index(string.Empty, string.Empty) as ViewResult;

			Assert.IsAssignableFrom<List<External>>(result.Model);
			Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


		}

		[Test]
		public async Task Index_Externals_ListOfExterlans_ReturnList()
		{
			var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			   .Options;

			DotNetWMSContext _context = new DotNetWMSContext(_options);
			_context.Database.EnsureCreated();
			Initialize(_context);

			var items = await _context.Externals.ToListAsync();

			Assert.AreEqual(3, items.Count);
		}

		[Test]
		public async Task Index_Externals_CheckAreResultAndModelNotNull_ReturnTrue()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Index(string.Empty, string.Empty) as ViewResult;
			Assert.IsNotNull(result);
			Assert.IsNotNull(result.Model);
		}

		[Test]
		public async Task Index_Externals_ReturnEmptyStringNameOfView_ReturnTrue()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Index(string.Empty, string.Empty) as ViewResult;
			Assert.IsTrue(string.IsNullOrEmpty(result.ViewName));


		}

		[Test]
		public void Externals_CreateGet_ReturnViewWithoutInjectedData()
		{

			var controller = new ExternalsController(_context);
			var result = controller.Create() as ViewResult;
			Assert.IsNotNull(result);
			Assert.IsNull(result.Model);
		}

		[Test]
		public async Task Externals_CreatePost_NewRecordToDatabase_RecordAdded()
		{
			var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
			   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
			   .Options;

			DotNetWMSContext _context = new DotNetWMSContext(_options);
			_context.Database.EnsureCreated();
			Initialize(_context);

			var emp = new External() { Id = 4, Type = ContractorType.Podwykonawca, Name = "Janusz", TaxId = "4445556677", Street = "św. Filipa 17", ZipCode = "30-000", City = "Kraków" };
			var controller = new ExternalsController(_context);
			var result = await controller.Create(emp) as RedirectToActionResult;
			Assert.IsTrue(result.ActionName == nameof(controller.Index));
		}

		[Test]
		public void Create_Externals_AddRecordWithTheSameId_ThrowsException()
		{
			var ext = new External() { Id = 2, Type = ContractorType.Podwykonawca, Name = "Janusz", TaxId = "4445556677", Street = "św. Filipa 17", ZipCode = "30-000", City = "Kraków" };
			var controller = new ExternalsController(_context);
			Assert.That(async () => await controller.Create(ext), Throws.InvalidOperationException);

		}

		[Test]
		public async Task Details_Externals_RecordWithTheSameKey_ReturnViewResult()
		{
			var controller = new ExternalsController(_context);
			var result = await controller.Details(1) as ViewResult;
			Assert.IsNotNull(result);
		}

		[Test]
		public async Task Details_Externals_RecordWithNotExistingKey_ReturnNull()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Details(99) as ViewResult;
			Assert.IsNull(result);
		}

		[Test]
		public async Task Details_Externals_RecordWithNull_ReturnNotFound()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Details(null);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
		}

		[Test]
		public async Task Details_Externals_IdDoesntExist_ReturnNotFound()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Details(99);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
		}

		[Test]
		public async Task Details_Externals_ModelHasTheSameValues_ReturnTrue()
		{
			var ext = _context.Externals.Find(1);
			var controller = new ExternalsController(_context);
			var vr = await controller.Details(1) as ViewResult;
			var result = vr.ViewData.Model as External;
			Assert.IsTrue(result.Id == ext.Id && result.Name == ext.Name);
		}

		[Test]
		public async Task EditGet_Externals_RecordWithTheSameKey_ReturnView()
		{
			var controller = new EmployeesController(_context);
			var result = await controller.Edit(1) as ViewResult;
			Assert.IsNotNull(result);
		}

		[Test]
		public async Task EditGet_Externals_RecordWithNotExistingKey_ReturnNull()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Edit(99) as ViewResult;
			Assert.IsNull(result);
		}

		[Test]
		public async Task EditGet_Externals_RecordWithNull_ReturnNotFound()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Edit(null);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
		}

		[Test]
		public async Task EditGet_Externals_IdDoesntExist_ReturnNotFound()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Edit(99);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
		}

		[Test]
		public async Task EditPost_Externals_IfCorrectDataRedirectToIndex_ReturnTrue()
		{

			var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			DotNetWMSContext _context = new DotNetWMSContext(_options);
			_context.Database.EnsureCreated();
			Initialize(_context);

			var ext = _context.Externals.Find(3);
			var controller = new ExternalsController(_context);
			var result = await controller.Edit(3, ext) as RedirectToActionResult;
			Assert.IsTrue(result.ActionName == nameof(controller.Index));
		}

		[Test]
		public async Task EditPost_Externals_RecordWithNoExistingId_ReturnNotFound()
		{
			var ext = _context.Externals.Find(1);
			var controller = new ExternalsController(_context);
			var result = await controller.Edit(99, ext);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
		}

		[Test]
		public async Task EditPost_Externals_RecordWithDifferentId_ReturnNotFound()
		{
			var ext = _context.Externals.Find(2);
			var controller = new ExternalsController(_context);
			var result = await controller.Edit(1, ext);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));
		}

		[Test]
		public void EditPost_Externals_DbUpdate_ThrowsException()
		{

			//TODO: DbUpdateConcurrencyException test


		}

		[Test]
		public async Task DeleteGet_Externals_RecordWithTheSameKey_ReturnView()
		{
			var controller = new ExternalsController(_context);
			var result = await controller.Delete(1) as ViewResult;
			Assert.IsNotNull(result);


		}

		[Test]
		public async Task DeleteGet_Externals_RecordWithNotExistingKey_ReturnNull()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Delete(99) as ViewResult;
			Assert.IsNull(result);


		}

		[Test]
		public async Task DeleteGet_Externals_RecordWithNull_ReturnNotFound()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Delete(null);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


		}

		[Test]
		public async Task DeleteGet_Externals_IdDoesntExist_ReturnNotFoud()
		{

			var controller = new ExternalsController(_context);
			var result = await controller.Delete(99);
			Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


		}

		[Test]
		public async Task DeletePost_Externals_IfCorrectDataRedirectToIndex_ReturnTrue()
		{
			var _options = new DbContextOptionsBuilder<DotNetWMSContext>()
				.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
				.Options;

			DotNetWMSContext _context = new DotNetWMSContext(_options);
			_context.Database.EnsureCreated();
			Initialize(_context);

			var ext = new External() { Id = 4, Type = ContractorType.Podwykonawca, Name = "Janusz", TaxId = "4445556677", Street = "św. Filipa 17", ZipCode = "30-000", City = "Kraków" };
			var controller = new ExternalsController(_context);
			await controller.Create(ext);
			var result = await controller.DeleteConfirmed(4) as RedirectToActionResult;
			Assert.IsTrue(result.ActionName == nameof(controller.Index));


		}
	}
}
