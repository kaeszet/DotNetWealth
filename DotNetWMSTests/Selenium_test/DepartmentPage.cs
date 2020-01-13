using OpenQA.Selenium;
using SeleniumExtras.PageObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace DotNetWMSTests.Selenium_test
{
    class DepartmentPage
    {
        private IWebDriver driver;
        public DepartmentPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }
        [FindsBy(How = How.LinkText, Using = "Create New")]
        public IWebElement CreateNewLinkText { get; set; }
        [FindsBy(How = How.Id, Using = "Name")]
        public IWebElement DepartmentName { get; set; }
        [FindsBy(How = How.XPath, Using = "//input[@value='Create']")]
        public IWebElement CreateButton { get; set; }
        [FindsBy(How = How.Id, Using = "Name-error")]
        public IWebElement CreateFailInfo{ get; set; }

        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44387/Departments");
        }
    }
}
