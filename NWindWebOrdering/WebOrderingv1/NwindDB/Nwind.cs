using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Data.Sql;

namespace NwindDB {
    public class Nwind {
        //private static string ConString = "Server=Colby-Desktop; Database=NwindSQL2; Trusted_Connection=True; Timeout=4";
        private static string ConString = "Server=win7b231-inst; Database=nwindsql; User Id=sa; Password=SQL_2012"; 
        private static SqlConnection con;

        public static void TestConnection() {
            con = new SqlConnection(ConString);
            con.Open();
            con.Close();
        }

        private static void OpenDB() {
            if(con != null)
                con = new SqlConnection(ConString);
            if(con.State != System.Data.ConnectionState.Open)
                con.Open();
        }

        private static void CloseDB() {
            if(con != null)
                if(con.State == System.Data.ConnectionState.Open)
                    con.Close();

        }

        public static DataTable ProductTable() {
            DataTable dtProducts = new DataTable();
            string sql = "SELECT * FROM Products";
            SqlDataAdapter da;
            OpenDB();
            da = new SqlDataAdapter(sql, con);
            da.FillSchema(dtProducts, SchemaType.Source);
            da.Fill(dtProducts);
            CloseDB();
            return dtProducts;
        }

        public static DataTable CategoryTable() {
            DataTable dtProducts = new DataTable();
            string sql = "SELECT CategoryID, CategoryName FROM Categories";
            SqlDataAdapter da;
            OpenDB();
            da = new SqlDataAdapter(sql, con);
            da.FillSchema(dtProducts, SchemaType.Source);
            da.Fill(dtProducts);
            CloseDB();
            return dtProducts;
        }

        public static DataTable searchProductTable(string query) {
            DataTable dtProducts = new DataTable();
            //string sql = "SELECT * FROM Products WHERE ProductName = 'chai'";
            string sql = "SELECT * FROM Products WHERE ProductName LIKE '" + query + "%'";
            SqlDataAdapter da;
            OpenDB();
            da = new SqlDataAdapter(sql, con);
            da.FillSchema(dtProducts, SchemaType.Source);
            da.Fill(dtProducts);
            CloseDB();
            return dtProducts;
        }
    }
}