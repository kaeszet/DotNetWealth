using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using DotNetWMSTests.Selenium_test;
using System.Threading;

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
        public void Externals_CreateNewClient_ReturnViewList()
        {
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
        }

        [Test]
        public void Externals_DeleteClient_ReturnView()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Externals");
            driver.FindElement(By.LinkText("Delete")).Click();
            driver.FindElement(By.XPath("//input[@value='Delete']")).Click();
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
