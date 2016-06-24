using System;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using D301_LunchToGo;
using SQLite.Net;
using Windows.Storage;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using Windows.Foundation;
using Windows.Foundation.Collections;
using D301_LunchToGo.Models;

namespace UnitTester
{
    [TestClass]
    public class UnitTesters
    {
        SQLiteConnection conn;
        private string path;

        /// <summary>
        /// Tests the credit card checker should work
        /// </summary>
        [TestMethod]
        public void RealCreditCard()
        {
            bool ccValid = false; 

            string input = "4444432114321432";

            string[] Patterns = new string[] { "4[0-9]{12}(?:[0-9]{3})?", "5[1-5][0-9]{14}", "3[47][0-9]{13}", "3(?:0[0-5]|[68][0-9])[0-9]{11}", "6(?:011|5[0-9]{2})[0-9]{12}", "(?:2131|1800|35\\d{3})\\d{11}" };

            foreach (var pattern in Patterns)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.Multiline))
                {
                    ccValid = true;
                }
            }

            Assert.AreEqual(true, ccValid);
        }

        /// <summary>
        /// Tests the credit card checker should not work
        /// </summary>
        [TestMethod]
        public void FakeCreditCard()
        {
            bool ccValid = false;

            string input = "1111111111111111";

            string[] Patterns = new string[] { "4[0-9]{12}(?:[0-9]{3})?", "5[1-5][0-9]{14}", "3[47][0-9]{13}", "3(?:0[0-5]|[68][0-9])[0-9]{11}", "6(?:011|5[0-9]{2})[0-9]{12}", "(?:2131|1800|35\\d{3})\\d{11}" };

            foreach (var pattern in Patterns)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(input, pattern, System.Text.RegularExpressions.RegexOptions.Multiline))
                {
                    ccValid = true;
                }
            }

            Assert.AreEqual(false, ccValid);
        }

        /// <summary>
        /// Checks whether registering account correctly goes to database
        /// </summary>
        [TestMethod]
        public async void RegisterAccount()
        {
            // Setup DB
            path = Path.Combine(ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            StorageFolder storageFolder = ApplicationData.Current.LocalFolder;
            StorageFile notesFile = await storageFolder.CreateFileAsync("db.sqlite", CreationCollisionOption.OpenIfExists);
            conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Error Check
            conn.CreateTable<CustomerDetailsDB>();
            try
            {
                conn.DeleteAll<CustomerDetailsDB>();
            }
            catch (Exception) { }

            // Create test data to store
            conn.CreateTable<CustomerDetailsDB>();
            var s = conn.Insert(new CustomerDetailsDB()
            {
                CustName = "Test",
                CustPhone = "Phone",
                CustAddr = "Addr",
                CustCity = "City"
            });

            // Pull back out of Database and check data is the same
            List<CustomerDetailsDB> cList = conn.Query<CustomerDetailsDB>("Select * from CustomerDetailsDB ORDER BY CustName ASC LIMIT 1");

            Assert.AreEqual("Test",cList.First().CustName);
        }

        /// <summary>
        /// Checks Delivery Time correctly adds to OrderManager
        /// </summary>
        [TestMethod]
        public void DeliveryTime()
        {
            string delTime = "1:10 - 2:00";
            OrderManager.DeliveryTime = delTime;
            Assert.AreEqual("1:10 - 2:00", OrderManager.DeliveryTime);
        }

        /// <summary>
        /// Checks meal correctly adds to OrderManager
        /// </summary>
        [TestMethod]
        public void MealOptions()
        {
            Meal m = new Meal("Toast", "Butter", 1);
            OrderManager.AddMeal(m);
            Assert.AreEqual("Toast", OrderManager.Meals.First().Dish);
        }

        /// <summary>
        /// Checks Internet Connection and that cart contains Meals
        /// </summary>
        [TestMethod]
        public void CanOrderMeal()
        {
            bool result = false;

            Meal m = new Meal("Toast", "Butter",1);
            OrderManager.AddMeal(m);

            // If the user has items in the cart then they can place the order else return false
            if (OrderManager.Meals != null && OrderManager.Meals.Count > 0 && NetworkConnectionTrigger.HasInternet())
                result = true;

            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Checks Delivery Region correctly adds to OrderManager
        /// </summary>
        [TestMethod]
        public void DeliveryRegion()
        {
            string delRegion = "Manawatu";
            OrderManager.DeliveryTime = delRegion;
            Assert.AreEqual("Manawatu", OrderManager.DeliveryTime);
        }
    }
}
