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
using System.Linq;

namespace DotNetWMSTests
{
    [TestFixture]
    public class DotNetWMSTests_Selenium
    {

        private IWebDriver driver;
        private const string URI = "https://localhost:44387/";
        private string UniquePesel;
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
            Login();
            DateTime dateTime = DateTime.Now;
            DepartmentPage deptPage = new DepartmentPage(driver);
            deptPage.GoToPage();
            deptPage.CreateNewLinkText.Click();
            deptPage.DepartmentName.SendKeys($"ZZZTestDept + {dateTime.ToLongTimeString()}");
            deptPage.CreateButton.Click();
            Thread.Sleep(200);
            Assert.IsTrue(deptPage.CreateNewLinkText.Displayed);
            ClearDataAfterTest();

        }
        [Test]
        public void Department_CreateNewDeptWithExistingData_ReturnErrorSpan()
        {
            Login();
            bool isCreateFailInfoDisplayed;
            DepartmentPage deptPage = new DepartmentPage(driver);
            deptPage.GoToPage();
            deptPage.CreateNewLinkText.Click();
            deptPage.DepartmentName.SendKeys("ZZZExistedDept");
            deptPage.CreateButton.Click();
            Thread.Sleep(1000);
            try
            {
                isCreateFailInfoDisplayed = deptPage.CreateFailInfo.Displayed;
            }
            catch (NoSuchElementException)
            {
                deptPage.CreateNewLinkText.Click();
                deptPage.DepartmentName.SendKeys("ZZZExistedDept");
                deptPage.CreateButton.Click();
            }
           
            Assert.IsTrue(deptPage.CreateFailInfo.Displayed);
            ClearDataAfterTest();
            

        }
        [Test]
        public void Employee_LoginAddEditRemoveObject_PathWithCorrectData()
        {
            
            Login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd pracowników')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Dodaj pracownika")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("Jessica");
            driver.FindElement(By.Id("Surname")).SendKeys("ZZZ");
            UniquePesel = DateTime.Now.ToString("yMMddHHmmss");
            driver.FindElement(By.Id("Pesel")).SendKeys(UniquePesel);
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Księgowy/a");
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-edit')])[{count}]")).Click();
            UniquePesel = DateTime.Now.ToString("yyMMddHHmms");
            driver.FindElement(By.Id("Pesel")).SendKeys(UniquePesel);
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Sprzedawca");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Zapisz')]")).Click();
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-info-circle')])[{count}]")).Click();
            driver.FindElement(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")).Click();
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")).Click();
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Usuń')]")).Click();

        }
        [Test]
        public void Employee_LoginAddEditRemoveObject_PathWithIncorrectData()
        {
            Login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd pracowników')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.LinkText("Dodaj pracownika")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("Janusz");
            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            driver.FindElement(By.Id("Pesel")).SendKeys("1234567");
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Sprzedawca");
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("30000");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Pesel-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            UniquePesel = DateTime.Now.ToString("yMMddHHmmss");
            driver.FindElement(By.Id("Pesel")).SendKeys(UniquePesel);
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-edit')])[{count}]")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Grażyna");
            driver.FindElement(By.Id("Pesel")).SendKeys("123456700");
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Księgowy/a");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("aa000");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Zapisz')]")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Pesel-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("Pesel")).Clear();
            UniquePesel = DateTime.Now.ToString("yyyMMddHHmm");
            driver.FindElement(By.Id("Pesel")).SendKeys(UniquePesel);
            driver.FindElement(By.Id("Surname")).Clear();
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Zapisz')]")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Surname-error")));
            Assert.IsTrue(IsElementPresent(By.Id("Street-error")));
            Assert.IsTrue(IsElementPresent(By.Id("City-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("Surname")).SendKeys("ZZZTestowa");
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Zapisz')]")).Click();
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-info-circle')])[{count}]")).Click();
            driver.FindElement(By.XPath("//a[@class='btn btn-sm btn-warning'][contains(.,'Edytuj')]")).Click();
            driver.FindElement(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")).Click();
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")).Click();
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Usuń')]")).Click();

            Assert.IsFalse(IsElementPresent(By.XPath($"(//i[contains(@class,'fas fa-info-circle')])[{count}]")));
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
            Assert.IsTrue(loginPage.LoginSuccessButton.Displayed && loginPage.LoginSuccessButton.Text == "Wyloguj się\r\nTestoJan9012");

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

        private void Registration()
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
        
        private void Login()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.GoToPage();
            loginPage.UserName.SendKeys("AdModJan9012");
            loginPage.Password.SendKeys("Test123!");
            loginPage.Submit.Click();
            if (IsElementPresent(By.XPath(invalidCredentials)))
            {
                Registration();
            }
        }

        private void ClearDataAfterTest()
        {
            Thread.Sleep(1000);
            if (IsElementPresent(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")))
            {
                driver.FindElement(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")).Click();
            }
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")).Click();
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Usuń')]")).Click();
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
