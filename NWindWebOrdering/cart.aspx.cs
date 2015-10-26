using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using NorthWindBusinessLayer;
using System.Data.SqlClient;

namespace ViewShoppingCart
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        List<CartItem> CartItems;
       
        protected void Page_preinit(object sender, EventArgs e)
        {
            //ShowProducts();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
           // CustomerID = (string)Session["CusID"];

            List<decimal> Subtotal = new List<decimal>();
            //List<Button> btnDelete;

            CartItems = BusinessLayer.LoadShoppingCart((string)Session["CusID"]);
            
            foreach (CartItem ci in CartItems)
            {
                Product P = BusinessLayer.GetProduct(ci.ProductID);

                Panel pPanel = new Panel();
                pPanel.ID = "pnlID" + P.ProductName;
                pPanel.CssClass = "ProductPanel";

                decimal Ptotal;
                
                Label PName = new Label();
                PName.Text = P.ProductName;
                PName.CssClass = "lName";
                pPanel.Controls.Add(PName);

                Label PPrice = new Label();
                PPrice.Text = ci.UnitPrice.ToString("c");
                PPrice.CssClass = "lPrice";
                pPanel.Controls.Add(PPrice);

                TextBox PQuantity = new TextBox();
                PQuantity.CssClass = "lQuant";
                PQuantity.Text = ci.Quantity.ToString();
                pPanel.Controls.Add(PQuantity);

                ContentPanel.Controls.Add(pPanel);

                Ptotal = ci.UnitPrice * ci.Quantity;
                Subtotal.Add(Ptotal);
               
              //  lsProducts.Items.Add(P.ProductName + " " + ci.UnitPrice.ToString("c") + "  " + ci.Quantity.ToString());
            }
            txtS.Text = Subtotal.Sum().ToString("c");
        }

        protected void btnP_Click(object sender, EventArgs e)
        {
            Response.Redirect("PlaceOrder.aspx");
        }
    }
}