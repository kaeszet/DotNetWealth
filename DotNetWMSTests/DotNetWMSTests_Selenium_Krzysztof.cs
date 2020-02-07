using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using DotNetWMSTests.Selenium_test;

namespace DotNetWMSTests
{
    [TestFixture]
    public class DotNetWMSTests_Selenium_Krzysztof
    {
        private IWebDriver driver;


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
        public void Externals_CreateTwoNewClient_ReturnViewList()
        {
            //First Client

            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("//div[4]/div[2]/div/div[2]/a/i")).Click();
            driver.FindElement(By.LinkText("Create New")).Click();
            new SelectElement(driver.FindElement(By.Id("Type"))).SelectByText("Wypożyczający");
            driver.FindElement(By.Id("Name")).SendKeys("Janek");
            driver.FindElement(By.Id("TaxId")).SendKeys("4440001119");
            driver.FindElement(By.Id("Street")).SendKeys("Górników");
            driver.FindElement(By.Id("ZipCode")).SendKeys("31-111");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();

            //Second Client

            driver.FindElement(By.LinkText("Create New")).Click();
            new SelectElement(driver.FindElement(By.Id("Type"))).SelectByText("Serwis");
            driver.FindElement(By.Id("Name")).SendKeys("Waldek");
            driver.FindElement(By.Id("TaxId")).SendKeys("1234567890");
            driver.FindElement(By.Id("Street")).SendKeys("Pole 1");
            driver.FindElement(By.Id("ZipCode")).SendKeys("01-112");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
        }

        [Test]
        public void Externals_DeleteClient_ReturnView()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Externals");
            driver.FindElement(By.LinkText("Delete")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
        }


        [Test]
        public void Externals_CreateNewClientWithValidation_ReturnViewTest()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Externals");
            driver.FindElement(By.LinkText("Create New")).Click();           
            driver.FindElement(By.Id("Name")).SendKeys("Janek"); 
            driver.FindElement(By.Id("TaxId")).SendKeys("1122332");       
            driver.FindElement(By.Id("Street")).SendKeys("Twoja 11");
            driver.FindElement(By.Id("ZipCode")).SendKeys("11223");
            driver.FindElement(By.XPath("//main/div")).Click();
            driver.FindElement(By.Id("City")).SendKeys("Rzeszów");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
            driver.FindElement(By.Id("TaxId")).SendKeys("112233211");
            driver.FindElement(By.Id("TaxId")).SendKeys("1122332111");
            driver.FindElement(By.Id("ZipCode")).SendKeys("11-223");
            driver.FindElement(By.XPath("//input[@value='Create']")).Click();
        }

        [Test]
        public void Externals_SearchClientWithNipNumberAndEdit_ReturnViewTest()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Externals?search=");
            driver.FindElement(By.Name("search")).SendKeys("6792480093");
            driver.FindElement(By.XPath("//button/i")).Click();
            driver.FindElement(By.LinkText("Edit")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("JanekEdit");
            driver.FindElement(By.XPath("//input[@value='Save']")).Click();
        }

        [Test]
        public void Error_OpenErrorMessageIfPageDoesNotExistAndBackToHomePage_ReturnErrorView()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.FindElement(By.XPath("//div[4]/div[2]/div/div[2]/a/i")).Click();

            driver.Navigate().GoToUrl("https://localhost:44387/External/invalid");

            driver.FindElement(By.LinkText("Wróć do strony głównej")).Click();

        }

        [Test]
        public void Error_OpenGlobalError_ReturnErrorView()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/");
            driver.Navigate().GoToUrl("https://localhost:44387/Error");


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

   
    }
}
