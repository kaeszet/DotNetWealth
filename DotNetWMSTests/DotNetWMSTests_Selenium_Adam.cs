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
        private const string URI = "https://localhost:44387/";
        private string invalidCredentials = "(.//*[normalize-space(text()) and normalize-space(.)='Zarejestruj się'])[1]/following::li[1]";


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
        private void CreateItemForTest()
        {
            Login();
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd majątku')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowy produkt')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Type")).SendKeys("ZZZTestProd");
            driver.FindElement(By.Id("Name")).SendKeys("ZZZTestProd");
            driver.FindElement(By.Id("Producer")).SendKeys("ZZZTestProd");
            driver.FindElement(By.Id("Model")).SendKeys("ZZZTestProd");
            driver.FindElement(By.Id("ItemCode")).SendKeys("1");
            driver.FindElement(By.Id("Quantity")).SendKeys("1");
            driver.FindElement(By.Id("WarrantyDate")).SendKeys("01-01-2022");
            driver.FindElement(By.XPath("//button[contains(.,'Dodaj')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.XPath("//i[contains(@class,'fas fa-arrow-left')]")).Click();

        }
        private void CreateWarehouseForTest()
        {
            Login();
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd magazynów')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowy magazyn')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("ZZZTestowyMagazyn");
            driver.FindElement(By.Id("Street")).SendKeys("ZZZTestowaUlica");
            driver.FindElement(By.Id("ZipCode")).SendKeys("12-345");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.XPath("//i[contains(@class,'fas fa-arrow-left')]")).Click();
        }
        private void ClearDataAfterTest()
        {
            Thread.Sleep(1000);
            if (IsElementPresent(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")))
            {
                driver.FindElement(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")).Click();
            }
            if (IsElementPresent(By.XPath("//h1[contains(.,'Przekaż na stan')]")) || IsElementPresent(By.XPath("//h1[contains(.,'Przekaż do magazynu')]")) || IsElementPresent(By.XPath("//h1[contains(.,'Przekaż na zewnątrz')]")))
            {
                driver.FindElement(By.XPath("//i[contains(@class,'fas fa-arrow-left')]")).Click();
                Thread.Sleep(200);
                driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd majątku')]")).Click();
                Thread.Sleep(200);
            }
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Usuń')]")).Click();
        }

        [Test]
        public void AddItem()
        {
            Login();
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd majątku')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowy produkt')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Type")).SendKeys("Telefon");
            driver.FindElement(By.Id("Name")).SendKeys("ZZZTelefon");
            driver.FindElement(By.Id("Producer")).SendKeys("Apple");
            driver.FindElement(By.Id("Model")).SendKeys("Iphone 11 pro max");
            driver.FindElement(By.Id("ItemCode")).SendKeys("1");
            driver.FindElement(By.Id("Quantity")).SendKeys("1");
            new SelectElement(driver.FindElement(By.Id("State"))).SelectByText("Nowy");
            new SelectElement(driver.FindElement(By.Id("EmployeeId"))).SelectByIndex(1);
            new SelectElement(driver.FindElement(By.Id("WarehouseId"))).SelectByText("Pawia");
            new SelectElement(driver.FindElement(By.Id("ExternalId"))).SelectByText("Brak");
            driver.FindElement(By.Id("WarrantyDate")).SendKeys("01-01-2022");
            driver.FindElement(By.XPath("//button[contains(.,'Dodaj')]")).Click();
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            Assert.IsTrue(IsElementPresent(By.XPath($"//tr[{count}]")));
            driver.Navigate().GoToUrl("https://localhost:44387/Items");
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-edit')])[{count}]")).Click();
            driver.FindElement(By.Id("Model")).Clear();
            driver.FindElement(By.Id("Model")).SendKeys("Iphone 11 pro");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Zapisz')]")).Click();
            Thread.Sleep(1000);
            Assert.IsTrue(IsElementPresent(By.XPath($"//tr[{count}]")));
            ClearDataAfterTest();

        }
        [Test]
        public void DetailItem()
        {
            Login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd majątku')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-info-circle')])[{count}]")).Click();
            Thread.Sleep(200);
            Assert.IsTrue(IsElementPresent(By.XPath("//a[@class='btn btn-outline-dark'][contains(.,'Wróć do podglądu')]")));
        }

        [Test]
        public void AssignToEmployee()
        {
            CreateItemForTest();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przekaż na stan')]")).Click();
            Thread.Sleep(200);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//a[@class='btn btn-sm btn-warning'][contains(.,'Przypisz')])[{count}]")).Click();
            new SelectElement(driver.FindElement(By.Id("EmployeeId"))).SelectByIndex(1);
            driver.FindElement(By.XPath("//i[contains(@class,'fas fa-save')]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//h1[contains(.,'Przekaż na stan')]")));
            ClearDataAfterTest();
        }

        [Test]
        public void AssignToWarehouse()
        {
            CreateItemForTest();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przekaż do magazynu')]")).Click();
            Thread.Sleep(200);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//a[@class='btn btn-sm btn-warning'][contains(.,'Przypisz')])[{count}]")).Click();
            new SelectElement(driver.FindElement(By.Id("WarehouseId"))).SelectByIndex(1);
            driver.FindElement(By.XPath("//i[contains(@class,'fas fa-save')]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//h1[contains(.,'Przekaż do magazynu')]")));
            ClearDataAfterTest();
        }

        [Test]
        public void AssignToExternal()
        {
            CreateItemForTest();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przekaż na zewnątrz')]")).Click();
            Thread.Sleep(200);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//a[@class='btn btn-sm btn-warning'][contains(.,'Przypisz')])[{count}]")).Click();
            new SelectElement(driver.FindElement(By.Id("ExternalId"))).SelectByIndex(1);
            driver.FindElement(By.XPath("//i[contains(@class,'fas fa-save')]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//h1[contains(.,'Przekaż na zewnątrz')]")));
            ClearDataAfterTest();
        }
        [Test]
        public void DeleteItem()
        {
            CreateItemForTest();
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd majątku')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Usuń')]")).Click();
            Assert.IsFalse(IsElementPresent(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")));
        }
        [Test]
        public void NewWarehouse()
        {
            Login();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd magazynów')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowy magazyn')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("ZZZTestowyMagazyn");
            driver.FindElement(By.Id("Street")).SendKeys("ZZZSezamkowa");
            driver.FindElement(By.Id("ZipCode")).SendKeys("12-345");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            int countForAssertion = driver.FindElements(By.XPath("//tr")).Count - 1;
            Assert.IsTrue(++count == countForAssertion);
            ClearDataAfterTest();
        }
        [Test]
        public void EditWarehouse()
        {
            CreateWarehouseForTest();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd magazynów')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-edit')])[{count}]")).Click();
            driver.FindElement(By.Id("Street")).Clear();
            driver.FindElement(By.Id("Street")).SendKeys("ZZZRozrywki");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Zapisz')]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//td[contains(.,'ZZZRozrywki')]")));
            ClearDataAfterTest();
        }
        [Test]
        public void DetailWarehouse()
        {
            Login();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd magazynów')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//i[contains(@class,'fas fa-info-circle')])[{count}]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//h4[contains(.,'Szczegóły o magazynie')]")));

        }
        [Test]
        public void DeleteWarehouse()
        {
            CreateWarehouseForTest();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd magazynów')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//a[contains(@class,'btn btn-sm btn-danger text-dark')])[{count}]")).Click();
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Usuń')]")).Click();
            Assert.IsFalse(IsElementPresent(By.XPath($"(//i[contains(@class,'fas fa-trash-alt')])[{count}]")));

        }
        [Test]
        public void Stocktaking()
        {
            Login();
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Inwentaryzacja')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.XPath("//button[@class='btn btn-success d-print-none'][contains(.,'Inwentaryzuj')]")).Click();
            Thread.Sleep(200);
            Assert.IsTrue(IsElementPresent(By.XPath("//th[contains(.,'Sprawdzono')]")));
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
