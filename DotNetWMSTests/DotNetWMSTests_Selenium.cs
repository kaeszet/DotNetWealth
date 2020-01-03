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

namespace DotNetWMSTests
{
    [TestFixture]
    public class DotNetWMSTests_Selenium
    {

        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private const string URI = "https://localhost:44387/";
        private bool acceptNextAlert = true;
        private int count;

        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            verificationErrors = new StringBuilder();
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
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [Test]
        public void Employee_LoginAddEditRemoveObject_PathWithCorrectData()
        {
            Registration();
            Login();
            driver.FindElement(By.XPath("//a/i")).Click();            
            driver.FindElement(By.LinkText("Dodaj użytkownika")).Click();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Jessica");
            driver.FindElement(By.Id("Surname")).Clear();
            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Pesel")).SendKeys("12345678901");
            driver.FindElement(By.Id("DepartmentId")).Click();
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Księgowy/a");
            driver.FindElement(By.Id("DepartmentId")).Click();
            driver.FindElement(By.Id("Street")).Click();
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[3]/following::a[1]")).Click();
            driver.FindElement(By.Id("Pesel")).Click();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Pesel")).SendKeys("12345678902");
            driver.FindElement(By.Id("DepartmentId")).Click();
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Sprzedawca");
            driver.FindElement(By.Id("DepartmentId")).Click();
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            driver.FindElement(By.XPath("//tr[3]/td[8]/a[2]/i")).Click();
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[3]/following::a[3]")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();

        }
        [Test]
        public void Employee_LoginAddEditRemoveObject_PathWithIncorrectData()
        {
            driver.Navigate().GoToUrl(URI);
            driver.FindElement(By.XPath("//a/i")).Click();
            
            driver.FindElement(By.LinkText("Dodaj użytkownika")).Click();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Janusz");
            driver.FindElement(By.Id("Surname")).Click();
            driver.FindElement(By.Id("Surname")).Clear();
            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            driver.FindElement(By.Id("Pesel")).Click();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Pesel")).SendKeys("1234567");
            driver.FindElement(By.Id("DepartmentId")).Click();
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Sprzedawca");
            driver.FindElement(By.Id("DepartmentId")).Click();
            driver.FindElement(By.Id("Street")).Click();
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("30000");
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Pesel-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("ZipCode")).Click();
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("Pesel")).Click();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Pesel")).SendKeys("12345670000");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[1]")).Click();
            driver.FindElement(By.XPath("//body")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Grażyna");
            driver.FindElement(By.Id("Pesel")).Click();
            driver.FindElement(By.Id("Pesel")).Clear();
            driver.FindElement(By.Id("Pesel")).SendKeys("123456700");
            driver.FindElement(By.Id("DepartmentId")).Click();
            new SelectElement(driver.FindElement(By.Id("DepartmentId"))).SelectByText("Księgowy/a");
            driver.FindElement(By.Id("DepartmentId")).Click();
            driver.FindElement(By.Id("ZipCode")).Click();
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("aa000");
            //driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Miasto'])[1]/following::div[1]")).Click();
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("Pesel-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            //driver.FindElement(By.Id("ZipCode")).Clear();
            //driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
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

            driver.FindElement(By.Id("Surname")).Click();
            driver.FindElement(By.Id("Surname")).Clear();
            driver.FindElement(By.Id("Surname")).SendKeys("Testowa");
            //driver.FindElement(By.Id("Street")).Click();
            //driver.FindElement(By.Id("Surname")).Clear();
            //driver.FindElement(By.Id("Surname")).SendKeys("Testowy");
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("Street")).SendKeys("św. Filipa");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("30-000");
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            driver.FindElement(By.XPath("//tr[3]/td[8]/a[2]/i")).Click();
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.FindElement(By.LinkText("Back to List")).Click();
            driver.FindElement(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[3]")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();

            Assert.IsFalse(IsElementPresent(By.XPath($"(.//*[normalize-space(text()) and normalize-space(.)='Kraków'])[{count}]/following::a[3]")));

            driver.FindElement(By.LinkText(".NetFortune")).Click();
        }
        [Test]
        public void Registration()
        {
            driver.Navigate().GoToUrl(URI);
            driver.FindElement(By.LinkText("Zarejestruj się")).Click();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Janusz");
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.Id("Surname")).Click();
            driver.FindElement(By.Id("Surname")).Clear();
            driver.FindElement(By.Id("Surname")).SendKeys("Testowy");
            driver.FindElement(By.Id("EmployeeNumber")).Click();
            driver.FindElement(By.Id("EmployeeNumber")).Clear();
            driver.FindElement(By.Id("EmployeeNumber")).SendKeys("123456789012");
            driver.FindElement(By.Id("Email")).Click();
            driver.FindElement(By.Id("Email")).Clear();
            driver.FindElement(By.Id("Email")).SendKeys("a@a.pl");
            driver.FindElement(By.Id("Password")).Click();
            driver.FindElement(By.Id("Password")).Clear();
            driver.FindElement(By.Id("Password")).SendKeys("Test123!");
            driver.FindElement(By.Id("ConfirmPassword")).Clear();
            driver.FindElement(By.Id("ConfirmPassword")).SendKeys("Test123!");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Potwierdź hasło'])[1]/following::button[1]")).Click();
        }
        [Test]
        public void Login()
        {
            driver.Navigate().GoToUrl(URI);
            driver.FindElement(By.LinkText("Zaloguj się")).Click();
            driver.FindElement(By.Id("inputEmail")).Click();
            driver.FindElement(By.Id("inputEmail")).Clear();
            driver.FindElement(By.Id("inputEmail")).SendKeys("TestoJan9012");
            driver.FindElement(By.Id("inputPassword")).Click();
            driver.FindElement(By.Id("inputPassword")).Clear();
            driver.FindElement(By.Id("inputPassword")).SendKeys("Test123!");
            driver.FindElement(By.XPath("(.//*[normalize-space(text()) and normalize-space(.)='Hasło'])[1]/following::button[1]")).Click();
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

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }

    }
}
