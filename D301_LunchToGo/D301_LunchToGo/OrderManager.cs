using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D301_LunchToGo
{
    public static class OrderManager
    {
        // Step two
        public static DateTime DeliveryDate { get; set; }
        public static string DeliveryTime { get; set; }

        // Step three
        public static string Region { get; set; }

        // Step four
        public static string CustomerName { get; set; }
        public static string CustomerPhone { get; set; }
        public static string CustomerAddress { get; set; }
        public static string CreditCardName { get; set; }
        public static string CreditCardNumber { get; set; }
        public static string CreditCardCCV { get; set; }
        public static string CreditCardMonth { get; set; }
        public static string CreditCardYear { get; set; }
        public static bool CreditCardValid { get; set; }
    }
}
