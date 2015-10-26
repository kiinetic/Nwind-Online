using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace DBLoveleenJames
{
    public class DB
    {
        private static string ConnectionString;
        private static SqlConnection con;
        private static string serverName, dataBase, userName, password;
        private static bool isTrustedSecurity;
        #region Properties
        public static bool IsTrustedSecurity
        {
            get { return DB.isTrustedSecurity; }
            set
            {
                DB.isTrustedSecurity = value;
                PrepareDB(DB.serverName, DB.dataBase, !isTrustedSecurity, DB.userName, DB.dataBase);
            }
        }

        public static string DataBase
        {
            get { return DB.dataBase; }
            set
            {
                DB.dataBase = value;
                PrepareDB(DB.serverName, DB.dataBase, !isTrustedSecurity, DB.userName, DB.dataBase);
            }
        }

        public static string ServerName
        {
            get { return DB.serverName; }
            set
            {
                DB.serverName = value;
                PrepareDB(DB.serverName, DB.dataBase, !isTrustedSecurity, DB.userName, DB.dataBase);
            }
        }
        #endregion
        public static void PrepareDB(string ServerName, string DatabaseName, bool UseSQLSecurity = true, string Username = "sa", string Password = "SQL_2012")
        {
            isTrustedSecurity = !UseSQLSecurity;
            userName = Username; password = Password; dataBase = DatabaseName; serverName = ServerName; isTrustedSecurity = !UseSQLSecurity;

            if (isTrustedSecurity)
                ConnectionString = "Server=" + DB.serverName + "; Database=" + DB.dataBase + "; Trusted_Connection=true; Connection Timeout=5";
            else
                ConnectionString = "Server=" + DB.serverName + "; Database=" + DB.dataBase + "; user=" + DB.userName + ";pwd=" + DB.password + "; Connection Timeout=5";



        }
        static DB()
        {
            PrepareDB("localhost", "NWindMart", false);
        }

        public static bool TestConnection(out string ErrorMessage)
        {
            con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();
                con.Close();
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        private static void OpenDB()
        {
            
            if (con == null)
                con = new SqlConnection(ConnectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
        }
        private static void CloseDB()
        {
            if (con != null)
                con.Close();
        }
        /// <summary>
        /// Returns a single datarow containing the Customer information for the CustomerID supplied
        /// </summary>
        /// <param name="CustomerID">Customer ID to search for</param>
        /// <returns>Returns a Datarow of Customer Records</returns>
        public static DataRow GetCustomer(string CustomerID)
        {
            OpenDB();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand("Select * from Customers where CustomerID = @CustomerID", con);
            cmd.Parameters.Add(new SqlParameter("@CustomerID", CustomerID));
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.FillSchema(dt, SchemaType.Source);
            da.Fill(dt);
            CloseDB();
            if (dt.Rows.Count == 1)
                return dt.Rows[0];
            else
                throw new Exception("Customer ID: " + CustomerID + " not found");
        }
        /// <summary>
        /// Returns a datatable for a specific customerID
        /// </summary>
        /// <param name="customerID">Customer ID to search for</param>
        /// <returns></returns>
        public static DataTable GetCart(string customerID)
        {

            DataTable dt = new DataTable();
            OpenDB();
            SqlCommand cmd = new SqlCommand("Select * from ShoppingCart where CustomerID = @CustomerID ", con);
            cmd.Parameters.AddWithValue("@CustomerID", customerID);
            SqlDataReader rdr = cmd.ExecuteReader();
            //dt = rdr.GetSchemaTable();
            dt.Load(rdr);
            CloseDB();
            return dt;

        }

        public static void addNewOrder(string customerID, DateTime orderDate, int employeeID, DateTime requiredDate, DateTime shippedDate, int shipVia, double freight) {
            int RowsAffected;
            OpenDB();
            SqlCommand cmd = new SqlCommand("AddOrder", con); //Note: Instead of the sql command (Insert into...) , use the NAME of the stored procedure as recorded in the database
            cmd.CommandType = CommandType.StoredProcedure; //Note: must set the CommandType to StoredProcedure
            cmd.Parameters.Add(new SqlParameter()); //Note: first parameter is for the return value
            cmd.Parameters[0].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@OrderDate", orderDate));
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));
            cmd.Parameters.Add(new SqlParameter("@RequiredDate", requiredDate));
            cmd.Parameters.Add(new SqlParameter("@ShippedDate", shippedDate));
            cmd.Parameters.Add(new SqlParameter("@ShipVia", shipVia));
            cmd.Parameters.Add(new SqlParameter("@Freight", freight));
            RowsAffected = cmd.ExecuteNonQuery();
            CloseDB();

        }
        /// <summary>
        /// Add a new shopping Cart Item
        /// </summary>
        /// <param name="customerID">Customer ID of customer</param>
        /// <param name="productID">Product ID of product</param>
        /// <param name="unitPrice">Unit Price of Item</param>
        /// <param name="quantity">Quantity of Item</param>
        /// <param name="discountCode">Discount Code</param>
        public static void AddCartItem(string customerID, int productID, decimal unitPrice, int quantity, string discountCode)
        {
            DateTime time = DateTime.Now;
            DataTable dt = new DataTable();
            OpenDB();
            SqlTransaction SqlTrans = con.BeginTransaction();
            try {
                SqlCommand cmd = new SqlCommand("INSERT INTO ShoppingCart (CustomerID, ProductID, LastChangedDate, UnitPrice, Quantity, DiscountCode)"
                + "VALUES(@customerID, @productID, @lastChangedDate, @unitPrice, @quantity, @discountCode)", con, SqlTrans);
                cmd.Parameters.AddWithValue("@customerID", customerID);
                cmd.Parameters.AddWithValue("@productID", productID);
                cmd.Parameters.AddWithValue("@lastChangedDate", time);
                cmd.Parameters.AddWithValue("@unitPrice", unitPrice);
                cmd.Parameters.AddWithValue("@quantity", quantity);
                cmd.Parameters.AddWithValue("@discountCode", discountCode);
                cmd.ExecuteNonQuery();
                SqlTrans.Commit();
                CloseDB();
            } catch(Exception e) {
                SqlTrans.Rollback();
                CloseDB();
                throw e;
            }

        }
        /// <summary>
        /// Updates a shopping Cart Item
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="productID"></param>
        /// <param name="newQuantity"></param>
        public static void UpdateCartItem(string customerID, int productID, int newQuantity)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE ShoppingCart SET Quantity = @newQuantity WHERE CustomerID = @customerID AND ProductID = @productID";
            cmd.Parameters.AddWithValue("@newQuantity", newQuantity);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@productID", productID);

            cmd.ExecuteNonQuery();
            CloseDB();
        }
        ///Removes a Shopping Cart Item
        public static void RemoveCartItem(string customerID, int productID)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("DELETE FROM ShoppingCart WHERE CustomerID = @customerID AND ProductID = @productID", con);
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@productID", productID);
            cmd.ExecuteNonQuery();
            CloseDB();
        }
        ///returns a DataTable of product optionally based on CategoryID
        public static DataTable GetProductTable(int? categoryID = null) 
        {
            OpenDB();
            DataTable dt = new DataTable();

            SqlCommand cmd;
            if (categoryID == null)
            {
                cmd = new SqlCommand("Select * from Products", con);

            }
            else
            {
                cmd = new SqlCommand("Select * from Products WHERE CategoryID = @categoryID", con);
                cmd.Parameters.AddWithValue("@categoryID", categoryID);
            }
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.FillSchema(dt, SchemaType.Source);
            da.Fill(dt);
            CloseDB();
            return dt;
        }
        /// <summary>
        /// Returns a DataTable with DataRows that match the specified ProductName
        /// </summary>
        /// <param name="productName"></param>
        /// <returns>Datatable of Results</returns>
        public static DataTable GetProduct(string productName)
        {
            OpenDB();
            productName = "%" + productName + "%";
            SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductName Like @productName", con);
            cmd.Parameters.AddWithValue("@productName", productName);

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            CloseDB();

            if (dt.Rows.Count > 0)
                return dt;
            else
                return null;
            
        }
        /// returns a DataTable of shippers
        public static DataTable GetShippers()
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Shippers";
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);
            CloseDB();
            return dt;
        }
        ///Returns a DataTable of orders based on CustomerID
        public static DataTable GetOrders(string customerID)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "SELECT * FROM Orders WHERE CustomerID = @customerID";
            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Connection = con;
            SqlDataReader rdr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(rdr);
            CloseDB();
            return dt;
        }
        ///Adds a New Order Record and returns the Order ID as INT
        public static int AddOrder(string customerID, DateTime orderDate, int employeeID, DateTime requiredDate, DateTime? shippedDate, int shipVIA, decimal FreightCost)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("AddOrder", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter());
            cmd.Parameters[0].Direction = ParameterDirection.ReturnValue;

            cmd.Parameters.Add(new SqlParameter("@CustomerID", customerID));
            cmd.Parameters.Add(new SqlParameter("@OrderDate", orderDate));
            cmd.Parameters.Add(new SqlParameter("@EmployeeID", employeeID));
            cmd.Parameters.Add(new SqlParameter("@RequiredDate", requiredDate));
            if (shippedDate.HasValue)
                cmd.Parameters.Add(new SqlParameter("@ShippedDate", shippedDate));
            else
                cmd.Parameters.AddWithValue("@ShippedDate", DBNull.Value);

            cmd.Parameters.Add(new SqlParameter("@ShipVia", shipVIA));
            cmd.Parameters.Add(new SqlParameter("@Freight", FreightCost));



            int RowsAffected = cmd.ExecuteNonQuery(); // This acts funky with stored procedures
            int NewOrderID = (int)cmd.Parameters[0].Value;

            CloseDB();
            return NewOrderID;
        }
        ///Adds an OrderDetails record and returns the Number of entries as Int
        public static int AddOrderDetails(int orderID, int productID, decimal unitPrice, short quantity, float discount)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("AddOrderDetail", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter());
            cmd.Parameters[0].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.AddWithValue("@OrderID", orderID);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            cmd.Parameters.AddWithValue("@UnitPrice", unitPrice);
            cmd.Parameters.AddWithValue("@Quantity", quantity);
            cmd.Parameters.AddWithValue("@Discount", discount);

            int RowsAffected = cmd.ExecuteNonQuery(); // This acts funky with stored procedures
            int rowsAffected = (int)cmd.Parameters[0].Value;

            CloseDB();
            return rowsAffected;
        }
        ///Returns all Categories as Datatable
        public static DataTable GetAllCategories()
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Categories", con);
            DataTable dt = new DataTable();

            dt.Load(cmd.ExecuteReader());
            CloseDB();
            return dt;

        }
        ///Returns a DataRow Based on CategoryID
        public static DataRow GetCategory(int CategoryID)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("SELECT * FROM Categories WHERE CategoryID = @CategoryID", con);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            CloseDB();

            if (dt.Rows.Count == 1)
                return dt.Rows[0];
            else
                throw new Exception("Category ID: " + CategoryID.ToString() + " not found");
        }
        ///Changes the Orders Ship-To fields
        public static void ChangeOrderAddress(int OrderID, string ShipAddress, string ShipName = null, string ShipCity = null,
            string ShipRegion = null, string ShipPostalCode = null, string ShipCountry = null)
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("ChangeOrderAddress", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add(new SqlParameter());
            cmd.Parameters[0].Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.AddWithValue("@OrderID", OrderID);
            cmd.Parameters.AddWithValue("@ShipAddress", ShipAddress);

            if (ShipName == null)
                cmd.Parameters.AddWithValue("@ShipName", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ShipName", ShipName);
            if (ShipCity == null)
                cmd.Parameters.AddWithValue("@ShipCity", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ShipCity", ShipCity);
            if (ShipRegion == null)
                cmd.Parameters.AddWithValue("@ShipRegion", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ShipRegion", ShipRegion);
            if (ShipPostalCode == null)
                cmd.Parameters.AddWithValue("@ShipPostalCode", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ShipPostalCode", ShipPostalCode);
            if (ShipCountry == null)
                cmd.Parameters.AddWithValue("@ShipCountry", DBNull.Value);
            else
                cmd.Parameters.AddWithValue("@ShipCountry", ShipCountry);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
                throw new Exception("There were no rows affected by this query!");




            CloseDB();
        }
        public static DataRow GetProduct(int productID)
        //Returns a datarow for a specified product
        {
            OpenDB();

            SqlCommand cmd = new SqlCommand("SELECT * FROM Products WHERE ProductID = @ProductID", con);
            cmd.Parameters.AddWithValue("@ProductID", productID);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.FillSchema(dt, SchemaType.Source);
            da.Fill(dt);
            CloseDB();


            if (dt.Rows.Count == 1)
                return dt.Rows[0];
            else
                throw new Exception("Product ID: " + productID.ToString() + " not found");



        }
        public static bool UpdateCategoryIcon(int CategoryID, string filename)
        //Updates a Category's Icon
        {
            //use filestream object to read the image.
            //read to the full length of image to a byte array.
            //add this byte as an oracle parameter and insert it into database.
            try
            {

                //proceed only when the image has a valid path
                if (filename != "")
                {
                    FileStream fs;
                    fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                    //a byte array to read the image
                    byte[] picbyte = new byte[fs.Length];
                    fs.Read(picbyte, 0, System.Convert.ToInt32(fs.Length));
                    fs.Close();
                    OpenDB();
                    string query;
                    query = "UPDATE Categories SET Picture = @pic WHERE CategoryID =@categoryID";
                    SqlParameter picparameter = new SqlParameter();
                    picparameter.SqlDbType = SqlDbType.Image;
                    picparameter.ParameterName = "pic";
                    picparameter.Value = picbyte;
                    SqlCommand cmd = new SqlCommand(query, con);
                    cmd.Parameters.Add(picparameter);
                    cmd.Parameters.AddWithValue("@categoryID", CategoryID);
                    cmd.ExecuteNonQuery();

                    cmd.Dispose();
                    CloseDB();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static void UpdatePassword(string customerID, string password)
        //Updates Password for a Given Username
        {
            OpenDB();
            SqlCommand cmd = new SqlCommand("UPDATE Customers SET Pwd = @password WHERE CustomerID = @customerID", con);

            cmd.Parameters.AddWithValue("@customerID", customerID);
            cmd.Parameters.AddWithValue("@password", password);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected != 1)
                throw new Exception("Something went wrong with the UPDATE Query. The Rows Affected where not equal to 1");
            CloseDB();


        }

    }
    public class DB_CategoryIcon
    {
        private string ConnectionString;
        private SqlConnection con;
        private string serverName, dataBase, userName, password;
        private bool isTrustedSecurity;
        #region Properties
        public bool IsTrustedSecurity
        {
            get { return this.isTrustedSecurity; }
            set
            {
                this.isTrustedSecurity = value;
                PrepareDB(this.serverName, this.dataBase, !isTrustedSecurity, this.userName, this.dataBase);
            }
        }

        public string DataBase
        {
            get { return this.dataBase; }
            set
            {
                this.dataBase = value;
                PrepareDB(this.serverName, this.dataBase, !isTrustedSecurity, this.userName, this.dataBase);
            }
        }

        public string ServerName
        {
            get { return this.serverName; }
            set
            {
                this.serverName = value;
                PrepareDB(this.serverName, this.dataBase, !isTrustedSecurity, this.userName, this.dataBase);
            }
        }
        #endregion
        public void PrepareDB(string ServerName, string DatabaseName, bool UseSQLSecurity = true, string Username = "sa", string Password = "SQL_2012")
        {
            isTrustedSecurity = !UseSQLSecurity;
            userName = Username; password = Password; dataBase = DatabaseName; serverName = ServerName; isTrustedSecurity = !UseSQLSecurity;

            if (isTrustedSecurity)
                ConnectionString = "Server=" + this.serverName + "; Database=" + this.dataBase + "; Trusted_Connection=true; Connection Timeout=5";
            else
                ConnectionString = "Server=" + this.serverName + "; Database=" + this.dataBase + "; user=" + this.userName + ";pwd=" + this.password + "; Connection Timeout=5";



        }
        public DB_CategoryIcon()
        {
            PrepareDB("win7b228-inst", "NWindMart", true);
        }

        public bool TestConnection(out string ErrorMessage)
        {
            con = new SqlConnection(ConnectionString);
            try
            {
                con.Open();
                con.Close();
                ErrorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
                return false;
            }
        }

        private void OpenDB()
        {
            if (con == null)
                con = new SqlConnection(ConnectionString);
            if (con.State != ConnectionState.Open)
                con.Open();
        }
        private void CloseDB()
        {
            if (con != null)
                con.Close();
        }

        public byte[] GetCategoryIcon(int ID, out string contentType)
        //Returns a stream that contains the Image Data for a given Image
        {
            string SqlString = "SELECT Picture FROM Categories WHERE CategoryID = @ID";
            OpenDB();
            SqlCommand cmd = new SqlCommand(SqlString, con);
            cmd.Parameters.Add(new SqlParameter("@ID", ID));

            SqlDataReader rdr = cmd.ExecuteReader();
            byte[] image = new byte[0];
            string picContentType = string.Empty;

            if (rdr.HasRows)
            {
                rdr.Read();
                Byte[] imageData = (byte[])rdr["Picture"];

                int offset = GetOffsetFromSQLField(imageData, out picContentType);



                for (int i = offset; i < imageData.Length; i++)
                {
                    Array.Resize(ref image, i - offset + 1);
                    image[i - offset] = imageData[i];
                }

            }

            contentType = picContentType;
            CloseDB();
            return image;

        }
        private int GetOffsetFromSQLField(byte[] oleBytes, out string ContentType)
        //Determines pictureType and offset
        {
            int RetVal = 0;
            ContentType = "";

            for (int i = 78; i < 300; i++)
            {
                if (oleBytes[i] == 0x42 && oleBytes[i + 1] == 0x4D)
                {
                    ContentType = "image/bmp";
                    RetVal = i;
                    break;
                }
                if (oleBytes[i] == 0xFF && oleBytes[i + 1] == 0xD8)
                {
                    ContentType = "image/jpeg";
                    RetVal = i;
                    break;
                }
                if (oleBytes[i] == 0x49 && oleBytes[i + 1] == 0x49 && oleBytes[i + 2] == 0x2A && oleBytes[i + 3] == 0x00)
                {
                    ContentType = "image/tiff";
                    RetVal = i;
                    break;
                }
                if (oleBytes[i] == 0x89 && oleBytes[i + 1] == 0x50 && oleBytes[i + 2] == 0x4E && oleBytes[i + 3] == 0x47)
                {
                    ContentType = "image/png";
                    RetVal = i;
                    break;
                }
                if (oleBytes[i] == 0x47 && oleBytes[i + 1] == 0x49 && oleBytes[i + 2] == 0x46 && oleBytes[i + 3] == 0x38)
                {
                    ContentType = "image/gif";
                    RetVal = i;
                    break;
                }
            }
            return RetVal;
        }
    }

}
