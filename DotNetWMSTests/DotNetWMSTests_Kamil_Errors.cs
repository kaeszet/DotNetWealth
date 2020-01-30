using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DotNetWMSTests
{
    class DotNetWMSTests_Kamil_Errors : DotNetWMSTests_Base
    {
        private Mock<IStatusCodeReExecuteFeature> _status;

        [SetUp]
        public void Setup()
        {
            _status = new Mock<IStatusCodeReExecuteFeature>();
            _status.SetupGet(s => s.OriginalPath).Returns("invalid");
            _status.SetupGet(s => s.OriginalPathBase).Returns("");

        }
        [Test]
        public void E_test_1()
        {

            var controller = new ErrorController();
            controller.ControllerContext = new ControllerContext();
            FeatureCollection featureCollection = new FeatureCollection();
            featureCollection.Set(_status.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection);
            
            var act = controller.HttpStatusCodeHandler(404) as ViewResult;
            Assert.IsTrue(act.ViewName == "NotFound");
           


        }
    }
}
