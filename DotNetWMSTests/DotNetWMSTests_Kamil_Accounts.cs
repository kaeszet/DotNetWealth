using DotNetWMS.Controllers;
using DotNetWMS.Data;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;
using Microsoft.Extensions.Logging;

namespace DotNetWMSTests
{
    public class DotNetWMSTests_Kamil_Accounts : DotNetWMSTests_Identity_Base
    {
        [SetUp]
        public void Setup()
        {

        }
        [Test]
        public void LoginViewModel_CheckIsModelValidIfRequiredFieldAreNotFilled_ReturnFalse()
        {
            var model = new LoginViewModel();
            var isModelValid = TryValidate(model, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void LoginViewModel_CheckIsModelValidIfNotAllRequiredFieldAreFilled_ReturnFalse()
        {
            var model = new LoginViewModel() { Login = "TestoJan9012" };
            var isModelValid = TryValidate(model, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void LoginViewModel_CheckIsModelValidIfRequiredFieldAreFilled_ReturnTrue()
        {
            var model = new LoginViewModel() { Login = "TestoJan9012" , Password = "Test123!"};
            var isModelValid = TryValidate(model, out _);
            Assert.IsTrue(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfRequiredFieldAreNotFilled_ReturnFalse()
        {
            var model = new RegisterViewModel();
            var isModelValid = TryValidate(model, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfNotAllRequiredFieldAreFilled_ReturnFalse()
        {
            var model = new RegisterViewModel() { Name = "Janusz", Surname = "Testowy" };
            var isModelValid = TryValidate(model, out _);
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfRequiredFieldAreFilled_ReturnTrue()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", City = "Wie300", EmployeeNumber = "111111111111", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var isModelValid = TryValidate(model, out _);
            Assert.IsTrue(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfFilledWithRegexException_ReturnCorrectErrorMessage()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", City = "Wie300", EmployeeNumber = "1111111111a", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            ICollection<ValidationResult> results;
            var isModelValid = TryValidate(model, out results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.AreEqual(arr[0].ErrorMessage, "Nieprawidłowy identyfikator!");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfFilledWithLengthException_ReturnCorrectErrorMessages()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", City = "Wie300", EmployeeNumber = "11111111110000", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            ICollection<ValidationResult> results;
            var isModelValid = TryValidate(model, out results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.IsTrue(arr[0].ErrorMessage == "The field Identyfikator must be a string with a maximum length of 12." && arr[1].ErrorMessage == "Nieprawidłowy identyfikator!");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfFilledWithDifferentPassword_ReturnCorrectErrorMessages()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", City = "Wie300", EmployeeNumber = "111111111111", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test1234!" };
            ICollection<ValidationResult> results;
            var isModelValid = TryValidate(model, out results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.AreEqual(arr[0].ErrorMessage, "Hasła nie pasują do siebie");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterGet_GetRegisterView_ReturnViewObjectWithoutInjectedData()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);
            var result = controller.Register() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);


        }
        [Test]
        public async Task RegisterPost_CheckCorrectTypeIsReturned_ReturnRedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success)).Build();
            
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();
            

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            RegisterViewModel rvm = new RegisterViewModel() { Name = "Grażyna", Surname = "Testowa", EmployeeNumber = "123456789012", City = "Kraków", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var result = await controller.Register(rvm);
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));


        }
        [Test]
        public void LoginGet_CheckCorrectTypeIsReturned_ReturnRedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            RegisterViewModel rvm = new RegisterViewModel() { Name = "Grażyna", Surname = "Testowa", EmployeeNumber = "123456789012", City = "Kraków", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var result = controller.Login();
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }

        [Test]
        public async Task LoginPost_UrlIsNotNullOrEmpty_ReturnRedirectResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder()
                .With(x => x.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success))
                .Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            controller.Url = mockUrlHelper.Object;
            var result = await controller.Login(new LoginViewModel(), "testPath");
            Assert.That(result, Is.InstanceOf(typeof(RedirectResult)));


        }
        [Test]
        public async Task LoginPost_UrlIsEmpty_RedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder()
                .With(x => x.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success))
                .Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            controller.Url = mockUrlHelper.Object;
            var result = await controller.Login(new LoginViewModel(), "");
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));


        }
        [Test]
        public async Task LoginPost_ModelIsInvalid_RedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder()
                .With(x => x.Setup(sm => sm.PasswordSignInAsync(It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<bool>(),
                        It.IsAny<bool>()))
                .ReturnsAsync(SignInResult.Success))
                .Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var mockUrlHelper = new Mock<IUrlHelper>(MockBehavior.Strict);
            mockUrlHelper
                .Setup(x => x.IsLocalUrl(It.IsAny<string>()))
                .Returns(true)
                .Verifiable();
            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            controller.Url = mockUrlHelper.Object;
            controller.ModelState.AddModelError("", "Nieprawidłowy login lub hasło");
            var result = await controller.Login(new LoginViewModel(), "testPath");
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public async Task IsEmailInUse_CheckCorrectTypeIsReturned_ReturnRedirectResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().With(x => x.Setup(um => um.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync(new WMSIdentityUser())).Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            var result = await controller.IsEmailInUse("a@a.pl");
            Assert.That(result, Is.InstanceOf(typeof(IActionResult)));


        }
        [Test]
        public async Task LogOut_CheckCorrectTypeIsReturned_ReturnRedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object);

            var result = await controller.Logout();
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));


        }
    }
}
