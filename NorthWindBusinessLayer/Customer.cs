using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class Customer
    {
        #region Fields

        private string customerID;
        private string companyName;
        private string address;
        private string city;
        private string region;
        private string postalCode;
        private string country;
        private string pword;

        #endregion

        #region Properties

        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public string CompanyName
        {
            get { return companyName; }
            set { companyName = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string City
        {
            get { return city; }
            set { city = value; }
        }

        public string Region
        {
            get { return region; }
            set { region = value; }
        }

        public string PostalCode
        {
            get { return postalCode; }
            set { postalCode = value; }
        }

        public string Country
        {
            get { return country; }
            set {country = value;}
        }


        public string PWord
        {
            get { return pword; }
        }

        #endregion

        public Customer(string cid, string cname, string ad, string cty, string reg, string pc, string cntry, string pw)
        {
            customerID = cid;
            companyName = cname;
            address = ad;
            city = cty;
            region = reg;
            postalCode = pc;
            country = cntry;
            pword = pw;
        }
    }
}
