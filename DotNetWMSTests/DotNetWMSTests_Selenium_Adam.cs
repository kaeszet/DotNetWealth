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
    public class DotNetWMSTests_Selenium_Adam
    {
        private IWebDriver driver;
        private StringBuilder verificationErrors;
        private int count;
        private bool acceptNextAlert = true;

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
        public void AddItem()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("(//a[@type='button'])[3]")).Click();
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Type")).Click();
            driver.FindElement(By.Id("Type")).Clear();
            driver.FindElement(By.Id("Type")).SendKeys("Telefon");
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Telefon");
            driver.FindElement(By.Id("Producer")).Clear();
            driver.FindElement(By.Id("Producer")).SendKeys("Apple");
            driver.FindElement(By.Id("Model")).Clear();
            driver.FindElement(By.Id("Model")).SendKeys("Iphone 11 pro max");
            driver.FindElement(By.Id("ItemCode")).Clear();
            driver.FindElement(By.Id("ItemCode")).SendKeys("1");
            driver.FindElement(By.Id("Quantity")).Clear();
            driver.FindElement(By.Id("Quantity")).SendKeys("1");
            driver.FindElement(By.Id("State")).Click();
            new SelectElement(driver.FindElement(By.Id("State"))).SelectByText("Nowy");
            driver.FindElement(By.Id("State")).Click();
            driver.FindElement(By.Id("EmployeeId")).Click();
            new SelectElement(driver.FindElement(By.Id("EmployeeId"))).SelectByText("Testowa Janusz");
            driver.FindElement(By.Id("EmployeeId")).Click();
            driver.FindElement(By.Id("WarehouseId")).Click();
            new SelectElement(driver.FindElement(By.Id("WarehouseId"))).SelectByText("Pawia");
            driver.FindElement(By.Id("WarehouseId")).Click();
            driver.FindElement(By.Id("ExternalId")).Click();
            new SelectElement(driver.FindElement(By.Id("ExternalId"))).SelectByText("Brak");
            driver.FindElement(By.Id("ExternalId")).Click();
            driver.FindElement(By.Id("WarrantyDate")).Click();
            driver.FindElement(By.Id("WarrantyDate")).Clear();
            driver.FindElement(By.Id("WarrantyDate")).SendKeys("01-01-2022");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            Assert.IsTrue(IsElementPresent(By.XPath($"//tr[{count}]/td[4]")));
            // ERROR: Caught exception [unknown command []]
            // ERROR: Caught exception [unknown command []]
            driver.Navigate().GoToUrl("https://localhost:44387/Items");
            driver.FindElement(By.XPath("(//a[contains(text(),'Edit')])[2]")).Click();
            driver.FindElement(By.Id("Model")).Click();
            driver.FindElement(By.Id("Model")).Clear();
            driver.FindElement(By.Id("Model")).SendKeys("Iphone 11 pro");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            Thread.Sleep(200);
            Assert.IsTrue(IsElementPresent(By.XPath("//tr[2]/td[4]")));
            // ERROR: Caught exception [unknown command []]
        }
        [Test]
        public void DetailItem()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Items");
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(//a[contains(text(),'Details')])[{count}]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//main/div")));
        }

        [Test]
        public void AssignToEmployee()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("//div[2]/div[2]/div[2]/div[2]/a/i")).Click();
            driver.FindElement(By.XPath("(//a[contains(text(),'Assign')])[2]")).Click();
            driver.FindElement(By.Id("EmployeeId")).Click();
            new SelectElement(driver.FindElement(By.Id("EmployeeId"))).SelectByText("Testowa Janusz");
            driver.FindElement(By.Id("EmployeeId")).Click();
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            // ERROR: Caught exception [unknown command []]
        }

        [Test]
        public void AssignToWarehouse()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("//div[3]/div[2]/a/i")).Click();
            driver.FindElement(By.XPath("(//a[contains(text(),'Assign')])[2]")).Click();
            driver.FindElement(By.Id("WarehouseId")).Click();
            new SelectElement(driver.FindElement(By.Id("WarehouseId"))).SelectByText("Hala, Myśliwska");
            driver.FindElement(By.Id("WarehouseId")).Click();
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            // ERROR: Caught exception [unknown command []]
        }

        [Test]
        public void AssignToExternal()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("//div[4]/div[2]/a/i")).Click();
            driver.FindElement(By.XPath("(//a[contains(text(),'Assign')])[2]")).Click();
            driver.FindElement(By.Id("ExternalId")).Click();
            new SelectElement(driver.FindElement(By.Id("ExternalId"))).SelectByText("Brak");
            driver.FindElement(By.Id("ExternalId")).Click();
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//tr[2]/td[8]")));
            // ERROR: Caught exception [unknown command []]
        }
        [Test]
        public void DeleteItem()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Items");
            driver.FindElement(By.XPath("(//a[contains(text(),'Delete')])[2]")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            Assert.IsFalse(IsElementPresent(By.XPath("//tr[2]/td[4]")));
        }
        [Test]
        public void NewWarehouse()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Warehouses");
            driver.FindElement(By.LinkText("Create New")).Click();
            driver.FindElement(By.Id("Name")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("Nowy Magazyn");
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("Street")).SendKeys("Sezamkowa");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("31-011");
            driver.FindElement(By.Id("City")).Clear();
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            Assert.IsTrue(IsElementPresent(By.XPath($"//tr[{count}]/td")));
            // ERROR: Caught exception [unknown command []]
        }
        [Test]
        public void EditWarehouse()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Warehouses");
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(//a[contains(text(),'Edit')])[{count}]")).Click();
            driver.FindElement(By.Id("Street")).Click();
            driver.FindElement(By.Id("Street")).Click();
            // ERROR: Caught exception [ERROR: Unsupported command [doubleClick | id=Street | ]]
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("Street")).SendKeys("Rozrywki");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath($"//tr[{count}]/td[2]")));
        }
        [Test]
        public void DetailWarehouse()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Warehouses");
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(//a[contains(text(),'Details')])[{count}]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//dd")));

        }
        [Test]
        public void DeleteWarehouse()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Warehouses");
            count = driver.FindElements(By.XPath("//*[@class='table']/tbody/tr")).Count;
            driver.FindElement(By.XPath($"(//a[contains(text(),'Delete')])[{count}]")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
            Assert.IsFalse(IsElementPresent(By.XPath($"//tr[{count}]/td")));

        }
        [Test]
        public void Stocktaking()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("//button/i")).Click();
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
