using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    class DotNetWMSTests_Kamil_Locations : DotNetWMSTests_Base
    {
        private Mock<ILogger<LocationsController>> _logger;

        [SetUp]
        public void Setup()
        {
            _logger = new Mock<ILogger<LocationsController>>();
        }

        [Test]
        public void Index_IfActionCalled_ReturnViewResult()
        {

            var controller = new LocationsController(_context, _logger.Object);

            var result = controller.Index("", "") as ViewResult;
            var model = (List<LocationListViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Count > 0);
            Assert.IsTrue(model[0].Id == 1);


        }
        [Test]
        public void Index_IfActionCalled_ReturnViewResultWithList()
        {

            var controller = new LocationsController(_context, _logger.Object);

            var result = controller.Index("", "") as ViewResult;
            var model = (List<LocationListViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Count > 0);
            Assert.IsTrue(model[2].IsInUse);
            Assert.IsTrue(model[2].Records.Count == 4);


        }
        [Test]
        public void Index_IfActionCalledWithSearchArgument_CheckRecordAddress()
        {

            var controller = new LocationsController(_context, _logger.Object);

            var result = controller.Index("", "Myśliwska 61") as ViewResult;
            var model = (List<LocationListViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Count > 0);
            Assert.IsTrue(model[0].Address == "Myśliwska 61, 30-718 Kraków, Polska");


        }
        [Test]
        public void Index_IfActionCalledWithOrderArgument_CheckRecordAddress()
        {

            var controller = new LocationsController(_context, _logger.Object);

            var result = controller.Index("address_desc", "") as ViewResult;
            var model = (List<LocationListViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Count > 0);
            Assert.IsTrue(model[0].Address == "Św. Filipa 17, 31-150 Kraków, Polska");


        }
        [Test]
        public async Task ShowMap_IfIdIsValid_ReturnViewResult()
        {
            int? id = 1;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.ShowMap(id) as ViewResult;
            var model = (Location)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Address == "Lipska 49, 30-716 Kraków, Polska");


        }
        [Test]
        public async Task ShowMap_IfIdIsNotValid_ReturnNotFound()
        {
            int? invalidID = 99;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.ShowMap(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task ShowMap_IfIdIsNull_ReturnNotFound()
        {
            int? invalidID = null;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.ShowMap(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Edit_Get_IfIdIsValid_ReturnViewResult()
        {
            int? id = 1;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Edit(id) as ViewResult;
            var model = (Location)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Address == "Lipska 49, 30-716 Kraków, Polska");


        }
        [Test]
        public async Task Edit_Get_IfIdIsNotValid_ReturnNotFound()
        {
            int? invalidID = 99;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Edit(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Edit_Get_IfIdIsNull_ReturnNotFound()
        {
            int? invalidID = null;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Edit(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Edit_Post_IfIdExistsAndAddressIsValid_RedirectToActionResult()
        {
            int id = 1;
            var locationBefore = _context.Locations.Find(id);
            locationBefore.Address = "Lipska 49, 30-721 Kraków, Polska";

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Edit(id, locationBefore) as RedirectToActionResult;

            var locationAfter = _context.Locations.Find(id);

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.AreEqual(locationBefore, locationAfter);

            //reset data

            locationAfter.Address = "Lipska 49, 30-716 Kraków, Polska";
            
        }
        [Test]
        public async Task Edit_Post_IfIdExistsAndAddressIsValidButIsInUse_ReturnSameViewWithError()
        {
            int id = 2;
            var locationBefore = _context.Locations.Find(id);
            locationBefore.Address = "Myśliwska 61, 30-718 Kraków, Polska";

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Edit(id, locationBefore) as ViewResult;
            var model = (Location)result.ViewData.Model;

            var locationAfter = _context.Locations.Find(id);
            

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.AreEqual(result.ViewData.ModelState[""].Errors[0].ErrorMessage, $"Lokalizacja \"{locationBefore.Address}\" jest już w systemie!");
            Assert.IsTrue(model.Address == locationAfter.Address);

            //reset data

            locationAfter.Address = "Św. Filipa 17, 31-150 Kraków, Polska";


        }
        [Test]
        public async Task Edit_Post_IfIdAndLocationIdAreDifferent_ReturnNotFound()
        {
            int id = 1;
            var location = _context.Locations.Find(2);

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Edit(id, location) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));


        }
        [Test]
        public async Task Delete_Get_IfIdIsValid_ReturnViewResult()
        {
            int? id = 2;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Delete(id) as ViewResult;
            var model = (Location)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.Address == "Św. Filipa 17, 31-150 Kraków, Polska");

        }
        [Test]
        public async Task Delete_Get_IfIdIsNotValid_ReturnNotFound()
        {
            int? invalidID = 99;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Delete(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task Delete_Get_IfIdIsNull_ReturnNotFound()
        {
            int? invalidID = null;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.Delete(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
        [Test]
        public async Task Delete_Post_IfIdIsValid_DeleteLocation()
        {
            int id = 1;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.DeleteConfirmed(id) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index");

            //reset data

            var loc = new Location { Id = 1, Address = "Lipska 49, 30-716 Kraków, Polska", Latitude = "50.0405946", Longitude = "19.9997631" };

            _context.Locations.Add(loc);
            await _context.SaveChangesAsync();

            Assert.IsFalse(_context.Locations.Find(1) == null);

        }
        [Test]
        public async Task Delete_Post_IfIdIsNotValid_ReturnNotFound()
        {
            int invalidID = 99;

            var controller = new LocationsController(_context, _logger.Object);

            var result = await controller.DeleteConfirmed(invalidID) as NotFoundResult;

            Assert.That(result, Is.InstanceOf(typeof(NotFoundResult)));

        }
    }
}
