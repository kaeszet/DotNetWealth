using DotNetWMS.Controllers;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace DotNetWMSTests
{
	public class DotNetWMSTests_Krzysztof_Home : DotNetWMSTests_Base
	{

		protected readonly ILogger<HomeController> _logger;

		[SetUp]
		public void Setup()
		{

		}

		[Test]
		public void ShowIndexViews()
		{
			var index = new HomeController(_logger);
			var resultIndex = index.Index();

			Assert.IsInstanceOf(typeof(ViewResult), resultIndex);
		}
		[Test]
		public void ShowPrivacyViews()
		{
			var privacy = new HomeController(_logger);
			var resultPrivacy = privacy.AboutUs();

			Assert.IsInstanceOf(typeof(ViewResult), resultPrivacy);
		}

		[Test]
		public void ShowErrorViews()
		{
			var error = new HomeController(_logger);


			error.ControllerContext = new ControllerContext();
			error.ControllerContext.HttpContext = new DefaultHttpContext();

			var resultError =  error.Error();

			Assert.IsInstanceOf(typeof(ViewResult), resultError);
		}
	}
}
