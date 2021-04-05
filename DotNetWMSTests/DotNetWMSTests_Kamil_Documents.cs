using DotNetWMS.Controllers;
using DotNetWMS.Models;
using DotNetWMS.Resources;
using DotNetWMSTests.StaticWrapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    class DotNetWMSTests_Kamil_Documents : DotNetWMSTests_Base
    {
        private Mock<ILogger<Doc_AssignmentsController>> _logger;
        private Mock<IPrincipal> _principal;
        private Mock<IStaticWrapper> _wrapper;


        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<Doc_AssignmentsController>>();
            _principal = new Mock<IPrincipal>();
            _wrapper = new Mock<IStaticWrapper>();
        }
        [Test]
        public void Model_1()
        {
            var items = new List<Item>()
            {
                new Item { Id = 1, Type = "Elektronika", Name = "Komputer", Producer = "CBM", Model = "Commodore 64", ItemCode="C-64", Quantity = 3.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "1" },
                new Item { Id = 2, Type = "Elektronika", Name = "Laptop", Producer = "Hykker", Model = "Hello", ItemCode="H-H", Quantity = 10.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "2" }
            };

            var doc = new Doc_Assignment()
            {
                DocumentId = "Dokument1",
                Title = "Dokument testowy",
                CreationDate = DateTime.Now,
                UserFrom = "001ae934-b890-455f-8077-c32bd593a327",
                UserTo = "0bd256e0-ffcb-438e-806a-63ce0fcb2d97",
                Items = items
            };

            var isModelValid = TryValidate(doc, out ICollection<ValidationResult> results);

            Assert.IsTrue(isModelValid);

        }
        [Test]
        public void Model_2()
        {
            var items = new List<Item>()
            {
                new Item { Id = 1, Type = "Elektronika", Name = "Komputer", Producer = "CBM", Model = "Commodore 64", ItemCode="C-64", Quantity = 3.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "1" },
                new Item { Id = 2, Type = "Elektronika", Name = "Laptop", Producer = "Hykker", Model = "Hello", ItemCode="H-H", Quantity = 10.0M, Units = ItemUnits.szt, State = ItemState.InEmployee, UserId = "2" }
            };

            var doc = new Doc_Assignment()
            {
                DocumentId = "Dokument1",
                Title = "Dokument testowy",
                CreationDate = DateTime.Now,
                UserFrom = "InvalidID",
                UserTo = "InvalidID",
                Items = items
            };

            var isModelValid = TryValidate(doc, out ICollection<ValidationResult> results);
            List<ValidationResult> errors = (List<ValidationResult>)results;

            Assert.AreEqual(errors[0].ErrorMessage, "Coś poszło nie tak");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void UserExtMethods_1()
        {
            var wm = new StaticWrapper.StaticWrapper();

            _principal.SetupSequence(p => p.IsInRole("Moderator")).Returns(false);
            _principal.SetupSequence(p => p.IsInRole("Admin")).Returns(true);

            var result_1 = wm.IsInAnyRoles(_principal.Object, "Moderator,Admin");
            var result_2 = wm.IsInAnyRoles(_principal.Object, "Moderator");

            Assert.IsTrue(result_1);
            Assert.IsFalse(result_2);
        }
        [Test]
        public void UserExtMethods_2()
        {

            var wm = new StaticWrapper.StaticWrapper();

            _principal.SetupSequence(p => p.IsInRole("Moderator")).Returns(true);
            _principal.SetupSequence(p => p.IsInRole("Admin")).Returns(true);

            var result_1 = wm.IsInAllRoles(_principal.Object, "Moderator,Admin");

            _principal.SetupSequence(p => p.IsInRole("Moderator")).Returns(false);
            _principal.SetupSequence(p => p.IsInRole("Admin")).Returns(true);

            var result_2 = wm.IsInAllRoles(_principal.Object, "Moderator,Admin");

            Assert.IsTrue(result_1);
            Assert.IsFalse(result_2);

        }

        [Test]
        public async Task Index_1()
        {

            var user = new WMSIdentityUser
            {
                Id = "1",
                Name = "Janusz",
                Surname = "Testowy",
                UserName = "TestoJan9012",
                NormalizedUserName = "TESTOJAN9012",
                EmployeeNumber = "23456789012",
                City = "Kraków",
                Email = "a@a.pl",
                LoginCount = 0
            };

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns("TESTOJAN9012");

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new Doc_AssignmentsController(_context, _logger.Object)
            {
                ControllerContext = context
            };

            var result = await controller.Index("", "") as ViewResult;
            var model = (List<Doc_Assignment>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Count > 0 && model[0].UserTo == user.Id);

        }
        [Test]
        public void ConfigureDocument_1_1()
        {

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = controller.ConfigureDocument() as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.That(result.ViewData["Users"], Is.InstanceOf(typeof(SelectList)));
            Assert.That(result.ViewData["Externals"], Is.InstanceOf(typeof(SelectList)));
            Assert.That(result.ViewData["Warehouses"], Is.InstanceOf(typeof(SelectList)));

        }
        [Test]
        public void ConfigureDocument_2_1()
        {
            string userId = "1";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = controller.ConfigureDocument("", userId, 0, 0) as PartialViewResult;

            var model = (List<Doc_ConfigureDocumentViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(PartialViewResult)));
            Assert.IsTrue(result.ViewName == "_Doc_AssignmentConfDocPartial");
            Assert.IsTrue(result.ViewData["InfoMessage"].ToString() == "");
            Assert.IsTrue(model[0].Code == "C-64");

        }
        [Test]
        public void ConfigureDocument_2_2()
        {
            string userId = "wrongID";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = controller.ConfigureDocument("", userId, 0, 0) as PartialViewResult;

            Assert.That(result, Is.InstanceOf(typeof(PartialViewResult)));
            Assert.IsTrue(result.ViewName == "_Doc_AssignmentConfDocPartial");
            Assert.IsTrue(result.ViewData["InfoMessage"].ToString() == "Użytkownik nie ma przypisanych przedmiotów");



        }
        [Test]
        public void ConfigureDocument_2_3()
        {
            string userId = "wrongID";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = controller.ConfigureDocument("", userId, 0, 1) as PartialViewResult;

            Assert.That(result, Is.InstanceOf(typeof(PartialViewResult)));
            Assert.IsTrue(result.ViewName == "_Doc_AssignmentConfDocPartial");
            Assert.IsTrue(result.ViewData["InfoMessage"].ToString() == "W wybranym magazynie nie ma przedmiotów!");



        }
        [Test]
        public void ConfigureDocument_2_4()
        {
            string externalId = "2";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = controller.ConfigureDocument("", externalId, 0, 2) as PartialViewResult;
            var model = (List<Doc_ConfigureDocumentViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(PartialViewResult)));
            Assert.IsTrue(result.ViewName == "_Doc_AssignmentConfDocPartial");
            Assert.IsTrue(result.ViewData["InfoMessage"].ToString() == "");
            Assert.IsTrue(model[0].Code == "A-S");



        }
        [Test]
        public void GenerateDocument_1_1()
        {
            List<Doc_ConfigureDocumentViewModel> viewModel = new List<Doc_ConfigureDocumentViewModel>()
            {
                new Doc_ConfigureDocumentViewModel()
                {
                    From = "1", To = "1", FromIndex = 0, ToIndex = 1, AddToDocument = true, Id = 1
                }
            };

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = controller.GenerateDocument(viewModel) as ViewResult;
            var model = (Doc_Assignment)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData["DocumentTitle"].ToString() == "ZW - Zwrot wewnętrzny");
            Assert.IsTrue(model.DocumentId.Contains("ZW") && model.Items[0].ItemCode == "C-64");

        }
        [Test]
        public async Task SaveDocument_1_1()
        {
            Doc_Assignment doc = new Doc_Assignment
            {
                DocumentId = "P/2021/1/01/00001/11111112",
                Title = "P",
                CreationDate = new DateTime(2021, 1, 1),
                UserFrom = "1",
                UserTo = "1"
            };

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.SaveDocument(doc) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "GlobalExceptionHandler");
            Assert.IsTrue(result.ViewData["ExceptionTitle"].ToString() == "Dodano dokument do bazy!");
            Assert.IsTrue(result.ViewData["ExceptionMessage"].ToString() == "P/2021/1/01/00001/11111112");



        }
        [Test]
        public async Task SaveDocument_1_2()
        {
            Doc_Assignment doc = new Doc_Assignment
            {
                DocumentId = "P/2021/1/01/00001/11111112",
                Title = "P",
                CreationDate = new DateTime(2021, 1, 1),
                UserFrom = "1",
                UserTo = "1"
            };

            var controller = new Doc_AssignmentsController(_context, _logger.Object);
            controller.ModelState.AddModelError("", "Testowy błąd");

            var result = await controller.SaveDocument(doc) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task ShowDocument_1_1()
        {
            string encodedId = "P%2F2021%2F1%2F01%2F00001%2F11111111";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.ShowDocument(encodedId) as ViewResult;
            var model = (Doc_Assignment)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData["DocumentTitle"].ToString() == "P - Potwierdzenie");
            Assert.IsTrue(model.DocumentId == "P/2021/1/01/00001/11111111");


        }
        [Test]
        public async Task ShowDocument_1_2()
        {
            string encodedId = "";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.ShowDocument(encodedId) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task ShowDocument_1_3()
        {
            string encodedId = "P%2F2021%2F1%2F01%2F00001%2FInvalid";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.ShowDocument(encodedId) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task DeleteDocument_1_1()
        {
            string encodedId = "P%2F2021%2F1%2F01%2F00001%2FToDelete";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.DeleteDocument(encodedId) as ViewResult;
            var model = (Doc_Assignment)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.DocumentId == "P/2021/1/01/00001/ToDelete");


        }
        [Test]
        public async Task DeleteDocument_1_2()
        {
            string encodedId = "";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.DeleteDocument(encodedId) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task DeleteDocument_1_3()
        {
            string encodedId = "P%2F2021%2F1%2F01%2F00001%2F11111112";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.DeleteDocument(encodedId) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task DeleteDocument_2_1()
        {
            string encodedId = "P%2F2021%2F1%2F01%2F00001%2FToDelete";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.DeleteDocumentConfirm(encodedId) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == nameof(Index));


        }
        [Test]
        public async Task DeleteDocument_2_2()
        {
            string encodedId = "";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.DeleteDocumentConfirm(encodedId) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");
        }
        [Test]
        public async Task DeleteDocument_2_3()
        {
            string encodedId = "P%2F2021%2F1%2F01%2F00001%2F11111112";

            var controller = new Doc_AssignmentsController(_context, _logger.Object);

            var result = await controller.DeleteDocumentConfirm(encodedId) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == nameof(Index));

        }




    }
}
