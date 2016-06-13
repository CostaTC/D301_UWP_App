using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using LunchToGoServer.Models;

namespace LunchToGoServer.Controllers
{
    public class ProductsController : ApiController
    {

        public IEnumerable<Product> GetAllProducts()
        {
            // CHANGE TO DATABASE STUFF IN HERE
            return new Product[]
        {
            new Product {
                ID = 1, CustomerName = "Tom",
                CustomerPhone = "355 5555",
                CustomerAddress = "123 Fake Street",
                CustomerCity = "Fake Town",
                DeliveryDate = DateTime.Now.ToString("dd/MM/yy"),
                DeliveryTime = "1pm - 2pm",
                Region = "Nether",
                CreditCardCCV = "123",
                CreditCardMonth = "08",
                CreditCardYear = "16",
                CreditCardName = "MR FAKE",
                CreditCardNumber = "1234 1234 1234 1234",
                Meals =  new List<Meal>
                {
                    new Meal  { Dish = "Toast", Secondary = "Butter" },
                    new Meal { Dish = "Cereal", Secondary = "Milk" },
                    new Meal { Dish = "WeetBix", Secondary = "Milk" }
                }
            },
            new Product {
                ID = 2, CustomerName = "Jack",
                CustomerPhone = "355 5555",
                CustomerAddress = "123 Fake Street",
                CustomerCity = "Fake Town",
                DeliveryDate = DateTime.Now.ToString("dd/MM/yy"),
                DeliveryTime = "1pm - 2pm",
                Region = "Nether",
                CreditCardCCV = "123",
                CreditCardMonth = "08",
                CreditCardYear = "16",
                CreditCardName = "MR FAKE",
                CreditCardNumber = "1234 1234 1234 1234",
                Meals =  new List<Meal>
                {
                    new Meal  { Dish = "Aids", Secondary = "Gays" },
                    new Meal { Dish = "Tom", Secondary = "Butter" },
                    new Meal { Dish = "WeetBix", Secondary = "Milk" }
                }
            },
            new Product {
                ID = 3, CustomerName = "Regi",
                CustomerPhone = "355 5555",
                CustomerAddress = "123 Fake Street",
                CustomerCity = "Fake Town",
                DeliveryDate = DateTime.Now.ToString("dd/MM/yy"),
                DeliveryTime = "1pm - 2pm",
                Region = "Nether",
                CreditCardCCV = "123",
                CreditCardMonth = "08",
                CreditCardYear = "16",
                CreditCardName = "MR FAKE",
                CreditCardNumber = "1234 1234 1234 1234",
                Meals =  new List<Meal>
                {
                    new Meal  { Dish = "Lamb", Secondary = "Jam" },
                    new Meal { Dish = "Cereal", Secondary = "Milk" },
                    new Meal { Dish = "WeetBix", Secondary = "Milk" }
                }
            },

        };
        }

        public HttpResponseMessage Post([FromBody]string value)
        {
            return Request.CreateResponse(HttpStatusCode.OK, "Success");
        }

        //public Product SingleProduct(int id)
        //{
        //    return products.FirstOrDefault((p) => p.ID == id);
        //}

    }
}
