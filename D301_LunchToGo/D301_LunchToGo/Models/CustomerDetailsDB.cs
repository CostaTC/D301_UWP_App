using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace D301_LunchToGo.Models
{
    // Stores customer details in DB
    public class CustomerDetailsDB
    {
        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string CustAddr { get; set; }
        public string CustCity { get; set; }
    }
}
