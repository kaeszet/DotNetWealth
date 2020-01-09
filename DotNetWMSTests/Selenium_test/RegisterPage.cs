using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetWMSTests.Selenium_test
{
    class RegisterPage
    {
        private IWebDriver driver;
        public RegisterPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        [FindsBy(How = How.Id, Using = "Name")]
        public IWebElement Name { get; set; }
        [FindsBy(How = How.Id, Using = "Surname")]
        public IWebElement Surname { get; set; }
        [FindsBy(How = How.Id, Using = "EmployeeNumber")]
        public IWebElement EmpId { get; set; }
        [FindsBy(How = How.Id, Using = "City")]
        public IWebElement Street { get; set; }
        [FindsBy(How = How.Id, Using = "Email")]
        public IWebElement Email { get; set; }
        [FindsBy(How = How.Id, Using = "Password")]
        public IWebElement Password { get; set; }
        [FindsBy(How = How.Id, Using = "ConfirmPassword")]
        public IWebElement ConfPassword { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[normalize-space(text()) and normalize-space(.)='Potwierdź hasło'])[1]/following::button[1]")]
        public IWebElement Submit { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[normalize-space(text()) and normalize-space(.)='Rejestracja'])[1]/following::li[1]")]
        public IWebElement FirstExceptionOnList { get; set; }
        [FindsBy(How = How.XPath, Using = "(.//*[normalize-space(text()) and normalize-space(.)='Identyfikator'])[1]/following::span[1]")]
        public IWebElement EmpIdSpan { get; set; }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Account/Register");
        }
        
    }
}
