using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Moq;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;
using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;



namespace DotNetWMSTests
{
	public class DotNetWMSTests_Krzysztof_Error : DotNetWMSTests_Base
	{
		private Mock<IStatusCodeReExecuteFeature> _status;
		private Mock<IExceptionHandlerPathFeature> _statusGlobal;


		[SetUp]
		public void Setup()
		{
			_status = new Mock<IStatusCodeReExecuteFeature>();
			_status.SetupGet(s => s.OriginalPath).Returns("invalid");
			_status.SetupGet(s => s.OriginalPathBase).Returns("");

			_statusGlobal = new Mock<IExceptionHandlerPathFeature>();
			_statusGlobal.SetupGet(a => a.Path).Returns("GlobalError");
			_statusGlobal.SetupGet(a => a.Error.Message).Returns("Test błędu globalnego");
		}
		[Test]
		public void HttpStatusCodeHandler_CheckIfGetNotFoundViews_ReturnTrue()
		{

			var controller = new ErrorController();

			FeatureCollection featureCollection = new FeatureCollection();
			featureCollection.Set(_status.Object);
			controller.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection);

			var act = controller.HttpStatusCodeHandler(404) as ViewResult;

			Assert.IsTrue(act.ViewName == "NotFound");

		}
		[Test]
		public void HttpStatusCodeHandler_CheckIfOrginalPathIsTheSame_ReturnInvalid()
		{

			var controller = new ErrorController();

			FeatureCollection featureCollection = new FeatureCollection();
			featureCollection.Set(_status.Object);
			controller.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection);

			var act = controller.HttpStatusCodeHandler(404) as ViewResult;

			Assert.AreEqual(act.ViewData["Path"], "invalid");

		}

		[Test]
		public void GlobalExceptionHandler_CheckIfPathNameIsTheSame_ReturnGlobalError()
		{
			var controller2 = new ErrorController();

			FeatureCollection featureCollection2 = new FeatureCollection();
			featureCollection2.Set(_statusGlobal.Object);
			controller2.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection2);

			var a = controller2.GlobalExceptionHandler() as ViewResult;

			Assert.AreEqual(a.ViewData["ExceptionPath"], "GlobalError");
		}

		[Test]
		public void GlobalExceptionHandler_CheckIfErrorMessageNameIsTheSame_ReturnTrue()
		{
			var controller2 = new ErrorController();

			FeatureCollection featureCollection2 = new FeatureCollection();
			featureCollection2.Set(_statusGlobal.Object);
			controller2.ControllerContext.HttpContext = new DefaultHttpContext(featureCollection2);

			var a = controller2.GlobalExceptionHandler() as ViewResult;

			Assert.IsTrue((string)a.ViewData["ExceptionMessage"] == "Test błędu globalnego");
			Assert.IsNotNull(a);

		}
	}
}
