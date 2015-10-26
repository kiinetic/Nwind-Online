using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NorthWindBusinessLayer;



namespace ViewShoppingCart
{

    public partial class PlaceOrder : System.Web.UI.Page
    {
        string CustomerID; // = "ALFKI";  
        Customer Cust;
        //List<CartItem> CartItems;
        ShoppingCart.ShippingAddress shipaddress;
        int ShipVia = 1;
        decimal Freight = 8.50M;
        protected void Page_Load(object sender, EventArgs e)
        {
        
            CustomerID = (string)Session["CusID"];
            lblCus.Text = CustomerID.ToString();
            Cust = BusinessLayer.GetCustomer(CustomerID);
            lblSub.Text = (string)Session["SubT"]; 
            if (!this.IsPostBack)
            {
                shipaddress.Shipname = Cust.CompanyName;
                shipaddress.Address = Cust.Address;
                shipaddress.City = Cust.City;
                shipaddress.Country = Cust.Country;
                shipaddress.PostalCode = Cust.PostalCode;
                shipaddress.Region = Cust.Region;
                txtCompanyName.Text = Cust.CompanyName;
                txtAddress.Text = Cust.Address.ToString();
                txtCountry.Text = Cust.Country.ToString();
                txtPostalCode.Text = Cust.PostalCode.ToString();
                txtCity.Text = Cust.City;
            }
            //txtRegion.Text = Cust.Region.ToString();
            
           // txtRegion.Text = Cust.Region.ToString();
        }

        protected void btnPlaceOrd_Click(object sender, EventArgs e)
        {
            if (chkNew.Checked)
            {
                shipaddress.Shipname = txtCompanyName.Text;
                shipaddress.Address = txtAddress.Text;
                shipaddress.City = txtCity.Text;
                shipaddress.Country = txtCountry.Text;
                shipaddress.PostalCode = txtPostalCode.Text;
                shipaddress.Region = txtRegion.Text;
            }
            else
            {
                shipaddress.Shipname = Cust.CompanyName;
                shipaddress.Address = Cust.Address;
                shipaddress.City = Cust.City;
                shipaddress.Country = Cust.Country;
                shipaddress.PostalCode = Cust.PostalCode;
                shipaddress.Region = Cust.Region;
            }
            BusinessLayer.PlaceOrder(CustomerID, shipaddress, DateTime.Now, DateTime.Now.AddDays(7), ShipVia, Freight);
            string str = "Saved Successfully into the Database";
            Response.Write("<script>alert('" + str + "')</script>");
            Response.Redirect("Login.aspx");
        }

        protected void chkNew_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNew.Checked)
            {
                txtAddress.Text = "";
                txtRegion.Text = "";
                txtCountry.Text = "";
                txtPostalCode.Text = "";
                txtCity.Text = "";
                txtCompanyName.Text = "";
            }
            else
            {
                txtCompanyName.Text = Cust.CompanyName;
                txtAddress.Text = Cust.Address.ToString();
                txtCountry.Text = Cust.Country.ToString();
                txtPostalCode.Text = Cust.PostalCode.ToString();
                txtCity.Text = Cust.City;
                //txtRegion.Text = Cust.Region.ToString();
            
            }
        }

    }
}