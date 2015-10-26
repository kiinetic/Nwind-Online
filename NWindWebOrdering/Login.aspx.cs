using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NorthWindBusinessLayer;

namespace NWindWebOrdering
{
    public partial class Login : System.Web.UI.Page
    {


        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            //string cusID = txtCustomer.Text.ToUpper();
            //string cusPW = txtPassword.Text;

            //Customer checkCurrentCustomer;

            //Session["CusID"] = cusID;

            //try
            //{
            //    checkCurrentCustomer = BusinessLayer.GetCustomer(cusID);

            //    if (cusPW == checkCurrentCustomer.PWord)
            //    {
            //        Session["name"] = checkCurrentCustomer.CompanyName;
            //        Response.Redirect("AccountManager.aspx");

            //    }
            //    else
            //    {
            //        lblError.Text = "Incorrect UserName or Password";
            //    }
            //}
            //catch
            //{
            //    lblError.Text = "Incorrect UserName or Password";
            //}


        }


    }
}