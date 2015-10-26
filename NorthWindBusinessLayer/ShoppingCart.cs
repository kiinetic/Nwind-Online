using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class ShoppingCart
    {
        #region Fields

        private string customerID;
        private DateTime creationDate;
        private DateTime latestUpdateDate;
        private string status;
        private ShippingAddress shippingAddress;
        List<CartItem> cart = new List<CartItem>();

        #endregion

        public struct ShippingAddress
        {
            public DateTime OrderDate;
            public string City;
            public string Shipname;
            public string Address;
            public string Region;
            public string PostalCode;
            public string Country;
        }

        #region Properties

        public string CustomerID
        {
            get { return customerID; }
            set { customerID = value; }
        }

        public DateTime CreationDate
        {
            get { return creationDate; }
        }

        public DateTime LatestUpdateDate
        {
            get { return LatestUpdateDate; }
            set { latestUpdateDate = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        public List<CartItem> Cart 
        {
            get { return cart; }
        }
        public ShippingAddress ShippingInformation
        {
            get { return shippingAddress; }
            set { shippingAddress = value; }
        }

        #endregion

        public ShoppingCart(string cid)
        {
            customerID = cid;
            creationDate = DateTime.Now;
        }

        public void PlaceOrder()
        {
        }

        public void RemoveItem(CartItem item)
        {
            cart.Remove(item);
        }
        public void AddToCart(CartItem item)
        {
            cart.Add(item);
        }

        public void EmptyCart()
        {
            cart.Clear();
        }
    }
}
