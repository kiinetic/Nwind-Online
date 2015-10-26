using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using DBLoveleenJames;

namespace NorthWindBusinessLayer
{
    public class BusinessLayer
    {
        //private static List<Product> masterProductList = null;
        private static List<Product> categoryList = null;
        public static List<Order> OrderList = new List<Order>();
        public static ShoppingCart CurrentCart;
        public static Customer CurrentUser;


        //public static List<Product> MasterProductList //Returns All Products
        //{

        //    get
        //    {
        //        if (masterProductList == null)
        //        {
        //            masterProductList = new List<Product>();
        //            DataTable dt = DB.GetProductTable();
        //            foreach (DataRow row in dt.Rows)
        //            {
        //                Product P = new Product((int)row["ProductID"], (string)row["ProductName"], (int)row["CategoryID"], (string)row["QuantityPerUnit"], (decimal)row["UnitPrice"], (int)(short)row["UnitsInStock"], (string)row["PictureFileName"]);
        //                masterProductList.Add(P);
        //            }
        //        }
        //        return masterProductList;
        //    }
        //}

        public static List<Product> CategoryProductList(int CategoryID) // Returns all products from a given category
        {

            if (categoryList == null)
            {
                categoryList = new List<Product>();
                DataTable dt = DB.GetProductTable(CategoryID);
                foreach (DataRow row in dt.Rows)
                {
                    Product P = new Product((int)row["ProductID"], (string)row["ProductName"], (int)row["CategoryID"], (string)row["QuantityPerUnit"], (decimal)row["UnitPrice"], (int)(short)row["UnitsInStock"], row["PictureFileName"] == DBNull.Value ? null : (string)row["PictureFileName"]);
                    categoryList.Add(P);
                }
            }
            else if (categoryList[0].CategoryID != CategoryID)
            {
                categoryList = new List<Product>();
                DataTable dt = DB.GetProductTable(CategoryID);
                foreach (DataRow row in dt.Rows)
                {
                    Product P = new Product((int)row["ProductID"], (string)row["ProductName"], (int)row["CategoryID"], (string)row["QuantityPerUnit"], (decimal)row["UnitPrice"], (int)(short)row["UnitsInStock"], row["PictureFileName"] == DBNull.Value ? null : (string)row["PictureFileName"]);
                    categoryList.Add(P);
                }
            }
            return categoryList;

        }

        public static Customer GetCustomer(string CustomerID) // Returns a customers information from a given ID
        {
            DataRow c = DB.GetCustomer(CustomerID);
            Customer RetCustomer = new Customer((string)c["CustomerID"], (string)c["CompanyName"], (string)c["Address"], (string)c["City"], c["Region"] == DBNull.Value ? null : (string)c["Region"], c["PostalCode"] == DBNull.Value ? null : (string)c["PostalCode"], (string)c["Country"], (string)c["Pwd"]);
            CurrentUser = RetCustomer;
            return RetCustomer;
        }

        public static List<CartItem> LoadShoppingCart(string CustomerID) //Gets cart items for a customer from the DB
        {
            CurrentCart = new ShoppingCart(CustomerID);
            DataTable Items = DB.GetCart(CustomerID);
            foreach (DataRow item in Items.Rows)
            {
                CartItem tmp = new CartItem((int)item["ProductID"], (decimal)item["UnitPrice"], (int)(short)item["Quantity"]);
                CurrentCart.AddToCart(tmp);
            }
            return CurrentCart.Cart;
        }

        public static void AddCartItem(int ProductID, int Amount, string DiscountCode = "")
        {
            Product CurrentProduct = GetProduct(ProductID);
            CartItem NewCartItem = new CartItem(ProductID, CurrentProduct.UnitPrice, Amount);
            CurrentCart.AddToCart(NewCartItem);

            DB.AddCartItem(CurrentUser.CustomerID, ProductID, CurrentProduct.UnitPrice, Amount, DiscountCode);
            //DB.addNewOrder("ALFKI", DateTime.Now, 1, DateTime.Now, DateTime.Now, 1, 24.24);
     
        }

