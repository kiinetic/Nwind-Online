<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PlaceOrder.aspx.cs" Inherits="ViewShoppingCart.PlaceOrder" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
    </div>
        <asp:Panel ID="Panel1" runat="server" Height="468px" Width="564px" BackColor="#0099FF">
            <asp:Label ID="Label2" runat="server" style="z-index: 1; left: 40px; top: 120px; position: absolute; height: 22px; width: 90px;" Text="Customer ID"></asp:Label>
            <asp:Button ID="btnPlaceOrd" runat="server" style="z-index: 1; left: 40px; top: 420px; position: absolute; width: 162px; right: 911px;" Text="Place the Order" OnClick="btnPlaceOrd_Click" />
            <asp:CheckBox ID="chkNew" runat="server" AutoPostBack="True" OnCheckedChanged="chkNew_CheckedChanged" style="z-index: 1; left: 251px; top: 118px; position: absolute" Text="Different Address" />
            <asp:Label ID="Label3" runat="server" style="z-index: 1; left: 40px; top: 150px; position: absolute; height: 22px; width: 77px; right: 1005px" Text="Cart Total"></asp:Label>
            <asp:Label ID="lblSub" runat="server" style="z-index: 1; left: 160px; top: 150px; position: absolute" Text="Label"></asp:Label>
            <asp:Label ID="Label4" runat="server" style="position: absolute; z-index: 1; left: 40px; top: 180px; height: 21px" Text="Company Name"></asp:Label>
            <asp:TextBox ID="txtCompanyName" runat="server" style="z-index: 1; left: 160px; top: 180px; position: absolute; width: 159px"></asp:TextBox>
            <asp:TextBox ID="txtRegion" runat="server" style="z-index: 1; left: 157px; top: 329px; position: absolute; height: 19px;"></asp:TextBox>
            <asp:TextBox ID="txtCity" runat="server" style="z-index: 1; left: 160px; top: 240px; position: absolute"></asp:TextBox>
            <asp:TextBox ID="txtCountry" runat="server" style="z-index: 1; left: 160px; top: 270px; position: absolute"></asp:TextBox>
            <asp:TextBox ID="txtAddress" runat="server" style="z-index: 1; left: 160px; top: 210px; position: absolute; width:200px"></asp:TextBox>
            <asp:TextBox ID="txtPostalCode" runat="server" style="z-index: 1; left: 160px; top: 300px; position: absolute"></asp:TextBox>
            <asp:Label ID="lblCus" runat="server" style="z-index: 1; left: 160px; top: 120px; position: absolute" Text="Label"></asp:Label>
            <asp:Label ID="Label5" runat="server" style="z-index: 1; left: 40px; top: 210px; position: absolute" Text="Address"></asp:Label>
            <asp:Label ID="Label7" runat="server" style="z-index: 1; left: 40px; top: 240px; position: absolute" Text="City"></asp:Label>
            <asp:Label ID="Label8" runat="server" style="z-index: 1; left: 40px; top: 270px; position: absolute" Text="Country"></asp:Label>
            <asp:Label ID="Label9" runat="server" style="z-index: 1; left: 40px; top: 300px; position: absolute" Text="Postal Code"></asp:Label>
            <asp:Label ID="Label10" runat="server" style="z-index: 1; left: 40px; top: 330px; position: absolute" Text="Region"></asp:Label>
            <br />
            <asp:Label ID="Label11" runat="server" Font-Bold="True" Font-Size="X-Large" style="z-index: 1; left: 169px; top: 60px; position: absolute" Text="Nwind Final Order Page"></asp:Label>
        </asp:Panel>
    </form>
</body>
</html>
