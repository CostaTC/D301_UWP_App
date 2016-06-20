using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace D301_LunchToGo.Models
{
    // Class for storing order details in to db
    public class OrderDB
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public DateTime DeliveryDate { get; set; }
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
    }

    // Class for storing meal details in to db
    public class MealDB
    {
        [PrimaryKey, AutoIncrement]
        public int MealID { get; set; }
        public int OrderID { get; set; }
        public string Dish { get; set; }
        public string Secondary { get; set; }
        public float Price { get; set; }
    }

    }
