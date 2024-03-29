﻿using DotNetWMS.Controllers;
using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DotNetWMSTests
{
	public class DotNetWMSTests_Krzysztof_Error : DotNetWMSTests_Base
	{
		private Mock<IStatusCodeReExecuteFeature> _status;
		private Mock<IExceptionHandlerPathFeature> _statusGlobal;
		private Mock<ILogger<ErrorController>> _logger;


		[SetUp]
		public void Setup()
		{
			_status = new Mock<IStatusCodeReExecuteFeature>();
			_status.SetupGet(s => s.OriginalPath).Returns("invalid");
			_status.SetupGet(s => s.OriginalPathBase).Returns("");

			_statusGlobal = new Mock<IExceptionHandlerPathFeature>();
			_statusGlobal.SetupGet(a => a.Path).Returns("GlobalError");
			_statusGlobal.SetupGet(a => a.Error.Message).Returns("Test błędu globalnego");

			_logger = new Mock<ILogger<ErrorController>>();
		}
		[Test]
		public void HttpStatusCodeHandler_CheckIfGetNotFoundViews_ReturnTrue()
		{

			var controller = new ErrorController(_logger.Object);

			FeatureCollection featureCollection = new FeatureCollection();
			featureCollection.Set(_status.Object);
			controller.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection);

			var act = controller.HttpStatusCodeHandler(404) as ViewResult;

			Assert.IsTrue(act.ViewName == "NotFound");

		}
		[Test]
		public void HttpStatusCodeHandler_CheckIfOrginalPathIsTheSame_ReturnInvalid()
		{

			var controller = new ErrorController(_logger.Object);

			FeatureCollection featureCollection = new FeatureCollection();
			featureCollection.Set(_status.Object);
			controller.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection);

			var act = controller.HttpStatusCodeHandler(404) as ViewResult;

			Assert.AreEqual(act.ViewData["Path"], "invalid");

		}

		[Test]
		public void GlobalExceptionHandler_CheckIfPathNameIsTheSame_ReturnGlobalError()
		{
			var controller2 = new ErrorController(_logger.Object);

			FeatureCollection featureCollection2 = new FeatureCollection();
			featureCollection2.Set(_statusGlobal.Object);
			controller2.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection2);

			var a = controller2.GlobalExceptionHandler() as ViewResult;

			Assert.AreEqual(a.ViewData["ExceptionPath"], "GlobalError");
		}

		[Test]
		public void GlobalExceptionHandler_CheckIfErrorMessageNameIsTheSame_ReturnTrue()
		{
			var controller2 = new ErrorController(_logger.Object);

			FeatureCollection featureCollection2 = new FeatureCollection();
			featureCollection2.Set(_statusGlobal.Object);
			controller2.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection2);

			var a = controller2.GlobalExceptionHandler() as ViewResult;

			Assert.IsTrue((string)a.ViewData["ExceptionMessage"] == "Test błędu globalnego");
			Assert.IsNotNull(a);

		}
	}
}
