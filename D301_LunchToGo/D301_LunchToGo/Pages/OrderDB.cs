using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace D301_LunchToGo
{
    public class OrderDB
    {
        //public OrderDB()
        //{
        //    DeliveryDate = OrderManager.DeliveryDate;
        //    DeliveryTime = OrderManager.DeliveryTime;
        //    Region = OrderManager.Region;
        //    CustomerName = OrderManager.CustomerName;
        //    CustomerPhone = OrderManager.CustomerPhone;
        //    CustomerAddress = OrderManager.CustomerAddress;
        //    CustomerCity = OrderManager.CustomerCity;
        //    CreditCardName = OrderManager.CreditCardName;
        //    CreditCardNumber = OrderManager.CreditCardNumber;
        //    CreditCardCCV = OrderManager.CreditCardCCV;
        //    CreditCardMonth = OrderManager.CreditCardMonth;
        //    CreditCardYear = OrderManager.CreditCardYear;
        //    Meals = OrderManager.Meals;
        //}

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

    public class MealDB
    {
        [PrimaryKey]
        public int OrderID { get; set; }
        public string Dish { get; set; }
        public string Secondary { get; set; }
    }

    }
