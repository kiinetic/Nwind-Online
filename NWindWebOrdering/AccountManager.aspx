<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountManager.aspx.cs" Inherits="NWindWebOrdering.AccountManager" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

   <script runat="server">
    
       void btnLogout_Click(object sender, EventArgs e)
        {
            FormsAuthentication.SignOut();
            Response.Redirect("Login.aspx");            
        }     
    
    </script>  

<head runat="server">
    <title>Northwind | Manager</title>
    <link href="AccountManagerStyles.css" rel="stylesheet" type="text/css" />
    <link href='http://fonts.googleapis.com/css?family=Oxygen' rel='stylesheet' type='text/css' />
</head>
<body>

    <div id="container">
        <form id="form1" runat="server">
            <asp:Button ID="btnLogout" runat="server" Text="Logout" BackColor="#2CA8C2" BorderColor="#1F7789" Font-Names="Corbel" Font-Size="18pt" ForeColor="#292929" OnClick="btnLogout_Click" />

            <div id="welcome">
                <h1>Welcome
                <asp:Label ID="lblCustName" runat="server"></asp:Label></h1>
            </div>


            <div id="order_history">
                <h2>Order History</h2>
                <asp:GridView ID="gvOrdHist" runat="server" CellPadding="10" CssClass="cells"></asp:GridView>
            </div>


            <div id="change_password">
                <h2>Change Password</h2>
                Current Password:&nbsp;<asp:TextBox ID="txtCurPwd" runat="server"></asp:TextBox>

                <br />

                New Password:&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<asp:TextBox ID="txtNewPwd" runat="server"></asp:TextBox>
                <br />
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Incorrect format for password! Must use uppercase or lowercase letters or numbers only!" ValidationExpression="^[-a-zA-Z0-9]+$" ControlToValidate="txtNewPwd"></asp:RegularExpressionValidator>

                <br />
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                <br />

                <asp:Button ID="btnChangePwd" runat="server" Text="Change Password" BackColor="#2CA8C2" BorderColor="#1F7789" Font-Names="Corbel" Font-Size="18pt" ForeColor="#292929" OnClick="btnChangePwd_Click" />
            </div>
            <div id="new_order">
                <asp:Button ID="btnCart" runat="server" Text="New Order" OnClick="btnCart_Click" BackColor="#2CA8C2" BorderColor="#1F7789" Font-Names="Corbel" Font-Size="18pt" ForeColor="#292929" />
            </div>

        </form>
    </div>
</body>
</html>
