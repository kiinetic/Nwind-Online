using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NorthWindBusinessLayer
{
    public class Product
    {
        #region Fields

        private int productID;
        private string productName;
        private int categoryID;
        private string quantityPerUnit;
        private decimal unitPrice;
        private string filename;
        private int unitsInStock;

        #endregion

        #region Properties

        public int ProductID
        {
            get { return productID; }
            set { productID = value; }
        }
        public string ProductName
        {
            get { return productName; }
            set { productName = value; }
        }
        public int CategoryID
        {
            get { return categoryID; }
            set { categoryID = value; }
        }
        public string QuantityPerUnit
        {
            get { return quantityPerUnit; }
            set { quantityPerUnit = value; }
        }
        public decimal UnitPrice
        {
            get { return unitPrice; }
            set { unitPrice = value; }
        }
        public int UnitsInStock
        {
            get { return unitsInStock; }
            set { unitsInStock = value; }
        }

        #endregion

        public Product(int id, string name, int cid, string qpu, decimal price, int unitsinstock, string fn)
        {
            productID = id;
            productName = name;
            categoryID = cid;
            quantityPerUnit = qpu;
            unitPrice = price;
            unitsInStock = unitsinstock;
            filename = fn;
        }
    }

}
