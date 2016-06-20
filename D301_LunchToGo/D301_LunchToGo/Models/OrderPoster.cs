using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Windows.Web.Http;
using Newtonsoft.Json;
using System.Net.Http;
using Windows.Web;
using SQLite.Net;
using SQLite.Net.Attributes;
using System.Diagnostics;

namespace D301_LunchToGo.Models
{
    // Static class that posts order to webserver
    public static class OrderPoster
    {
        // Posts OrderJSON to webserver
        public async static Task<bool> SendOrder()
        {
            // Setup db for retrieval
            string path = Path.Combine(Windows.Storage.ApplicationData.Current.LocalFolder.Path, "db.sqlite");
            SQLiteConnection conn = new SQLiteConnection(new SQLite.Net.Platform.WinRT.SQLitePlatformWinRT(), path);

            // Get order from db
            List<OrderDB> cList = conn.Query<OrderDB>("Select * from OrderDB ORDER BY ID ASC LIMIT 1");
            OrderDB o = cList.First();
            List<MealDB> mdb = conn.Query<MealDB>("Select * from MealDB");

            // Convert meals to json
            List<MealJSON> mJson = new List<MealJSON>();

            foreach(MealDB m in mdb)
            {
                mJson.Add( new MealJSON
                {
                    MealID = m.MealID,
                    OrderID = m.OrderID,
                    Dish = m.Dish,
                    Secondary = m.Secondary,
                    Price = m.Price
                });
            }
            

            var payload = new OrderJSON
            {
                ID = o.ID,
                CustomerName = o.CustomerName,
                CustomerPhone = o.CustomerPhone,
                CustomerAddress = o.CustomerAddress,
                CustomerCity = o.CustomerCity,
                DeliveryDate = o.DeliveryDate.ToString("dd/MM/yy"),
                DeliveryTime = o.DeliveryTime,
                Region = o.Region,
                CreditCardCCV = o.CreditCardCCV,
                CreditCardMonth = o.CreditCardMonth,
                CreditCardYear = o.CreditCardYear,
                CreditCardName = o.CreditCardName,
                CreditCardNumber = o.CreditCardNumber,
                Meals = mJson
            };

            // Serialize our concrete class into a JSON String
            var stringPayload = await Task.Run(() => JsonConvert.SerializeObject(payload));

            // Wrap our JSON inside a StringContent which then can be used by the HttpClient class
            var httpContent = new StringContent(stringPayload, Encoding.UTF8, "application/json");

            using (var httpClient = new System.Net.Http.HttpClient())
            {
                Debug.WriteLine(payload);
                Debug.WriteLine(stringPayload);
                Debug.WriteLine(httpContent.ToString());
                // Do the actual request and await the response
                var httpResponse = await httpClient.PostAsync("http://localhost:29102/api/products", httpContent);

                // If the response contains content we want to read it!
                if (httpResponse.Content != null)
                {
                    var responseContent = await httpResponse.Content.ReadAsStringAsync();
                    Debug.WriteLine("RESPONSE STRING----------:");
                    Debug.WriteLine(responseContent);
                    if (responseContent.ToString() != "\"null\"")
                        return true;
                    else
                        return false;
                }
                else
                    return false;

            }
            
        }
    }
 }
