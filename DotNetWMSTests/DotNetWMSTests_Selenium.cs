using System;
using System.Collections.Generic;
using System.Text;
using DotNetWMS.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DotNetWMS.Models;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using DotNetWMSTests.Selenium_test;
using System.Threading;

namespace DotNetWMSTests
{
    [TestFixture]
    public class DotNetWMSTests_Selenium
    {

        private IWebDriver driver;
        private const string URI = "https://localhost:44387/";
        private string invalidCredentials = "(.//*[normalize-space(text()) and normalize-space(.)='Zarejestruj się'])[1]/following::li[1]";
        private int count;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            
        }

        [TearDown]
        public void TeardownTest()
        {
            try
            {
                driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }
        [Test]
        public void Department_CreateNewDeptWithNotExistingData_ReturnDeptMainPage()
        {
            DateTime dateTime = DateTime.Now;
            DepartmentPage deptPage = new DepartmentPage(driver);
            deptPage.GoToPage();
            deptPage.CreateNewLinkText.Click();
            deptPage.DepartmentName.SendKeys($"TestDept + {dateTime.ToLongTimeString()}");
            deptPage.CreateButton.Click();
            Thread.Sleep(200);
            Assert.IsTrue(deptPage.CreateNewLinkText.Displayed);

        }
        [Test]
        public void Department_CreateNewDeptWithExistingData_ReturnErrorSpan()
        {
            bool isCreateFailInfoDisplayed;
            DepartmentPage deptPage = new DepartmentPage(driver);
            deptPage.GoToPage();
            deptPage.CreateNewLinkText.Click();
            deptPage.DepartmentName.SendKeys("ExistedDept");
            deptPage.CreateButton.Click();
            Thread.Sleep(200);
            try
            {
                isCreateFailInfoDisplayed = deptPage.CreateFailInfo.Displayed;
            }
            catch (NoSuchElementException)
            {
                deptPage.CreateNewLinkText.Click();
                deptPage.DepartmentName.SendKeys("ExistedDept");
                deptPage.CreateButton.Click();
            }
           
            Assert.IsTrue(deptPage.CreateFailInfo.Displayed);

        }
        [Test]
        public void Employee_LoginAddEditRemoveObject_PathWithCorrectData()
        {
            
            driver.Navigate().GoToUrl(URI);
            Login();
            driver.FindElement(By.XPath("//a/i")).Click();            
            driver.FindElement(By.LinkText("Dodaj użytkownika")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Jessica");
            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            driver.FindElement(By.Id("Pesel")).SendKeys("12345678901");
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Księgowy/a");
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[1]")).Click();
            driver.FindElement(By.Id("Pesel")).SendKeys("12345678902");
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Sprzedawca");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            driver.FindElement(By.XPath($"//tr[{count}]/td[8]/a[2]/i")).Click();
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.FindElement(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[3]")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();

        }
        [Test]
        public void Employee_LoginAddEditRemoveObject_PathWithIncorrectData()
        {
            driver.Navigate().GoToUrl(URI);
            Login();
            driver.FindElement(By.XPath("//a/i")).Click();
            
            driver.FindElement(By.LinkText("Dodaj użytkownika")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Janusz");
            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            driver.FindElement(By.Id("Pesel")).SendKeys("1234567");
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Sprzedawca");
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("30000");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Pesel-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("Pesel")).SendKeys("12345670000");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[1]")).Click();
            driver.FindElement(By.XPath("//body")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Grażyna");
            driver.FindElement(By.Id("Pesel")).SendKeys("123456700");
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Księgowy/a");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("aa000");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Pesel-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Pesel")).SendKeys("12345670011");
            driver.FindElement(By.Id("Surname")).Clear();
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Surname-error")));
            Assert.IsTrue(IsElementPresent(By.Id("Street-error")));
            Assert.IsTrue(IsElementPresent(By.Id("City-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            driver.FindElement(By.XPath($"//tr[{count}]/td[8]/a[2]/i")).Click();
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.FindElement(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[3]")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();

            Assert.IsFalse(IsElementPresent(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[3]")));
;
        }
        [Test]
        public void AccountLogin_UseInvalidLoginAndPassword_ReturnLoginFailInfo()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.GoToPage();
            loginPage.UserName.SendKeys("InvalidLogin");
            loginPage.Password.SendKeys("InvalidPassword");
            loginPage.Submit.Click();
            Assert.IsTrue(loginPage.LoginFailInfo.Displayed);

        }
        [Test]
        public void AccountLogin_UseValidLoginAndPassword_ReturnLoginSuccessButton()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.GoToPage();
            loginPage.UserName.SendKeys("TestoJan9012");
            loginPage.Password.SendKeys("Test123!");
            loginPage.Submit.Click();
            Assert.IsTrue(loginPage.LoginSuccessButton.Displayed && loginPage.LoginSuccessButton.Text == "Logout TestoJan9012");

        }
        [Test]
        public void AccountRegistration_LeaveRequiredFieldsEmpty_ShowExceptionList()
        {
            RegisterPage registerPage = new RegisterPage(driver);
            registerPage.GoToPage();
            registerPage.Submit.Click();
            Assert.IsTrue(registerPage.FirstExceptionOnList.Displayed);

        }
        [Test]
        public void AccountRegistration_UseInvalidEmployeeId_ShowException()
        {
            RegisterPage registerPage = new RegisterPage(driver);
            registerPage.GoToPage();
            registerPage.EmpId.SendKeys("InvalidEmpId");
            registerPage.Submit.Click();
            Assert.IsTrue(registerPage.EmpIdSpan.Displayed && registerPage.EmpIdSpan.Text == "Nieprawidłowy identyfikator!");

        }

        public void Registration()
        {
            driver.Navigate().GoToUrl(URI);
            driver.FindElement(By.LinkText("Zarejestruj się")).Click();
            driver.FindElement(By.Id("Name")).SendKeys("Janusz");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.Id("Surname")).SendKeys("Testowy");
            driver.FindElement(By.Id("EmployeeNumber")).SendKeys("123456789012");
            driver.FindElement(By.Id("Email")).SendKeys("a@a.pl");
            driver.FindElement(By.Id("Password")).SendKeys("Test123!");
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Test123!");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Potwierdź hasło'])[1]/following::button[1]")).Click();
        }
        
        public void Login()
        {
            driver.Navigate().GoToUrl(URI);
            driver.FindElement(By.LinkText("Zaloguj się")).Click();
            driver.FindElement(By.Id("inputEmail")).SendKeys("TestoJan9012");
            driver.FindElement(By.Id("inputPassword")).SendKeys("Test123!");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Hasło'])[1]/following::button[1]")).Click();
            if (IsElementPresent(By.XPath(invalidCredentials)))
            {
                Registration();
            }
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }


    }
}
