using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

namespace WebApplication1 {
    public partial class WebForm2 : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            dt.Columns.Add("Product");
            dt.Columns.Add("Quantity");
            dt.Columns.Add("Price");
            GridView1.DataSource = dt;

            for(int i = 0; i < 6; i++) {
                dr = dt.NewRow();
                dr["Product"] = (i + 1).ToString();
                dr["Quantity"] = i + 2;
                dt.Rows.Add(dr);
                GridView1.DataBind();
            }
        }

        protected void GridView11_SelectedIndexChanged(object sender, EventArgs e) {

        }
    }
}