        public static void RemoveCartItem(string CustomerID, int ProductID)
        {

            foreach (CartItem item in CurrentCart.Cart)
            {
                if (item.ProductID == ProductID)
                {
                    CurrentCart.RemoveItem(item);
                    DB.RemoveCartItem(CurrentUser.CustomerID, ProductID);
                    break;
                }
            }
        }

        public static void UpdateCartItem(string CustomerID, int ProductID, int NewQuantity)
        {
            foreach (CartItem item in CurrentCart.Cart)
                if (item.ProductID == ProductID)
                {
                    item.Quantity = NewQuantity;
                    DB.UpdateCartItem(CustomerID, ProductID, NewQuantity);
                    break;
                }
        }

        public static List<Order> GetOrders(string CustomerID)
        {
            DataTable RawOrders = DB.GetOrders(CustomerID);
            OrderList.Clear();
            foreach (DataRow row in RawOrders.Rows)
            {
                Order tmpOrder = new Order((int)row["OrderID"], (string)row["CustomerID"], (DateTime)row["OrderDate"], row["RequiredDate"] == DBNull.Value ? null : (DateTime?)row["RequiredDate"], row["ShippedDate"] == DBNull.Value ? null : (DateTime?)row["ShippedDate"], row["ShipVia"] == DBNull.Value ? null : (int?)row["ShipVia"]);
                OrderList.Add(tmpOrder);
            }
            return OrderList;
        }

        public static void PlaceOrder(string CustomerID, ShoppingCart.ShippingAddress ShipAddress, DateTime OrderDate, DateTime RequiredDate, int ShipVIA, decimal FreightCost)
        {
            int orderid = DB.AddOrder(CustomerID, OrderDate, 12, RequiredDate, null, ShipVIA, FreightCost);
            foreach (CartItem item in CurrentCart.Cart)
            {
                DB.AddOrderDetails(orderid, item.ProductID, item.UnitPrice, (short)item.Quantity, 0.0F);
                DB.RemoveCartItem(CurrentUser.CustomerID, item.ProductID);
            }
            CurrentCart.Cart.Clear();
            DB.ChangeOrderAddress(orderid, ShipAddress.Address, ShipAddress.Shipname, ShipAddress.City, ShipAddress.Region, ShipAddress.PostalCode, ShipAddress.Country);
        }

        public static Product GetProduct(int ProductID)
        {
            Product P;
            //get from db
            DataRow dr = DBLoveleenJames.DB.GetProduct(ProductID);
            string filename = dr["PictureFileName"] == DBNull.Value ? null : (string)dr["PictureFileName"];
            P = new Product(ProductID, (string)dr["ProductName"], (int)dr["CategoryID"], (string)dr["QuantityPerUnit"], (decimal)dr["UnitPrice"], (int)(short)dr["UnitsInStock"], filename);
            return P;
        }

        public static List<Category> GetCategories()
        {
            List<Category> Categories = new List<Category>();
            DataTable dt = DB.GetAllCategories();
            foreach (DataRow row in dt.Rows)
            {
                Categories.Add(new Category((int)row["CategoryID"], (string)row["CategoryName"]));
            }
            return Categories;

        }
        public static void UpdatePassword(string CustomerID, string NewPassword)
        {
            DB.UpdatePassword(CustomerID, NewPassword);
        }

        public static List<Shipper> GetShippers()
        {
            DataTable DT = DB.GetShippers();
            List<Shipper> Shippers = new List<Shipper>();
            foreach (DataRow row in DT.Rows)
            {
                Shippers.Add(new Shipper((int)row["ShipperID"], (string)row["CompanyName"]));
            }
            return Shippers;
        }

        public void SetCart(ShoppingCart Cart)
        {
            CurrentCart = Cart;
        }

        public static List<Product> GetSearchItems(string SearchTerm)
        {
            List<Product> SearchResults = new List<Product>();
            DataTable RawSearch = DB.GetProduct(SearchTerm);
            foreach (DataRow row in RawSearch.Rows)
            {
                Product P = new Product((int)row["ProductID"], (string)row["ProductName"], (int)row["CategoryID"], (string)row["QuantityPerUnit"], (decimal)row["UnitPrice"], (int)(short)row["UnitsInStock"], row["PictureFileName"] == DBNull.Value ? null : (string)row["PictureFileName"]);
                SearchResults.Add(P);
            }
            return SearchResults;

        }

    }
}
