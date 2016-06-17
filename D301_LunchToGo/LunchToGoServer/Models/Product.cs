using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace LunchToGoServer.Models
{
    public class Product
    {
        public int ID { get; set; }
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public string Region { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerCity { get; set; }
        public string CreditCardName { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardCCV { get; set; }
        public string CreditCardMonth { get; set; }
        public string CreditCardYear { get; set; }
        public List<Meal> Meals { get; set; }

        public override string ToString()
        {
            string meals = "";
            if (Meals != null)
            {
                foreach (Meal m in Meals)
                {
                    meals += "\n" + m.ToString();
                }
            }
            return $"Delivery Date: {DeliveryDate}\nDelivery Time: {DeliveryTime}\nRegion: {Region}\nCustomer Name: {CustomerName}\nCustomer Phone: {CustomerPhone}\nCustomer Addr: {CustomerAddress}\nCustomer City: {CustomerCity}\nMeals: {meals}";

        }
    }

    public class Meal
    {
        public int MealID { get; set; }
        public int OrderID { get; set; }
        public string Dish { get; set; }
        public string Secondary { get; set; }
    }
}
