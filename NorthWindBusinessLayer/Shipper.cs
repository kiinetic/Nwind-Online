using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class Shipper
    {
        private int shipperID;
        private string companyName;

        public int ShipperID
        {
            get { return shipperID; }
        }

        public string CompanyName
        {
            get { return companyName; }
        }

        public Shipper(int ShipperID, string CompanyName)
        {
            shipperID = ShipperID;
            companyName = CompanyName;
        }
    }
}
