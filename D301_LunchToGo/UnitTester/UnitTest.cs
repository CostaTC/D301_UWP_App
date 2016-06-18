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

namespace UnitTester
{
    [TestClass]
    public class UnitTesters
    {
        SQLiteConnection conn;
        private string path;

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
            Meal m = new Meal("Toast", "Butter");
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

            Meal m = new Meal("Toast", "Butter");
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
