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
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace DotNetWMSTests
{
    [TestFixture]
    public class DotNetWMSTests_Selenium_Items
    {

        private IWebDriver driver;
        private ItemPage page;
        private WebDriverWait waiter;
        
        [SetUp]
        public void SetupTest()
        {
            driver = new ChromeDriver();
            page = new ItemPage(driver);
            waiter = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            
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
                
            }
        }
        [Test]
        public void Items_CreateNewItem_Success()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest();

            try
            {
                page.WaitUntilPopUpDisappears(waiter);
                assertVal = page.WelcomeText.Displayed;
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
           
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

            

        }
        [Test]
        public void Items_CreateNewItemButWithErrors_Fail()
        {
            Login();
            page.GoToPage();
            page.Items_Button_Overview.Click();
            page.Items_Button_AddNewItem.Click();
            page.Items_Input_Producer.SendKeys("AAAProducer");
            page.Items_Input_Name.SendKeys(DateTime.Now.ToString());
            page.Items_Input_Type.SendKeys("AAAType");
            page.Items_Input_Model.SendKeys("AAAModel");
            page.Items_Input_ItemCode.SendKeys("AAAItemCode");
            page.Items_Input_WarrantyDate.SendKeys("01.01.2021");
            page.Items_Input_Quantity.SendKeys("a");
            page.SelectByText("Units", "kg");
            page.Items_Button_Create_Submit.Click();

            Assert.IsTrue(page.Items_Error_Quantity.Displayed);

        }
        [Test]
        public void Items_CreateNewItemButWithEmptyForm_Fail()
        {
            Login();
            page.GoToPage();

            page.Items_Button_Overview.Click();
            page.Items_Button_AddNewItem.Click();
            page.Items_Button_Create_Submit.Click();

            Assert.IsTrue(page.Items_Error_Name.Text.Contains("jest wymagane!"));
            Assert.IsTrue(page.Items_Error_Type.Text.Contains("jest wymagane!"));
            Assert.IsTrue(page.Items_Error_ItemCode.Text.Contains("jest wymagane!"));
            Assert.IsTrue(page.Items_Error_Quantity.Text.Contains("jest wymagane!"));

        }
        [Test]
        public void Items_AssignItemToUser_Success()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest();

            try
            {
                page.Items_Button_Overview.Click();
                page.Items_Button_Assign.Click();
                page.Items_Checkbox_IsChecked.Click();
                page.Items_Button_Assign_To.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Assign_ToUser);
                page.Items_Button_Assign_ToUser.Click();
                page.SelectByText("UserId", "Nowy Józef");
                page.Items_Button_Assign_Submit.Click();
                page.WaitUntilPopUpDisappears(waiter);
                page.GoToPage();
                page.Items_Button_Overview.Click();
                assertVal = page.WelcomeText.Displayed;
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
            
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

        }
        [Test]
        public void Items_AssignItemToUserButNoItemChecked_Fail()
        {

            Login();
            page.GoToPage();

            page.Items_Button_Overview.Click();
            page.Items_Button_Assign.Click();
            page.Items_Button_Assign_To.Click();
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_Assign_ToUser);
            page.Items_Button_Assign_ToUser.Click();

            Assert.IsTrue(page.Items_ErrorList_NoItemChecked.Displayed);

        }
        [Test]
        public void Items_AssignItemToUserButWhenItemWasAssignedToExt_Fail()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest("ExternalId");

            try
            {
                page.Items_Button_Overview.Click();
                page.Items_Button_Assign.Click();
                page.Items_Checkbox_IsChecked.Click();
                page.Items_Button_Assign_To.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Assign_ToUser);
                page.Items_Button_Assign_ToUser.Click();
                assertVal = page.Items_ErrorList_Model.Displayed;
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
            
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

        }
        [Test]
        public void Items_AssignItemToUserButWhenItemWasAssignedToSameUser_Fail()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest("UserId");

            try
            {
                page.Items_Button_Overview.Click();
                page.Items_Button_Assign.Click();
                page.Items_Checkbox_IsChecked.Click();
                page.Items_Button_Assign_To.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Assign_ToUser);
                page.Items_Button_Assign_ToUser.Click();
                page.SelectByText("UserId", "Nowy Józef");
                page.Items_Button_Assign_Submit.Click();
                assertVal = page.Items_ErrorList_Model.Displayed;
                //page.GoToPage();
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
            
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

        }
        [Test]
        public void Items_AssignItemToUserButWhenItemWasAssignedToSameWarehouse_Fail()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest("WarehouseId");

            try
            {
                page.Items_Button_Overview.Click();
                page.Items_Button_Assign.Click();
                page.Items_Checkbox_IsChecked.Click();
                page.Items_Button_Assign_To.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Assign_ToWarehouse);
                page.Items_Button_Assign_ToWarehouse.Click();
                page.SelectByText("WarehouseId", "Magazyn główny, Myśliwska 61");
                page.Items_Button_Assign_Submit.Click();
                assertVal = page.Items_ErrorList_Model.Displayed;
                //page.GoToPage();
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
            
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

        }
        [Test]
        public void Items_AssignItemToUserButWhenItemWasAssignedToSameExternal_Fail()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest("ExternalId");

            try
            {
                page.Items_Button_Overview.Click();
                page.Items_Button_Assign.Click();
                page.Items_Checkbox_IsChecked.Click();
                page.Items_Button_Assign_To.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Assign_ToExternal);
                page.Items_Button_Assign_ToExternal.Click();
                page.SelectByText("ExternalId", "FIAT AUTO KRAK, 6771173032");
                page.Items_Button_Assign_Submit.Click();
                assertVal = page.Items_ErrorList_Model.Displayed;
                //page.GoToPage();
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
           
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

        }
        [Test]
        public void Items_ItemDetailIsQRCodeGenerated_Success()
        {
            bool assertVal;

            Login();
            page.GoToPage();
            CreateItemForTest();

            try
            {
                page.Items_Button_Overview.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Index_More_1);
                page.Items_Button_Index_More_1.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Index_Details_1);
                page.Items_Button_Index_Details_1.Click();
                page.WaitUntilElementIsClickable(waiter, page.Items_Button_Details_PrintQrCode);
                assertVal = page.Items_Details_QRCode.Displayed;
                //page.GoToPage();
            }
            catch (Exception e)
            {
                DeleteItemAfterTest();
                throw e;
            }
            
            DeleteItemAfterTest();

            Assert.IsTrue(assertVal);

        }

        private void Login()
        {
            LoginPage loginPage = new LoginPage(driver);
            loginPage.GoToPage();
            loginPage.UserName.SendKeys("wlasceug0006");
            loginPage.Password.SendKeys("Test123!");
            if (loginPage.Cookie.Displayed)
            {
                loginPage.Cookie.Click();
            }
            loginPage.Submit.Click();

        }

        private void CreateItemForTest(string req = "")
        {
            page.Items_Button_Overview.Click();
            //page.WaitUntilElementIsVisible(waiter, "//a[@href='/Items/Create']");
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_AddNewItem);
            page.Items_Button_AddNewItem.Click();
            //page.WaitUntilElementIsVisible(waiter, "//h2[contains(.,'Dodawanie produktu')]");
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_Create_Submit);
            page.Items_Input_Producer.SendKeys("AAAProducer");
            page.Items_Input_Name.SendKeys(DateTime.Now.ToString());
            page.Items_Input_Type.SendKeys("AAAType");
            page.Items_Input_Model.SendKeys("AAAModel");
            page.Items_Input_ItemCode.SendKeys("AAAItemCode");
            page.Items_Input_WarrantyDate.SendKeys("01.01.2021");
            page.Items_Input_Quantity.SendKeys("1");
            page.SelectByText("Units", "kg");

            if (req == "UserId")
            {
                page.SelectByText("UserId", "Nowy Józef");
            }
            if (req == "WarehouseId")
            {
                page.SelectByText("WarehouseId", "Magazyn główny, Myśliwska 61");
            }
            if (req == "ExternalId")
            {
                page.SelectByText("ExternalId", "FIAT AUTO KRAK");
            }

            page.Items_Button_Create_Submit.Click();
            //page.WaitUntilPageElement(waiter, "//h2[contains(.,'Przegląd majątku')]");
            //page.WaitUntilElementIsClickable(waiter, page.Items_Button_Create_Submit);
            //waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("(//span[contains(.,'×')])[1]")));
            page.WaitUntilPopUpDisappears(waiter);
        }

        private void DeleteItemAfterTest()
        {
            //Thread.Sleep(1000);
            //waiter.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.InvisibilityOfElementLocated(By.XPath("(//span[contains(.,'×')])[1]")));
            page.GoToPage();
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_Overview);
            page.Items_Button_Overview.Click();
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_Index_More_1);
            page.Items_Button_Index_More_1.Click();
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_Index_Delete_1);
            page.Items_Button_Index_Delete_1.Click();
            page.WaitUntilElementIsClickable(waiter, page.Items_Button_Index_Alert_Yes_1);
            page.Items_Button_Index_Alert_Yes_1.Click();
        }



    }
}
