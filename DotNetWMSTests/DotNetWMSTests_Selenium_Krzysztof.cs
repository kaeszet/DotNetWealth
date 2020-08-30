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
        private int count;
        private const string URI = "https://localhost:44387/";
        private string invalidCredentials = "(.//*[normalize-space(text()) and normalize-space(.)='Zarejestruj się'])[1]/following::li[1]";

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
        private void CreateExternalForTest()
        {
            Login();
            driver.FindElement(By.XPath("//div[@class='card-body'][contains(.,'Przegląd kl. zewn.')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowego klienta')]")).Click();
            Thread.Sleep(1000);
            new SelectElement(driver.FindElement(By.Id("Type"))).SelectByIndex(1);
            driver.FindElement(By.Id("Name")).SendKeys("ZZZTestowyKlient");
            driver.FindElement(By.Id("TaxId")).SendKeys("4440001119");
            driver.FindElement(By.Id("Street")).SendKeys("Górników");
            driver.FindElement(By.Id("ZipCode")).SendKeys("31-111");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.XPath("//i[contains(@class,'fas fa-arrow-left')]")).Click();

        }

        [Test]
        public void Externals_CreateNewClient_ReturnViewList()
        {

            Login();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd kl. zewn.')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowego klienta')]")).Click();
            Thread.Sleep(1000);
            new SelectElement(driver.FindElement(By.Id("Type"))).SelectByText("Wypożyczający");
            driver.FindElement(By.Id("Name")).SendKeys("ZZZJanek");
            driver.FindElement(By.Id("TaxId")).SendKeys("4440001119");
            driver.FindElement(By.Id("Street")).SendKeys("Górników");
            driver.FindElement(By.Id("ZipCode")).SendKeys("31-111");
            driver.FindElement(By.Id("City")).SendKeys("Kraków");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//td[contains(.,'ZZZJanek')]")));
            ClearDataAfterTest();

        }

        [Test]
        public void Externals_DeleteClient_ReturnView()
        {
            CreateExternalForTest();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd kl. zewn.')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath($"(//a[contains(@class,'btn btn-sm btn-danger text-dark')])[{count}]")).Click();
            Thread.Sleep(200);
            driver.FindElement(By.XPath("//button[contains(.,'Usuń')]")).Click();
            Assert.IsFalse(IsElementPresent(By.XPath($"(//a[contains(@class,'btn btn-sm btn-danger text-dark')])[{count}]")));

        }

        [Test]
        public void Externals_CreateNewClientWithValidation_ReturnViewTest()
        {
            Login();
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd kl. zewn.')]")).Click();
            Thread.Sleep(1000);
            count = driver.FindElements(By.XPath("//tr")).Count - 1;
            driver.FindElement(By.XPath("//a[contains(.,'Dodaj nowego klienta')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("Name")).SendKeys("Janek"); 
            driver.FindElement(By.Id("TaxId")).SendKeys("1122332");       
            driver.FindElement(By.Id("Street")).SendKeys("Twoja 11");
            driver.FindElement(By.Id("ZipCode")).SendKeys("11223");
            driver.FindElement(By.Id("City")).SendKeys("Rzeszów");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();

            Assert.IsTrue(IsElementPresent(By.Id("TaxId-error")));
            Assert.IsTrue(IsElementPresent(By.Id("ZipCode-error")));

            driver.FindElement(By.Id("TaxId")).Clear();
            driver.FindElement(By.Id("TaxId")).SendKeys("1122332111");
            driver.FindElement(By.Id("ZipCode")).Clear();
            driver.FindElement(By.Id("ZipCode")).SendKeys("11-223");
            driver.FindElement(By.XPath("//button[@type='submit'][contains(.,'Dodaj')]")).Click();
            Thread.Sleep(200);
            int countForAssertion = driver.FindElements(By.XPath("//tr")).Count - 1;

            Assert.IsTrue(++count == countForAssertion);
            ClearDataAfterTest();

        }

        [Test]
        public void Externals_SearchClientWithNipNumberAndEdit_ReturnViewTest()
        {
            CreateExternalForTest();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//h6[@class='card-title'][contains(.,'Przegląd kl. zewn.')]")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//input[@name='search']")).SendKeys("4440001119");
            driver.FindElement(By.XPath("//i[contains(@class,'fab fa-sistrix')]")).Click();
            driver.FindElement(By.XPath("(//i[contains(@class,'fas fa-info-circle')])[1]")).Click();
            driver.FindElement(By.XPath("//a[@class='btn btn-sm btn-warning'][contains(.,'Edytuj')]")).Click();
            driver.FindElement(By.Id("Name")).Clear();
            driver.FindElement(By.Id("Name")).SendKeys("ZZZTestowyKlient2");
            driver.FindElement(By.XPath("//button[contains(.,'Zapisz')]")).Click();
            Assert.IsTrue(IsElementPresent(By.XPath("//td[contains(.,'ZZZTestowyKlient2')]")));
            ClearDataAfterTest();
        }

        [Test]
        public void Error_OpenErrorMessageIfPageDoesNotExistAndBackToHomePage_ReturnErrorView()
        {
            Login();
            driver.Navigate().GoToUrl("https://localhost:44387/External/invalid");
            Assert.IsTrue(IsElementPresent(By.XPath("//a[contains(.,'Wróć do strony głównej')]")));

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
