using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NorthWindBusinessLayer;


namespace NWindWebOrdering
{
    public partial class AccountManager : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!this.IsPostBack)
            {
                List<Order> orderHistoryList = new List<Order>();

                lblCustName.Text = (string)Session["name"];

                orderHistoryList = BusinessLayer.GetOrders((string)Session["CusID"]);

                gvOrdHist.DataSource = orderHistoryList;
                gvOrdHist.DataBind();
            }

        }

        protected void btnCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("index.aspx");
        }

        protected void btnChangePwd_Click(object sender, EventArgs e)
        {
            Customer checkCurrentCustomer;
            string cusPW = txtCurPwd.Text;
            lblError.Text = "";
            try
            {
                checkCurrentCustomer = BusinessLayer.GetCustomer((string)Session["CusID"]);

                if (cusPW == checkCurrentCustomer.PWord)
                {
                    BusinessLayer.UpdatePassword((string)Session["CusID"], txtNewPwd.Text);
                    lblError.Text = "Password successfully updated.";
                    txtCurPwd.Text = "";
                    txtNewPwd.Text = "";
                }
                else
                {
                    lblError.Text = "Incorrect Current Password! Please try again.";
                }
            }
            catch (Exception ex)
            {
                lblError.Text = ex.ToString();
            }
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            
        }
    }
}