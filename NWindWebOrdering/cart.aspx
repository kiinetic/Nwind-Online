<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cart.aspx.cs" Inherits="ViewShoppingCart.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Master.css" rel="stylesheet" />
    <link href="Cart.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server" class="form1">
        <div class="navPanel">
            <asp:Panel ID="navPanel" runat="server" CssClass="navPnl">
                <asp:Image ID="imgLogo" runat="server" CssClass="imgLogo" ImageUrl="~/Resources/NWind_Logo.png" />
                <br />
            </asp:Panel>
        </div>

        <asp:Panel ID="BodyPanel" runat="server" CssClass="body">
            <header>
            </header>

            <section>
                <asp:Panel ID="ContentPanel" CssClass="ContentPanel" runat="server">
                    <asp:Label ID="Label1" runat="server" Style="left: 60px;" Text="Name" CssClass="h1"></asp:Label>
                    <asp:Label ID="Label2" runat="server" Style="left: 320px;" Text="Price" CssClass="h1"></asp:Label>
                    <asp:Label ID="Label3" runat="server" Style="left: 380px;" Text="Quantity" CssClass="h1"></asp:Label>

                    <asp:Label ID="lbls" runat="server" Style="z-index: 1; left: 473px; top: 430px; position: relative; height: 18px; width: 80px" Text="SubTotal" CssClass="h1"></asp:Label>

                    <asp:Button ID="btnShop" runat="server" Style="z-index: 1; left: -101px; top: 435px; position: relative" Text="Continue Shopping" />
                    <asp:Button ID="btnCheckout" runat="server" Style="z-index: 1; left: 48px; top: 434px; position: relative; width: 157px;" Text="Proceed to Checkout" OnClick="btnP_Click" />

                    <asp:TextBox ID="txtS" runat="server" Style="z-index: 1; left: 343px; top: 424px; position: relative; height: 22px; width: 128px;"></asp:TextBox>

                </asp:Panel>
            </section>
        </asp:Panel>
    </form>
</body>
</html>
