using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace D301_LunchToGo.Models
{
    /// <summary>
    /// Static class that holds information the user inputs during the order process
    /// </summary>
    public static class OrderManager
    {
        // Properties that hold the data for the Delivery Section - Step two
        public static DateTime DeliveryDate { get; set; }
        public static string DeliveryTime { get; set; }

        // Property that holds the user selected region - Step three
        public static string Region { get; set; }

        // Properties for the Customer Registration -  Step four
        public static string CustomerName { get; set; }
        public static string CustomerPhone { get; set; }
        public static string CustomerAddress { get; set; }
        public static string CustomerCity { get; set; }
        public static string CreditCardName { get; set; }
        public static string CreditCardNumber { get; set; }
        public static string CreditCardCCV { get; set; }
        public static string CreditCardMonth { get; set; }
        public static string CreditCardYear { get; set; }
        public static bool CreditCardValid { get; set; }
        public static bool RememberDetails { get; set; }

        // Property that holds the meals that the user has ordered Step 5
        public static List<Meal> Meals { get; set; }

        /// <summary>
        /// Adds meal to the Meals list whilst also null checking
        /// </summary>
        /// <param name="meal">Object to be added</param>
        public static void AddMeal(Meal meal)
        {
            // Convert meal name
            Meal currentMeal = new Meal(meal.Dish, meal.Secondary, meal.Price);
            currentMeal.ConvertName();

            // Null check the list - initialize if it's empty
            if(Meals != null)
                Meals.Add(currentMeal);
            else
            {
                Meals = new List<Meal>();
                Meals.Add(currentMeal);
            }
        }

        /// <summary>
        /// Clears Meals List with a null check
        /// </summary>
        public static void ClearMeals()
        {
            if (Meals != null)
                Meals.Clear();
        }

        /// <summary>
        /// Puts order details into a formatted string
        /// </summary>
        /// <returns>Formatted string of the order details</returns>
        public static string OrderDetails()
        {
            string meals = "";
            if(Meals != null)
            {
                foreach(Meal m in Meals)
                {
                    meals += "\n" + m.ToString();
                }
            }
            return $"Delivery Date: {DeliveryDate.Date}\nDelivery Time: {DeliveryTime}\nRegion: {Region}\nCustomer Name: {CustomerName}\nCustomer Phone: {CustomerPhone}\nCustomer Addr: {CustomerAddress}\nCustomer City: {CustomerCity}\nCredit Card Valid: {CreditCardValid}\nMeals: {meals}";
        }
    }

    /// <summary>
    /// Class that holds information about the meal
    /// </summary>
    public class Meal
    {
        // Properties that reference the dish and the secondary choice that goes with it
        public string Dish { get; set; }
        public string Secondary { get; set; }
        public float Price { get; set; }

        /// <summary>
        /// Class Constructor
        /// </summary>
        /// <param name="dish">Main Dish</param>
        /// <param name="secondary">Secondary option for the dish</param>
        public Meal(string dish, string secondary, float price)
        {
            Dish = dish;
            Secondary = secondary;
            Price = price;
        }

        /// <summary>
        /// Converts the dish name e.g. "imgBeefNoodle" to "Beef Noodle"
        /// </summary>
        public void ConvertName()
        {
            if (Dish.Contains("img"))
                Dish = Dish.Remove(0, 3);

            switch (Dish)
            {
                case "BeefNoodle":
                    Dish = "Beef Noodle";
                    break;
                case "ChickenSandwich":
                    Dish = "Chicken Sandwich";
                    break;
                case "GreenSalad":
                    Dish = "Green Salad";
                    break;
                case "LambKorma":
                    Dish = "Lamb Korma";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// To string override
        /// </summary>
        /// <returns>New string</returns>
        public override string ToString()
        {
            if (Dish.Contains("img"))
                ConvertName();

            return "$" + Price + "\n" + Dish + "\n(" + Secondary + ")";
        }

    }
}
