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
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Linq;
using System.Security.Principal;
using System.Security.Claims;

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
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", Street = "Śliczna", ZipCode = "30-000", City = "Wie300", EmployeeNumber = "11111111111", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var isModelValid = TryValidate(model, out _);
            Assert.IsTrue(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfFilledWithRegexException_ReturnCorrectErrorMessage()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", City = "Wie300", EmployeeNumber = "1111111111a", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var isModelValid = TryValidate(model, out ICollection<ValidationResult> results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.AreEqual(arr[0].ErrorMessage, "Nieprawidłowy identyfikator!");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfFilledWithLengthException_ReturnCorrectErrorMessages()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", Street = "Śliczna", ZipCode = "30-000", City = "Wie300", EmployeeNumber = "11111111110000", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            ICollection<ValidationResult> results;
            var isModelValid = TryValidate(model, out results);
            ValidationResult[] arr = new ValidationResult[5];
            results.CopyTo(arr, 0);
            Assert.IsTrue(arr[0].ErrorMessage == "The field Identyfikator must be a string with a maximum length of 11." && arr[1].ErrorMessage == "Nieprawidłowy identyfikator!");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public void RegisterViewModel_CheckIsModelValidIfFilledWithDifferentPassword_ReturnCorrectErrorMessages()
        {
            var model = new RegisterViewModel() { Name = "Jessica", Surname = "Testowa", Street = "Śliczna", ZipCode = "30-000", City = "Wie300", EmployeeNumber = "11111111111", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test1234!" };
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

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);
            var result = controller.Register() as ViewResult;
            Assert.IsNotNull(result);
            Assert.IsNull(result.Model);


        }
        [Test]
        public async Task RegisterPost_CheckCorrectTypeIsReturned_ReturnRedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .Build();
            
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("https");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:44387"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Account"));
            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);
            var controllerContext = new ControllerContext() { HttpContext = httpContext, ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() };


            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context) { ControllerContext = controllerContext };
            controller.Url = new Mock<IUrlHelper>().Object;

            RegisterViewModel rvm = new RegisterViewModel() { Name = "Grażyna", Surname = "Testowa", EmployeeNumber = "123456789012", City = "Kraków", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var result = await controller.Register(rvm) as ViewResult;
            string registrationSuccessfulInfo = result.ViewData.Values.ToArray()[0].ToString();
            Assert.IsTrue(registrationSuccessfulInfo == "Rejestracja udana!");


        }
        [Test]
        public async Task RegisterPost_IfDataAreValid_RegisterUserAndRedirectToIndex()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.Users)
                .Returns(_context.Users))
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(x => x.Setup(um => um.GenerateEmailConfirmationTokenAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync("RandomString"))
                .With(x => x.Setup(um => um.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var fakeSignInManager = new FakeSignInManagerBuilder()
                .With(x => x.Setup(sim => sim.IsSignedIn(It.IsAny<ClaimsPrincipal>()))
                .Returns(true))
                .Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("https");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:44387"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Account"));
            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns("TESTOJAN9012");
            httpContext.Setup(h => h.Request).Returns(request.Object);
            httpContext.Setup(h => h.User.IsInRole("Admin")).Returns(true);
            var controllerContext = new ControllerContext() { HttpContext = httpContext.Object, ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() };


            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context) { ControllerContext = controllerContext };
            controller.Url = new Mock<IUrlHelper>().Object;

            RegisterViewModel rvm = new RegisterViewModel() { Name = "Grażyna", Surname = "Testowa", EmployeeNumber = "123456789012", City = "Kraków", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var result = await controller.Register(rvm) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Index" && result.ControllerName == "Employees");


        }
        [Test]
        public async Task RegisterPost_IfDataAreValidButUsernameIsInUser_ReturnSameViewWithError()
        {
            IdentityError error = new CustomIdentityErrorDescriber().DuplicateUserName("TestoGra9012");

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.Users)
                .Returns(_context.Users))
                .With(x => x.Setup(um => um.CreateAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(error)))
                .Build();

            var fakeSignInManager = new FakeSignInManagerBuilder()
                .Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();
            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("https");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:44387"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Account"));

            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);
            var controllerContext = new ControllerContext() { HttpContext = httpContext, ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() };


            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context) { ControllerContext = controllerContext };
            controller.Url = new Mock<IUrlHelper>().Object;

            RegisterViewModel rvm = new RegisterViewModel() { Name = "Grażyna", Surname = "Testowa", EmployeeNumber = "123456789012", Address = "Św. Filipa 17, 31-150 Kraków, Polska", City = "Kraków", Email = "b@b.pl", Password = "Test123!", ConfirmPassword = "Test123!" };
            var result = await controller.Register(rvm) as ViewResult;
            var model = (RegisterViewModel)result.ViewData.Model;

            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == $"Nazwa użytkownika 'TestoGra9012' jest już zajęta!");
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(model.EmployeeNumber == "123456789012");


        }
        [Test]
        public async Task ConfirmEmail_IfUserIdAndTokenAreNull_ReturnRedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ConfirmEmail(null, null) as RedirectToActionResult;
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));


        }
        [Test]
        public async Task ConfirmEmail_IfTokenAndUserIdAreInvalid_ReturnNotFoundResult()
        {
            string invalidID = "99";
            string invalidToken = "invalidToken";

            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ConfirmEmail(invalidID, invalidToken) as ViewResult;
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");
            Assert.IsTrue(result.ViewData["ErrorMessage"].ToString() == $"Identyfikator użytkownika {invalidID} jest nieprawidłowy");


        }
        [Test]
        public async Task ConfirmEmail_IfTokenIsInvalid_ReturnException()
        {
            string validID = "1";
            string invalidToken = "invalidToken";

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByIdAsync(validID))
                .ReturnsAsync(_context.Users.Find(validID)))
                .With(x => x.Setup(um => um.ConfirmEmailAsync(It.IsAny<WMSIdentityUser>(), invalidToken))
                .ReturnsAsync(IdentityResult.Failed()))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ConfirmEmail(validID, invalidToken) as ViewResult;
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "GlobalExceptionHandler");
            Assert.IsTrue(result.ViewData["ExceptionTitle"].ToString() == "Nie można potwierdzić adresu email!");


        }
        [Test]
        public async Task ConfirmEmail_IfAllParametersAreValid_ReturnConfirmationViewResult()
        {
            string validID = "1";
            string validToken = "validToken";

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByIdAsync(validID))
                .ReturnsAsync(_context.Users.Find(validID)))
                .With(x => x.Setup(um => um.ConfirmEmailAsync(It.IsAny<WMSIdentityUser>(), validToken))
                .ReturnsAsync(IdentityResult.Success))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ConfirmEmail(validID, validToken) as ViewResult;
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public void ForgotPassword_Get_IfCalled_ReturnViewResult()
        {

            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = controller.ForgotPassword() as ViewResult;
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }
        [Test]
        public async Task ForgotPassword_Post_IfCorrectEmailAndUser_SendResetPasswordLinkAndReturnConfirmationViewResult()
        {
            var user = _context.Users.Find("1");

            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Email = "a@a.pl"
            };
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByEmailAsync(model.Email))
                .ReturnsAsync(user))
                .With(x => x.Setup(um => um.IsEmailConfirmedAsync(user))
                .ReturnsAsync(true))
                .With(x => x.Setup(um => um.GeneratePasswordResetTokenAsync(user))
                .ReturnsAsync("token"))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var request = new Mock<HttpRequest>();
            request.Setup(x => x.Scheme).Returns("https");
            request.Setup(x => x.Host).Returns(HostString.FromUriComponent("https://localhost:44387"));
            request.Setup(x => x.PathBase).Returns(PathString.FromUriComponent("/Account"));

            var httpContext = Mock.Of<HttpContext>(_ => _.Request == request.Object);
            var controllerContext = new ControllerContext() { HttpContext = httpContext, ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor() };

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context) { ControllerContext = controllerContext };
            controller.Url = new Mock<IUrlHelper>().Object;

            var result = await controller.ForgotPassword(model) as ViewResult;
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ForgotPasswordConfirmation");


        }
        [Test]
        public async Task ForgotPassword_Post_IfUserAndEmailAreIncorrect_ReturnConfirmationViewResult()
        {
            var user = _context.Users.Find("invalid");

            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Email = "invalid@invalid.pl"
            };
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByEmailAsync(model.Email))
                .ReturnsAsync(user))
                .With(x => x.Setup(um => um.IsEmailConfirmedAsync(user))
                .ReturnsAsync(true))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ForgotPassword(model) as ViewResult;
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ForgotPasswordConfirmation");


        }
        [Test]
        public void ForgotPassword_Model_InvalidEmail_ReturnValidationError()
        {

            ForgotPasswordViewModel model = new ForgotPasswordViewModel()
            {
                Email = "a"
            };

            var isModelValid = TryValidate(model, out ICollection<ValidationResult> results);

            Assert.IsFalse(isModelValid);

        }
        [Test]
        public async Task ForgotPassword_Post_IfModelIsNotValid_ReturnSameView()
        {

            ForgotPasswordViewModel vm = new ForgotPasswordViewModel()
            {
                Email = "a"
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);
            controller.ModelState.AddModelError("", "testerror");

            var result = await controller.ForgotPassword(vm) as ViewResult;
            var model = (ForgotPasswordViewModel)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.AreEqual(model, vm);

        }
        [Test]
        public void ResetPassword_Get_IfEmailAndTokenAreNull_ReturnSameViewWithErrorMessage()
        {
            string token = null;
            string email = null;

            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = controller.ResetPassword(token, email) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Nieprawidłowy token!");

        }
        [Test]
        public void ResetPassword_IfEmailAndTokenIsNotNull_ReturnViewResult()
        {
            string token = "token";
            string email = "email";

            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = controller.ResetPassword(token, email) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));

        }
        [Test]
        public async Task ResetPassword_Post_IfUserIsNotNullAndModelIsValid_ReturnConfirmationViewResult()
        {
            var user = _context.Users.Find("1");

            ResetPasswordViewModel model = new ResetPasswordViewModel()
            {
                Email = "a@a.pl",
                Password = "password",
                ConfirmPassword = "password",
                Token = "token"
            };

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByEmailAsync(model.Email))
                .ReturnsAsync(user))
                .With(x => x.Setup(um => um.ResetPasswordAsync(user, model.Token, model.Password))
                .ReturnsAsync(IdentityResult.Success))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ResetPassword(model) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ResetPasswordConfirmation");

        }
        [Test]
        public async Task ResetPassword_Post_IfPasswordsMismatch_ReturnModelError()
        {
            var user = _context.Users.Find("1");

            IdentityError error = new CustomIdentityErrorDescriber().PasswordMismatch();

            ResetPasswordViewModel vm = new ResetPasswordViewModel()
            {
                Email = "a@a.pl",
                Password = "password",
                ConfirmPassword = "pass",
                Token = "token"
            };

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByEmailAsync(vm.Email))
                .ReturnsAsync(user))
                .With(x => x.Setup(um => um.ResetPasswordAsync(user, vm.Token, vm.Password))
                .ReturnsAsync(IdentityResult.Failed(error)))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ResetPassword(vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Nieprawidłowe hasło!");

        }
        [Test]
        public async Task ResetPassword_Post_IfUserIsNullAndModelIsValid_ReturnConfirmationView()
        {
            var user = _context.Users.Find("invalid");

            ResetPasswordViewModel vm = new ResetPasswordViewModel()
            {
                Email = "a@a.pl",
                Password = "password",
                ConfirmPassword = "password",
                Token = "token"
            };

            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByEmailAsync(vm.Email))
                .ReturnsAsync(user))
                .Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.ResetPassword(vm) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "ResetPasswordConfirmation");

        }
        [Test]
        public async Task ResetPassword_Post_IfModelIsNotValid_ReturnSameView()
        {

            ResetPasswordViewModel vm = new ResetPasswordViewModel()
            {
                Email = "a@a.pl",
                Password = "password",
                ConfirmPassword = "pass",
                Token = "token"
            };

            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);
            controller.ModelState.AddModelError("", "testerror");

            var result = await controller.ResetPassword(vm) as ViewResult;
            var model = (ResetPasswordViewModel)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.AreEqual(model, vm);

        }

        [Test]
        public void LoginGet_CheckCorrectTypeIsReturned_ReturnRedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder().Build();
            var fakeSignInManager = new FakeSignInManagerBuilder().Build();
            var fakeRoleManager = new FakeRoleManagerBuilder().Build();
            var fakeLogger = new Mock<ILogger<AccountController>>();

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = controller.Login();
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));


        }

        [Test]
        public async Task LoginPost_UrlIsNotNullOrEmpty_ReturnRedirectResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(new WMSIdentityUser() { Name = "Janusz", Surname = "Testowy", UserName = "TestoJan9012", EmployeeNumber = "23456789012", City = "Kraków", Email = "a@a.pl", LoginCount = 0 }))
                .Build();
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
            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context)
            {
                Url = mockUrlHelper.Object
            };
            var result = await controller.Login(new LoginViewModel(), "testPath");
            Assert.That(result, Is.InstanceOf(typeof(RedirectResult)));


        }
        [Test]
        public async Task LoginPost_UrlIsEmpty_RedirectToActionResult()
        {
            var fakeUserManager = new FakeUserManagerBuilder()
                .With(x => x.Setup(um => um.FindByNameAsync(It.IsAny<string>())).ReturnsAsync( new WMSIdentityUser() { Name = "Janusz", Surname = "Testowy", UserName = "TestoJan9012", EmployeeNumber = "23456789012", City = "Kraków", Email = "a@a.pl", LoginCount = 0 }))
                .Build();
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
            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context)
            {
                Url = mockUrlHelper.Object
            };
            var result = await controller.Login(new LoginViewModel() { Login = "TestoJan9012" }, "");
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
            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

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

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

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

            var controller = new AccountController(fakeUserManager.Object, fakeSignInManager.Object, fakeRoleManager.Object, fakeLogger.Object, _context);

            var result = await controller.Logout();
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));


        }
    }
}
