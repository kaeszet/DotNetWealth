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

        [FindsBy(How = How.Id, Using = "inputEmail")]
        public IWebElement UserName { get; set; }
        [FindsBy(How = How.Id, Using = "inputPassword")]
        public IWebElement Password { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[normalize-space(text()) and normalize-space(.)='Hasło'])[1]/following::button[1]")]
        public IWebElement Submit { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[normalize-space(text()) and normalize-space(.)='Zarejestruj się'])[1]/following::li[1]")]
        public IWebElement LoginFailInfo { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[@type='submit']")]
        public IWebElement LoginSuccessButton { get; set; }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Account/Login");
        }
    }
}
