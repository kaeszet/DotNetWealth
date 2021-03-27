using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.PageObjects;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace DotNetWMSTests.Selenium_test
{
    class ItemPage
    {
        private IWebDriver driver;


        public ItemPage(IWebDriver driver)
        {
            this.driver = driver;
            PageFactory.InitElements(driver, this);
        }

        //welcome text
        [FindsBy(How = How.XPath, Using = "//h2[contains(.,'Przegląd majątku')]")]
        public IWebElement WelcomeText { get; set; }

        //buttons
        [FindsBy(How = How.XPath, Using = "//a[@data-id='Items']")]
        public IWebElement Items_Button_Overview { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@href='/Items/ItemAssignment']")]
        public IWebElement Items_Button_Assign { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Przypisz do')]")]
        public IWebElement Items_Button_Assign_To { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Użytkownika')]")]
        public IWebElement Items_Button_Assign_ToUser { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Magazynu')]")]
        public IWebElement Items_Button_Assign_ToWarehouse { get; set; }
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Kontrahenta')]")]
        public IWebElement Items_Button_Assign_ToExternal { get; set; }
        [FindsBy(How = How.XPath, Using = "//a[@href='/Items/Create']")]
        public IWebElement Items_Button_AddNewItem{ get; set; }
        [FindsBy(How = How.Id, Using = "Dodaj")]
        public IWebElement Items_Button_Create_Submit{ get; set; }
        [FindsBy(How = How.XPath, Using = "//button[contains(.,'Zatwierdź')]")]
        public IWebElement Items_Button_Assign_Submit { get; set; }
        [FindsBy(How = How.XPath, Using = "(//button[@type='button'][contains(.,'Więcej')])[1]")]
        public IWebElement Items_Button_Index_More_1 { get; set; }
        [FindsBy(How = How.XPath, Using = "(//button[@type='button'][contains(.,'Usuń')])[1]")]
        public IWebElement Items_Button_Index_Delete_1{ get; set; }
        [FindsBy(How = How.XPath, Using = "(//a[@class='dropdown-item'][contains(.,'Szczegóły')])[1]")]
        public IWebElement Items_Button_Index_Details_1 { get; set; }
        [FindsBy(How = How.XPath, Using = "(//button[@type='submit'][contains(.,'Tak')])[1]")]
        public IWebElement Items_Button_Index_Alert_Yes_1 { get; set; }
        [FindsBy(How = How.Id, Using = "printQrCode")]
        public IWebElement Items_Button_Details_PrintQrCode { get; set; }


        //checkboxes
        [FindsBy(How = How.XPath, Using = "(//label[@for='z0__IsChecked'])[2]")]
        public IWebElement Items_Checkbox_IsChecked { get; set; }

        //inputs
        [FindsBy(How = How.Id, Using = "Producer")]
        public IWebElement Items_Input_Producer { get; set; }
        [FindsBy(How = How.Id, Using = "Name")]
        public IWebElement Items_Input_Name { get; set; }
        [FindsBy(How = How.Id, Using = "Type")]
        public IWebElement Items_Input_Type{ get; set; }
        [FindsBy(How = How.Id, Using = "Model")]
        public IWebElement Items_Input_Model { get; set; }
        [FindsBy(How = How.Id, Using = "ItemCode")]
        public IWebElement Items_Input_ItemCode { get; set; }
        [FindsBy(How = How.Id, Using = "WarrantyDate")]
        public IWebElement Items_Input_WarrantyDate { get; set; }
        [FindsBy(How = How.Id, Using = "Quantity")]
        public IWebElement Items_Input_Quantity { get; set; }

        //spans
        [FindsBy(How = How.Id, Using = "Name-error")]
        public IWebElement Items_Error_Name { get; set; }
        [FindsBy(How = How.Id, Using = "Type-error")]
        public IWebElement Items_Error_Type { get; set; }
        [FindsBy(How = How.Id, Using = "ItemCode-error")]
        public IWebElement Items_Error_ItemCode{ get; set; }
        [FindsBy(How = How.Id, Using = "Quantity-error")]
        public IWebElement Items_Error_Quantity { get; set; }

        //model errors
        [FindsBy(How = How.XPath, Using = "//li[contains(.,'Nie wybrano żadnego przedmiotu do przekazania')]")]
        public IWebElement Items_ErrorList_NoItemChecked { get; set; }
        [FindsBy(How = How.XPath, Using = "//li[contains(.,'jest w posiadaniu zewnętrznej firmy)]")]
        public IWebElement Items_ErrorList_ItemAssignedToExt { get; set; }
        [FindsBy(How = How.Id, Using = "modelErrors")]
        public IWebElement Items_ErrorList_Model { get; set; }

        //qrcode
        [FindsBy(How = How.Id, Using = "QrCode")]
        public IWebElement Items_Details_QRCode{ get; set; }


        public void GoToPage()
        {
            driver.Navigate().GoToUrl("https://localhost:44387");
            Thread.Sleep(500);
        }

        public void Wait(int milis) => Thread.Sleep(milis);
        public void SelectByText(string id, string text) => new SelectElement(driver.FindElement(By.Id(id))).SelectByText(text);
        public void WaitUntilElementIsClickable(WebDriverWait waiter, IWebElement ele) => waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(ele));
        public void WaitUntilPopUpDisappears(WebDriverWait waiter) => waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("(//span[contains(.,'×')])[1]")));





    }
}
