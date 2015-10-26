using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class Order
    {
        private int orderID;
        private string customerID;
        private DateTime orderDate;
        private DateTime? requiredDate;
        private DateTime? shippedDate;
        private int? shipper;

        public int OrderID
        {
            get { return orderID; }
        }

        public string CustomerID 
        {
            get { return customerID; }
        }

        public DateTime OrderDate
        {
            get { return orderDate; }
        }

         public DateTime? RequredDate
        {
            get { return requiredDate; }
        }

        public DateTime? ShippedDate
        {
            get { return shippedDate; }
        }

        public int? Shipper
        {
            get { return shipper; }
        }

        public Order(int OrderID, string CustomerID, DateTime OrderDate, DateTime? RequiredDate, DateTime? ShippedDate, int? Shipper)
        {
            orderID = OrderID;
            customerID = CustomerID;
            orderDate = OrderDate;
            requiredDate = RequiredDate;
            shippedDate = ShippedDate;
            shipper = Shipper;
        }
    }
}
