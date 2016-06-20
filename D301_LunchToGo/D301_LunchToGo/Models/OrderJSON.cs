using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace D301_LunchToGo.Models
{
    // Class to convert order details into JSON
    public class OrderJSON
    {
        [JsonProperty("ID")]
        public int ID { get; set; }
        [JsonProperty("DeliveryDate")]
        public string DeliveryDate { get; set; }
        [JsonProperty("DeliveryTime")]
        public string DeliveryTime { get; set; }
        [JsonProperty("Region")]
        public string Region { get; set; }
        [JsonProperty("CustomerName")]
        public string CustomerName { get; set; }
        [JsonProperty("CustomerPhone")]
        public string CustomerPhone { get; set; }
        [JsonProperty("CustomerAddress")]
        public string CustomerAddress { get; set; }
        [JsonProperty("CustomerCity")]
        public string CustomerCity { get; set; }
        [JsonProperty("CreditCardName")]
        public string CreditCardName { get; set; }
        [JsonProperty("CreditCardNumber")]
        public string CreditCardNumber { get; set; }
        [JsonProperty("CreditCardCCV")]
        public string CreditCardCCV { get; set; }
        [JsonProperty("CreditCardMonth")]
        public string CreditCardMonth { get; set; }
        [JsonProperty("CreditCardYear")]
        public string CreditCardYear { get; set; }
        [JsonProperty("Meals")]
        public List<MealJSON> Meals { get; set; }
    }

    // Class to convert meal details into JSON
    public class MealJSON
    {
        [JsonProperty("MealID")]
        public int MealID { get; set; }
        [JsonProperty("OrderID")]
        public int OrderID { get; set; }
        [JsonProperty("Dish")]
        public string Dish { get; set; }
        [JsonProperty("Secondary")]
        public string Secondary { get; set; }
        [JsonProperty("Price")]
        public float Price { get; set; }
    }
}
