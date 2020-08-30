using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetWMSTests.Selenium_test
{
    class LoginPage
    {
        private IWebDriver driver;
        public LoginPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.XPath, Using = "//input[contains(@name,'Login')]")]
        public IWebElement UserName { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@name='Password']")]
        public IWebElement Password { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Zaloguj')]")]
        public IWebElement Submit { get; set; }
        [FindsBy(How = How.XPath, Using = "//li[contains(.,'Nieprawidłowy login lub hasło')]")]
        public IWebElement LoginFailInfo { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        public IWebElement LoginSuccessButton { get; set; }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Account/Login");
        }
    }
}
