using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite.Net;
using SQLite.Net.Attributes;

namespace D301_LunchToGo
{

    public class CustomerDetailsDB
    {
        //public CustomerDetailsDB (string name, string phone, string addr, string city){
        //    CustName = name;
        //    CustPhone = phone;
        //    CustAddr = addr;
        //    CustCity = city;
        //}

        public string CustName { get; set; }
        public string CustPhone { get; set; }
        public string CustAddr { get; set; }
        public string CustCity { get; set; }
    }
}
