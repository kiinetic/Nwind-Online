<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="NWindWebOrdering.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<script runat="server">

    void btnLogin_Click(object sender, ImageClickEventArgs e)
    {
        string cusID = txtCustomer.Text.ToUpper();
        string cusPW = txtPassword.Text;

        NorthWindBusinessLayer.Customer checkCurrentCustomer;
        
        Session["CusID"] = cusID;

        try
        {
            checkCurrentCustomer = NorthWindBusinessLayer.BusinessLayer.GetCustomer(cusID);

            if (cusPW == checkCurrentCustomer.PWord)
            {
                Session["name"] = checkCurrentCustomer.CompanyName;
                FormsAuthentication.RedirectFromLoginPage(cusID,false);
                //Response.Redirect("AccountManager.aspx");

            }
            else
            {
                lblError.Text = "Incorrect UserName or Password";
                txtPassword.Text = "";
                txtPassword.Focus();
            }
        }
        catch
        {
            lblError.Text = "Incorrect UserName or Password";
            txtCustomer.Text = "";
            txtCustomer.Focus();
        }


    }
    
</script>
<head runat="server">
    <title>Northwind | Login</title>
    <link href="LoginStyle.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">



        <asp:Panel ID="Panel1" runat="server" BackImageUrl="LoginFormImage.jpg" Style="z-index: 0; left: 34%; top: 20%; position: absolute; height: 375px; width: 600px" CssClass="login">


            <asp:TextBox ID="txtCustomer" runat="server" Style="z-index: 1; left: 145px; top: 135px; position: absolute; width: 336px; height: 30px;" BackColor="Transparent" Font-Names="Verdana" Font-Size="25px" ForeColor="White"></asp:TextBox>

            <asp:TextBox ID="txtPassword" runat="server" Width="336px" Height="30px" Style="z-index: 1; left: 145px; top: 220px; position: absolute" BackColor="Transparent" Font-Names="Verdana" Font-Size="25px" ForeColor="White" TextMode="Password"></asp:TextBox>



            <asp:ImageButton ID="btnLogin" runat="server" ImageUrl="LoginButton.png" Style="z-index: 1; left: 115px; top: 283px; position: absolute" OnClick="btnLogin_Click" />

            <asp:Label ID="lblError" runat="server" Font-Names="Verdana" Font-Size="16pt" ForeColor="White" Style="z-index: 1; left: 121px; top: 329px; position: absolute; height: 33px; width: 374px"></asp:Label>
        </asp:Panel>




    </form>
</body>
</html>
