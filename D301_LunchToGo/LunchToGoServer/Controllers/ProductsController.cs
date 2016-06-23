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
    // Class that handles api calls
    public class ProductsController : ApiController
    {
        // DB connection string
        private string connectionString = "Data Source=sql.uict.nz;Initial Catalog=0849511;Integrated Security=True;Connect Timeout=15;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;MultipleActiveResultSets=True;";

        // Fetches info from database on GET call
        public IEnumerable<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            // Return product[]
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                Debug.WriteLine(conn.Database);
                conn.Open();

                SqlCommand s = new SqlCommand("SELECT * FROM [dbo].[ORDER]", conn);
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

                        products.Add(product);

                    }
                }
                reader.Close(); 
                

                foreach(Product product in products)
                {
                    // Setup meals for the product
                    s = new SqlCommand("SELECT * FROM [dbo].[MEAL] WHERE ORDERID=@ID", conn);
                    s.Parameters.Add(new SqlParameter("ID", product.ID));

                    reader = s.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            product.Meals.Add(new Meal()
                            {
                                Dish = reader.GetString(1),
                                Secondary = reader.GetString(2),
                                Price = (float)reader.GetDouble(4)
                            });
                        }
                    }
                   
                }

                reader.Close();
                    conn.Close();
            }

            return products;
        }

        // Handles data sent through in POST call
        [HttpPost]
        public HttpResponseMessage Post(Product p)
        {

            if (p != null)
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    int x = 0;

                    try
                    {
                        SqlCommand s = new SqlCommand("INSERT INTO [dbo].[ORDER] (CUSTOMERNAME,CUSTOMERPHONE,CUSTOMERADDRESS,CUSTOMERCITY,REGION,DELIVERYDATE,DELIVERYTIME,CREDITCARDNAME,CREDITCARDNUMBER,CREDITCARDCCV,CREDITCARDMONTH,CREDITCARDYEAR) VALUES (@name,@phone,@address,@city,@region,@deliveryDate,@deliveryTime,@creditCardName,@creditCardNumber,@creditCardCCV,@creditCardMonth,@creditCardYear)", conn);
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
                        s.ExecuteScalar();

                        SqlCommand sq = new SqlCommand("SELECT TOP 1 ID FROM [dbo].[ORDER] ORDER BY ID DESC", conn);
                        SqlDataReader reader = sq.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                x = reader.GetInt32(0);
                            }
                        }
                    }
                    catch
                    {

                    }

                    for (int i = 0; i < p.Meals.Count; i++)
                    {
                        try
                        {
                            SqlCommand sc = new SqlCommand("INSERT INTO [dbo].[MEAL] (DISH, SECONDARY, ORDERID, PRICE) VALUES (@dish,@secondary,@id,@price)", conn);
                            sc.Parameters.Add(new SqlParameter("dish", p.Meals[i].Dish));
                            sc.Parameters.Add(new SqlParameter("secondary", p.Meals[i].Secondary));
                            sc.Parameters.Add(new SqlParameter("id", x));
                            sc.Parameters.Add(new SqlParameter("price", p.Meals[i].Price));
                            sc.ExecuteScalar();
                        }
                        catch { }

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
