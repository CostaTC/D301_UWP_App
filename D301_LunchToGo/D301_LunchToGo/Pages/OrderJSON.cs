using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace D301_LunchToGo
{
    public class OrderJSON
    {
        [JsonProperty("id")]
        public int ID { get; set; }
        [JsonProperty("deliveryDate")]
        public string DeliveryDate { get; set; }
        [JsonProperty("deliveryTime")]
        public string DeliveryTime { get; set; }
        [JsonProperty("region")]
        public string Region { get; set; }
        [JsonProperty("name")]
        public string CustomerName { get; set; }
        [JsonProperty("phone")]
        public string CustomerPhone { get; set; }
        [JsonProperty("address")]
        public string CustomerAddress { get; set; }
        [JsonProperty("city")]
        public string CustomerCity { get; set; }
        [JsonProperty("creditCardName")]
        public string CreditCardName { get; set; }
        [JsonProperty("creditCardNumber")]
        public string CreditCardNumber { get; set; }
        [JsonProperty("creditCardCCV")]
        public string CreditCardCCV { get; set; }
        [JsonProperty("creditCardMonth")]
        public string CreditCardMonth { get; set; }
        [JsonProperty("creditCardYear")]
        public string CreditCardYear { get; set; }
        [JsonProperty("meals")]
        public List<MealJSON> Meals { get; set; }
    }

    public class MealJSON
    {
        [JsonProperty("id")]
        public int MealID { get; set; }
        [JsonProperty("orderID")]
        public int OrderID { get; set; }
        [JsonProperty("dish")]
        public string Dish { get; set; }
        [JsonProperty("secondary")]
        public string Secondary { get; set; }
    }
}
