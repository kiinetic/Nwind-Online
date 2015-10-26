using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class CartItem
    {
        #region Fields

        private int productID;
        private decimal unitPrice;
        private int quantity;
        private string discountCode;
        #endregion

        #region Properties

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }

        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }

        public int Quantity
        {
            get { return quantity; }
            set { quantity = value; }
        }

        public string DiscountCode
        {
            get { return discountCode; }
            set { discountCode = value; }
        }

        #endregion

        public CartItem(int id, decimal price, int q)
        {
            productID = id;
            unitPrice = price;
            quantity = q;
        }
    }
}
