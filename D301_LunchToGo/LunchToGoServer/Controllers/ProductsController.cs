using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchToGoServer.Models;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;


namespace LunchToGoServer.Controllers
{
    public class ProductsController : ApiController
    {
       
        private string connectionString = "Data Source=(LocalDB)\\MSSQLLocalDB;" + "Initial Catalog=Orders.mdf;" + "Integrated Security=False;";

        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            // Return product[]
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand s = new SqlCommand("SELECT * FROM [dbo].[Order]", conn);
                SqlDataReader reader = s.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        int i = 0;
                        // Setup product
                        Product product = new Product()
                        {
                            ID = reader.GetInt32(i++),
                            CustomerName = reader.GetString(i++),
                            CustomerPhone = reader.GetString(i++),
                            CustomerAddress = reader.GetString(i++),
                            CustomerCity = reader.GetString(i++),
                            Region = reader.GetString(i++),
                            DeliveryDate = reader.GetString(i++),
                            DeliveryTime = reader.GetString(i++),
                            CreditCardName = reader.GetString(i++),
                            CreditCardNumber = reader.GetString(i++),
                            CreditCardCCV = reader.GetString(i++),
                            CreditCardMonth = reader.GetString(i++),
                            CreditCardYear = reader.GetString(i++),
                            Meals = new List<Meal>()
                        };

                    }
                }      

                foreach(Product product in products)
                {
                    // Setup meals for the product
                    s = new SqlCommand("SELECT * FROM Meal WHERE OrderID=@ID", conn);
                    s.Parameters.Add(new SqlParameter("ID", product.ID));

                    SqlDataReader r = s.ExecuteReader();
                    if (r.HasRows)
                    {
                        while (r.Read())
                        {
                            product.Meals.Add(new Meal()
                            {
                                Dish = r.GetString(1),
                                Secondary = r.GetString(2)
                            });
                        }
                    }
                   
                }


                    conn.Close();
            }

            return products;
        }

        [HttpPost]
        public HttpResponseMessage Post(Product p)
        {

            if (p != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();
                    
                    SqlCommand s = new SqlCommand("INSERT INTO [dbo].[Order] (CustomerName,CustomerPhone,CustomerAddress,CustomerCity,Region,DeliveryDate,DeliveryTime,CreditCardName,CreditCardNumber,CreditCardCCV,CreditCardMonth,CreditCardYear) VALUES (@name,@phone,@address,@city,@region,@deliveryDate,@deliveryTime,@creditCardName,@creditCardNumber,@creditCardCCV,@creditCardMonth,@creditCardYear)", conn);
                    s.Parameters.Add(new SqlParameter("name", p.CustomerName));
                    s.Parameters.Add(new SqlParameter("phone", p.CustomerPhone));
                    s.Parameters.Add(new SqlParameter("address", p.CustomerAddress));
                    s.Parameters.Add(new SqlParameter("city", p.CustomerCity));
                    s.Parameters.Add(new SqlParameter("region", p.Region));
                    s.Parameters.Add(new SqlParameter("deliveryDate", p.DeliveryDate));
                    s.Parameters.Add(new SqlParameter("deliveryTime", p.DeliveryTime));
                    s.Parameters.Add(new SqlParameter("creditCardName", p.CreditCardName));
                    s.Parameters.Add(new SqlParameter("creditCardNumber", p.CreditCardNumber));
                    s.Parameters.Add(new SqlParameter("creditCardCCV", p.CreditCardCCV));
                    s.Parameters.Add(new SqlParameter("creditCardMonth", p.CreditCardMonth));
                    s.Parameters.Add(new SqlParameter("creditCardYear", p.CreditCardYear));
                    int x = (Int32)s.ExecuteScalar();

                    foreach (Meal m in p.Meals)
                    {
                        SqlCommand sc = new SqlCommand("INSERT INTO Meal (Dish, Secondary, OrderID) VALUES (@dish,@secondary,@id)", conn);
                        sc.Parameters.Add(new SqlParameter("dish", m.Dish));
                        sc.Parameters.Add(new SqlParameter("secondary", m.Secondary));
                        sc.Parameters.Add(new SqlParameter("id", x));
                        sc.ExecuteScalar();
                    }

                    conn.Close();
                }
                return Request.CreateResponse(HttpStatusCode.OK, p);
            }
                
            else
                return Request.CreateResponse(HttpStatusCode.OK, "null");

        }

    }
}
