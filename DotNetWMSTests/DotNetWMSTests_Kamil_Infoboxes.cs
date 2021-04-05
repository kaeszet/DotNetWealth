using DotNetWMS.Controllers;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    class DotNetWMSTests_Kamil_Infoboxes : DotNetWMSTests_Base
    {
        private Mock<ILogger<InfoboxesController>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<InfoboxesController>>();
        }
        [Test]
        public void Model_1()
        {
            var info = new Infobox
            {
                Id = 1,
                Title = "Test",
                Message = "Test"
            };

            var isModelValid = TryValidate(info, out ICollection<ValidationResult> results);

            Assert.IsTrue(isModelValid);

        }
        [Test]
        public void Model_2()
        {
            var info = new Infobox
            {
                Id = 1,
                Title = "Test1",
                Message = "Test1"
            };

            var isModelValid = TryValidate(info, out ICollection<ValidationResult> results);
            List<ValidationResult> errors = (List<ValidationResult>)results;

            Assert.IsTrue(errors.Count == 2);
            Assert.AreEqual(errors[0].ErrorMessage, "Tytuł nie może zawierać cyfr i znaków specjalnych");
            Assert.AreEqual(errors[1].ErrorMessage, "Wiadomość nie może zawierać cyfr i znaków specjalnych");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public async Task Index_1()
        {
            string normalizedUserName = "TESTOJAN9012";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns(normalizedUserName);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new InfoboxesController(_context, _logger.Object)
            {
                ControllerContext = context
            };

            var result = await controller.Index(true) as ViewResult;
            var model = (List<Infobox>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue((bool)result.ViewData["IsChecked"] == false);
            Assert.IsTrue(model[0].User.NormalizedUserName == normalizedUserName);

        }
        [Test]
        public async Task Delete_1()
        {

            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Delete(2) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));

        }
        [Test]
        public async Task Delete_2()
        {

            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Delete(null) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");
        }
        [Test]
        public async Task Delete_3()
        {

            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Delete(99) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");

        }
        [Test]
        public async Task DeleteAllChecked_1()
        {
            int infosCountBefore = _context.Infoboxes.Local.Count;
            string normalizedUserName = "TESTOJAN";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns(normalizedUserName);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new InfoboxesController(_context, _logger.Object)
            {
                ControllerContext = context
            };

            var result = await controller.DeleteAllChecked() as RedirectToActionResult;
            int infosCountAfter = _context.Infoboxes.Local.Count;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.AreEqual(infosCountBefore, infosCountAfter);


        }
        [Test]
        public async Task DeleteAllChecked_2()
        {
            int infosCountBefore = _context.Infoboxes.Local.Count;
            string normalizedUserName = "TESTOJAN9012";
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns(normalizedUserName);

            var context = new ControllerContext(new ActionContext(httpContext.Object, new RouteData(), new ControllerActionDescriptor()));

            var controller = new InfoboxesController(_context, _logger.Object)
            {
                ControllerContext = context
            };

            var result = await controller.DeleteAllChecked() as RedirectToActionResult;
            int infosCountAfter = _context.Infoboxes.Local.Count;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.AreNotEqual(infosCountBefore, infosCountAfter);


        }
        [Test]
        public async Task Check_1()
        {
            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Check(null) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");


        }
        [Test]
        public async Task Check_2()
        {
            int? notExistedId = 99;

            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Check(notExistedId) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");
            Assert.IsTrue(result.ViewData["ErrorMessage"].ToString() == $"Nie odnaleziono wiadomości o podanym ID = {notExistedId}");


        }
        [Test]
        public async Task Check_3()
        {
            int? Id = 5;

            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Check(Id) as RedirectToActionResult;
            var isChecked = _context.Infoboxes.Find(Id).IsChecked;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");
            Assert.IsFalse(isChecked);

        }
        [Test]
        public async Task Check_4()
        {
            int? Id = 6;

            var controller = new InfoboxesController(_context, _logger.Object);

            var result = await controller.Check(Id) as RedirectToActionResult;

            var isConfirmed = _context.Doc_Assignments.Find("P/2021/1/01/00001/ToCheck").IsConfirmed;
            var isChecked = _context.Infoboxes.Find(Id).IsChecked;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");
            Assert.IsTrue(isConfirmed);
            Assert.IsTrue(isChecked);



        }


    }
}
