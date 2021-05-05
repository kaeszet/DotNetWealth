using DotNetWMS.Controllers;
using DotNetWMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
    class DotNetWMSTests_Kamil_Administration : DotNetWMSTests_Identity_Base
    {
        private Mock<FakeUserManager> fakeUserManager;
        private Mock<FakeRoleManager> fakeRoleManager;
        private Mock<ILogger<AdministrationController>> logger;

        [SetUp]
        public void Setup()
        {
            fakeUserManager = new FakeUserManagerBuilder().Build();
            fakeRoleManager = new FakeRoleManagerBuilder().Build();
            logger = new Mock<ILogger<AdministrationController>>();
        }
        [Test]
        public void AccessDenied_IfMethodCalled_ReturnViewResult()
        { 
            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);
            var result = controller.AccessDenied();
            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));

        }
        [Test]
        public void ListOfRoles_IfMethodCalled_ReturnViewResult()
        {
            fakeRoleManager.Setup(x => x.Roles).Returns(_context.Roles);
            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);
            var result = controller.ListOfRoles() as ViewResult;
            Assert.IsNotNull(result.Model);

        }
        [Test]
        public void ListOfUsers_IfMethodCalled_ReturnViewResult()
        {
            fakeUserManager.Setup(x => x.Users).Returns(_context.Users);
            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);
            var result = controller.ListOfUsers() as ViewResult;
            Assert.IsNotNull(result.Model);

        }
        [Test]
        public async Task EditUsersInRole_Get_IfCorrectIdIsPassed_ReturnViewWithCorrectUser()
        {
            fakeRoleManager.Setup(frm => frm.FindByIdAsync("1")).ReturnsAsync(await _context.Roles.FindAsync("1"));
            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(true))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);
            var result = await controller.EditUsersInRole("1") as ViewResult;
            var user = (List<Admin_UsersInRoleViewModel>)result.ViewData.Model;
            var contextUser = _context.Users.Find("1");

            Assert.AreEqual(contextUser.FullName, user[0].FullName);

        }
        [Test]
        public async Task EditUsersInRole_Get_IfRoleIsIncorrect_ReturnNotFound()
        {
            string roleId = "fakeRoleID";

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);
            var result = await controller.EditUsersInRole(roleId) as ViewResult;

            Assert.AreEqual("NotFound", result.ViewName);

        }
        [Test]
        public async Task EditUsersInRole_Post_IfRoleIsIncorrectModelIsCorrect_ReturnSameViewWithErrorMessage()
        {
            string roleId = "fakeRoleID";

            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);
            var result = await controller.EditUsersInRole(model, roleId) as ViewResult;

            Assert.AreEqual("NotFound", result.ViewName);
            Assert.IsTrue(result.ViewData["ErrorMessage"].ToString() == $"Rola o numerze ID: {roleId} nie została odnaleziona!");

        }
        [Test]
        public async Task EditUsersInRole_Post_IfModelIsValidUserIsNotNullAndRoleIsNotNull_IdentityResultSuccess()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "1";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(false))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.RemoveFromRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_IfSelectDifferrentRoleAndOnlyOneUserIsInAdminRole_ReturnDbException()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "1";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };
            

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(false).ReturnsAsync(true).ReturnsAsync(false).ReturnsAsync(false))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as ViewResult;

            Assert.IsTrue(result.ViewName == "DbExceptionHandler" && result.ViewData.Values.Count == 2);

        }
        [Test]
        public async Task EditUsersInRole_Post_IfUncheckUserAndOnlyOneUserIsInAdminRole_ReturnDbException()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "Admin";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = false,
                UserName = "TestoJan9012"
            };


            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(true))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as ViewResult;

            Assert.IsTrue(result.ViewName == "DbExceptionHandler" 
                && result.ViewData["ErrorMessage"].ToString() == "Co najmniej jeden użytkownik musi mieć uprawnienia administratora.");

        }
        [Test]
        public async Task EditUsersInRole_Post_IfUserIsCheckedAndOtherUserWithAdminRoleExists_IdentityResultSuccess()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "1";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), roleId))
                .ReturnsAsync(false))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Admin"))
                .ReturnsAsync(true)
                .ReturnsAsync(true)
                .ReturnsAsync(true))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_AddStaffRole_IdentityResultSuccess()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "1";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), roleId))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Admin"))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Kadry"))
                .ReturnsAsync(true))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Standard"))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "StandardPlus"))
                .ReturnsAsync(false))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_WhenTryToAddStandardAndStuffRole_IdentityResultSuccessAndUserInStandardAndStaffRole()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "1";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Admin"))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Kadry"))
                .ReturnsAsync(true))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Standard"))
                .ReturnsAsync(false)
                .ReturnsAsync(true)
                .ReturnsAsync(true))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "StandardPlus"))
                .ReturnsAsync(false)
                .ReturnsAsync(false))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_EditUsersInRole_Post_WhenTryToAddStandardPlusAndStuffRole_IdentityResultSuccessAndUserInBothRoles()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "1";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Admin"))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Kadry"))
                .ReturnsAsync(true))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Standard"))
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false)
                .ReturnsAsync(false))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "StandardPlus"))
                .ReturnsAsync(true)
                .ReturnsAsync(true)
                .ReturnsAsync(true))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_WhenTryToAddKadryRoleToUserInStandardRole_IdentityResultSuccessAndUserIsInBothRoles()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "Kadry";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Admin"))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Kadry"))
                .ReturnsAsync(false))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Standard"))
                .ReturnsAsync(true))
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "StandardPlus"))
                .ReturnsAsync(false))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.AddToRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_WhenUserIsUnchecked_IdentityResultSuccessAndRemoveFromRole()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "Kadry";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = false,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Kadry"))
                .ReturnsAsync(true)
                .ReturnsAsync(true))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.RemoveFromRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsersInRole_Post_IfNothingChanged_ReturnView()
        {
            List<Admin_UsersInRoleViewModel> model = new List<Admin_UsersInRoleViewModel>();
            string userId = "1";
            string roleId = "Kadry";

            Admin_UsersInRoleViewModel vm = new Admin_UsersInRoleViewModel()
            {
                UserId = "1",
                FullName = "Testowy Janusz",
                EmployeeNumber = "23456789012",
                IsSelected = true,
                UserName = "TestoJan9012"
            };

            var user = _context.Users.Find(userId);

            model.Add(vm);

            fakeRoleManager.Setup(frm => frm.FindByIdAsync(roleId)).ReturnsAsync(await _context.Roles.FindAsync(roleId));

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(b => b.SetupSequence(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), "Kadry"))
                .ReturnsAsync(true)
                .ReturnsAsync(false))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUsersInRole(model, roleId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "EditRole" && result.RouteValues.Values.Contains(roleId));

        }
        [Test]
        public async Task EditUsers_Get_IfInvalidUserId_ReturnNotFound()
        {
            string userId = "fakeUserID";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUser(userId) as ViewResult;

            Assert.IsTrue(result.ViewName == "NotFound" && result.ViewData["ErrorMessage"].ToString() == $"Użytkownik o numerze ID: {userId} nie został odnaleziony!");

        }
        [Test]
        public async Task EditUsers_Get_IfValidUserId_ReturnViewResult()
        {
            List<string> userRoles = new List<string>()
            {
                "Admin"
            };

            string userId = "1";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.GetRolesAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(userRoles))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUser(userId) as ViewResult;
            var user = (Admin_EditUserViewModel)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(user.Roles == userRoles);

        }
        [Test]
        public async Task EditUsers_Post_IfInvalidUserId_ReturnNotFound()
        {
            string userId = "fakeUserID";
            string emailInvalidFormat = "invalidFormat";

            var model = new Admin_EditUserViewModel()
            {
                Id = "1",
                Name = "Janusz",
                Surname = "Testowy",
                City = "Kraków",
                EmployeeNumber = "23456789012",
                Roles = new List<string>() { "Admin" },
                Email = emailInvalidFormat
            };

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUser(model) as ViewResult;

            Assert.IsTrue(result.ViewName == "NotFound");
            Assert.IsTrue(result.ViewData["ErrorMessage"].ToString() == $"Użytkownik o numerze ID: {model.Id} nie został odnaleziony!");

        }
        [Test]
        public async Task EditUsers_Post_IfInvalidEmailFormat_ReturnSameViewWithErrorMessage()
        {
            string userId = "1";
            string emailInvalidFormat = "invalidFormat";

            var model = new Admin_EditUserViewModel()
            {
                Id = "1",
                Name = "Janusz",
                Surname = "Testowy",
                City = "Kraków",
                EmployeeNumber = "23456789012",
                Roles = new List<string>() { "Admin" },
                Email = emailInvalidFormat
            };

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUser(model) as ViewResult;

            Assert.IsFalse(result.ViewData.ModelState.IsValid);
            Assert.IsTrue(result.ViewData.ModelState["Email"].Errors[0].ErrorMessage == "Adres mailowy powinien być zapisany w formacie użytkownik@domena np. jankowalski@twojafirma.pl");

        }
        [Test]
        public void EditUsers_Post_IfInvalidContainsDigits_ReturnsSameViewWithErrorList()
        {
            string nameInvalidFormat = "nameContainsNum9";
            string surnameInvalidFormat = "surnameContainsNum9";
            string employeeNumberInvalidFormat = "777";

            var model = new Admin_EditUserViewModel()
            {
                Id = "1",
                Name = nameInvalidFormat,
                Surname = surnameInvalidFormat,
                City = "Kraków",
                EmployeeNumber = employeeNumberInvalidFormat,
                Roles = new List<string>() { "Admin" },
                Email = "a@a.pl"
            };

            var isModelValid = TryValidate(model, out ICollection<ValidationResult> results);
            List<ValidationResult> errors = (List<ValidationResult>)results;
            Assert.AreEqual(errors[0].ErrorMessage, "Imię nie może zawierać cyfr");
            Assert.AreEqual(errors[1].ErrorMessage, "Nazwisko nie może zawierać cyfr");
            Assert.AreEqual(errors[2].ErrorMessage, "Nieprawidłowy identyfikator!");
            Assert.IsFalse(isModelValid);

        }
        [Test]
        public async Task EditUsers_Post_IfDataIsNotChanged_IdentityresultSuccessAndRedirect()
        {
            string userId = "1";

            var model = new Admin_EditUserViewModel()
            {
                Id = "1",
                Name = "Janusz",
                Surname = "Testowy",
                City = "Kraków",
                EmployeeNumber = "23456789012",
                Roles = new List<string>() { "Admin" },
                Email = "a@a.pl"
            };

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.UpdateAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUser(model) as RedirectToActionResult;

            Assert.IsTrue(controller.ModelState.IsValid);
            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "Index" && result.ControllerName == "Employees");

        }
        [Test]
        public async Task EditUsers_Post_IfEmailIsNotValid_IdentityResultFailedWithError()
        {
            string userId = "1";
            string email = "b@b.pl";

            var model = new Admin_EditUserViewModel()
            {
                Id = "1",
                Name = "Janusz",
                Surname = "Testowy",
                City = "Kraków",
                EmployeeNumber = "23456789012",
                Roles = new List<string>() { "Admin" },
                Email = email
            };

            var error = new IdentityErrorDescriber().InvalidEmail(email);

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.UpdateAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(IdentityResult.Failed(error)))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.EditUser(model) as ViewResult;

            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == error.Description);
            Assert.AreEqual(result.ViewData.Model, model);

            //reset data

            var user = _context.Users.Find("1");
            user.Email = "a@a.pl";

        }
        [Test]
        public async Task ChangePassword_Get_IfInvalidUserId_ReturnNotFound()
        {
            string userId = "fakeUserID";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ChangePassword(userId) as ViewResult;

            Assert.IsTrue(result.ViewName == "NotFound" && result.ViewData["ErrorMessage"].ToString() == $"Użytkownik o numerze ID: {userId} nie został odnaleziony!");

        }
        [Test]
        public async Task ChangePassword_Get_IfValidUserId_ReturnViewResult()
        {
            string userId = "1";
            var userName = _context.Users.FindAsync("1").Result.UserName;

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ChangePassword(userId) as ViewResult;
            var user = (Admin_ChangePasswordViewModel)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(user.Login == userName);

        }
        [Test]
        public async Task ChangePassword_Post_IfInvalidUserName_ReturnNotFound()
        {

            var model = new Admin_ChangePasswordViewModel()
            {
                FullName = "Testowy Janusz",
                Login = "TestoJan9015",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                NewPasswordConfirm = "NewPassword2"
            };

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByNameAsync(model.Login))
                .ReturnsAsync(await _context.Users.FindAsync(model.Login)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ChangePassword(model) as ViewResult;

            Assert.IsTrue(result.ViewName == "NotFound");
            Assert.IsTrue(result.ViewData["ErrorMessage"].ToString() == $"Użytkownik: {model.Login} nie został odnaleziony!");

        }
        [Test]
        public async Task ChangePassword_IfPasswordMismatch_ReturnSameViewWithErrorMessage()
        {
            string userId = "1";

            var model = new Admin_ChangePasswordViewModel()
            {
                FullName = "Testowy Janusz",
                Login = "TestoJan9012",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                NewPasswordConfirm = "NewPassword2"
            };

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByNameAsync(model.Login))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ChangePassword(model) as ViewResult;

            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Hasła nie pasują do siebie");

        }
        [Test]
        public async Task ChangePassword_Post_IfValidData_IdentityResultSuccess()
        {
            string userId = "1";

            var model = new Admin_ChangePasswordViewModel()
            {
                FullName = "Testowy Janusz",
                Login = "TestoJan9012",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                NewPasswordConfirm = "NewPassword"
            };

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByNameAsync(model.Login))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.ChangePasswordAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ChangePassword(model) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "EditUser" && result.RouteValues["id"].ToString() == "1");

        }
        [Test]
        public async Task ChangePassword_Post_IfLoginIsInUse_ReturnSameViewWithErrorMessage()
        {
            var userId = "1";

            var model = new Admin_ChangePasswordViewModel()
            {
                FullName = "Testowy Janusz",
                Login = "TestoJan9012",
                OldPassword = "OldPassword",
                NewPassword = "NewPassword",
                NewPasswordConfirm = "NewPassword"
            };

            var error = new IdentityErrorDescriber().LoginAlreadyAssociated();

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByNameAsync(model.Login))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.ChangePasswordAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(IdentityResult.Failed(error)))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ChangePassword(model) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == error.Description);
            Assert.AreEqual(model, result.ViewData.Model);

        }
        [Test]
        public async Task DeleteUser_Get_IfInvalidUserId_ReturnNotFound()
        {
            string userId = "fakeUserID";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.DeleteUser(userId) as ViewResult;

            Assert.IsTrue(result.ViewName == "NotFound" && result.ViewData["ErrorMessage"].ToString() == $"Użytkownik o numerze ID: {userId} nie został odnaleziony!");

        }
        [Test]
        public async Task DeleteUser_Post_IfValidUserId_IdentityResultSuccess()
        {
            string userId = "1";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.DeleteAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.DeleteUser(userId) as RedirectToActionResult;

            Assert.IsTrue(result.ActionName == "Index" && result.ControllerName == "Employees");

        }
        [Test]
        public async Task DeleteUser_Post_IfUserIsAssignedToRole_ReturnDBError()
        {
            string userId = "1";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.DeleteAsync(It.IsAny<WMSIdentityUser>()))
                .Throws(new Microsoft.EntityFrameworkCore.DbUpdateException()))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.DeleteUser(userId) as ViewResult;

            Assert.IsTrue(result.ViewName == "DbExceptionHandler" && 
                result.ViewData["ErrorMessage"].ToString().Contains("Istnieje rola przypisana do tego użytkownika"));

        }
        [Test]
        public async Task DeleteUser_Post_IfUserIsInAdminRole_ReturnSameViewWithError()
        {
            string userId = "1";

            var error = new IdentityErrorDescriber().UserAlreadyInRole("Admin");

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.DeleteAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(IdentityResult.Failed(error)))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.DeleteUser(userId) as ViewResult;

            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == error.Description);

        }
        [Test]
        public async Task ManageUserRoles_Get_IfInvalidUserId_ReturnNotFound()
        {
            string userId = "fakeUserID";

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ManageUserRoles(userId) as ViewResult;

            Assert.IsTrue(result.ViewName == "NotFound" && result.ViewData["ErrorMessage"].ToString() == $"Użytkownik o numerze ID: {userId} nie został odnaleziony!");

        }
        [Test]
        public async Task ManageUserRoles_Get_IfValidUserId_ReturnViewWithListOfRoles()
        {
            string userId = "1";

            fakeRoleManager.Setup(frm => frm.Roles).Returns(_context.Roles);

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.IsInRoleAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<string>()))
                .ReturnsAsync(true))
                .Build();

            var httpContext = new Mock<HttpContext>();
            httpContext.Setup(h => h.User.Identity.Name).Returns("TESTOJAN9012");

            var controllerContext = new ControllerContext()
            {
                HttpContext = httpContext.Object
            };

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object) { ControllerContext = controllerContext };

            var result = await controller.ManageUserRoles(userId) as ViewResult;
            var roles = (List<UserRolesViewModel>)result.ViewData.Model;

            Assert.That(result, Is.InstanceOf<ViewResult>());
            Assert.IsTrue(roles[0].RoleName == "Standard");
            Assert.IsTrue(roles[1].RoleName == "Admin");

        }
        [Test]
        public async Task ManageUserRoles_Post_IfUserIsNull_ReturnNotFound()
        {
            string userId = "fakeUserID";

            var model = new List<UserRolesViewModel>()
            {
                new UserRolesViewModel()
                {
                    RoleId = "1",
                    RoleName = "Standard",
                    IsSelected = false
                }
            };

            fakeRoleManager.Setup(frm => frm.Roles).Returns(_context.Roles);

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ManageUserRoles(model, userId) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewName == "NotFound");
            Assert.IsTrue(result.ViewData["ErrorMessage"].ToString() == $"Użytkownik o numerze ID: {userId} nie został odnaleziony!");

        }
        [Test]
        public async Task ManageUserRoles_Post_IfAllDataAreValid_IdentityResultSuccess()
        {
            string userId = "1";

            var model = new List<UserRolesViewModel>()
            {
                new UserRolesViewModel()
                {
                    RoleId = "1",
                    RoleName = "Standard",
                    IsSelected = false
                },

                new UserRolesViewModel()
                {
                    RoleId = "Admin",
                    RoleName = "Admin",
                    IsSelected = true
                },
            };

            List<string> rolesList = new List<string>() { "Standard", "Admin " };


            fakeRoleManager.Setup(frm => frm.Roles).Returns(_context.Roles);

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.GetRolesAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(rolesList))
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IList<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.AddToRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ManageUserRoles(model, userId) as RedirectToActionResult;

            Assert.That(result, Is.InstanceOf(typeof(RedirectToActionResult)));
            Assert.IsTrue(result.ActionName == "EditUser" && result.RouteValues["id"].ToString() == "1");

        }
        [Test]
        public async Task ManageUserRoles_Post_IfCannotDeleteRoles_ReturnSameViewWithErrorMessage()
        {
            string userId = "1";

            var model = new List<UserRolesViewModel>()
            {
                new UserRolesViewModel()
                {
                    RoleId = "1",
                    RoleName = "Standard",
                    IsSelected = false
                },

                new UserRolesViewModel()
                {
                    RoleId = "Admin",
                    RoleName = "Admin",
                    IsSelected = true
                },
            };

            List<string> rolesList = new List<string>() { "Standard", "Admin" };


            fakeRoleManager.Setup(frm => frm.Roles).Returns(_context.Roles);

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.GetRolesAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(rolesList))
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IList<string>>()))
                .ReturnsAsync(IdentityResult.Failed()))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ManageUserRoles(model, userId) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Nie można usunąć istniejących ról użytkownika");

        }
        [Test]
        public async Task ManageUserRoles_Post_IfCannotAddRoles_ReturnSameViewWithErrorMessage()
        {
            string userId = "1";

            var model = new List<UserRolesViewModel>()
            {
                new UserRolesViewModel()
                {
                    RoleId = "1",
                    RoleName = "Standard",
                    IsSelected = false
                },

                new UserRolesViewModel()
                {
                    RoleId = "Admin",
                    RoleName = "Admin",
                    IsSelected = true
                },
            };

            List<string> rolesList = new List<string>() { "Standard", "Admin " };


            fakeRoleManager.Setup(frm => frm.Roles).Returns(_context.Roles);

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.Users)
                .Returns(_context.Users))
                .With(async b => b.Setup(fum => fum.FindByIdAsync(userId))
                .ReturnsAsync(await _context.Users.FindAsync(userId)).Verifiable())
                .With(b => b.Setup(fum => fum.GetRolesAsync(It.IsAny<WMSIdentityUser>()))
                .ReturnsAsync(rolesList))
                .With(b => b.Setup(fum => fum.RemoveFromRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IList<string>>()))
                .ReturnsAsync(IdentityResult.Success))
                .With(b => b.Setup(fum => fum.AddToRolesAsync(It.IsAny<WMSIdentityUser>(), It.IsAny<IEnumerable<string>>()))
                .ReturnsAsync(IdentityResult.Failed()))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.ManageUserRoles(model, userId) as ViewResult;

            Assert.That(result, Is.InstanceOf(typeof(ViewResult)));
            Assert.IsTrue(result.ViewData.ModelState[""].Errors[0].ErrorMessage == "Nie można przypisać zaznaczonych ról do użytkownika");

        }
        [Test]
        public async Task IsEmailInUse_IfInvalidEmail_ReturnSameViewWithErrorMessage()
        {
            string email = "invalidFormat";

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.IsEmailInUse(email, "") as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.ToString() == "Adres mailowy powinien być zapisany w formacie użytkownik@domena np. jankowalski@twojafirma.pl");

        }
        [Test]
        public async Task IsEmailInUse_IfEmailIsInUse_ReturnSameViewWithErrorMessage()
        {
            string email = "a@a.pl";
            var user = _context.Users.Find("1");

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.FindByEmailAsync(email))
                .ReturnsAsync(user.Email == email ? new WMSIdentityUser() : null))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.IsEmailInUse(email, "") as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.ToString() == $"Konto z powyższym adresem ({email}) już istnieje!");

        }
        [Test]
        public async Task IsEmailInUse_IfEmailIsNotInUse_ReturnJsonResultTrue()
        {
            string email = "b@a.pl";
            var user = _context.Users.Find("1");

            fakeUserManager = new FakeUserManagerBuilder()
                .With(b => b.Setup(fum => fum.FindByEmailAsync(email))
                .ReturnsAsync(user.Email == email ? new WMSIdentityUser() : null))
                .Build();

            var controller = new AdministrationController(fakeRoleManager.Object, fakeUserManager.Object, logger.Object);

            var result = await controller.IsEmailInUse(email, "") as JsonResult;

            Assert.That(result, Is.InstanceOf(typeof(JsonResult)));
            Assert.IsTrue(result.Value.Equals(true));

        }
    }
}
