<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="NorthWindShopping.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="Master.css" rel="stylesheet" />
    <link href="Index.css" rel="stylesheet" />
    <script src="http://code.jquery.com/jquery-1.10.2.js"></script>
    <script src="http://code.jquery.com/ui/1.11.3/jquery-ui.js"></script>

    <script type="text/javascript">
        function initializePanel() {
            $("#shoppingPanel").slideUp(0);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server" class="form1">

        <asp:ScriptManager runat="server"></asp:ScriptManager>

        <script type="text/javascript">
            function animatePanel(isVisible) {
                if (isVisible == true) {
                    $("#shoppingPanel").slideDown(1000, "easeOutBack");
                } else {
                    $("#shoppingPanel").slideUp(1000, "easeOutElastic");
                }
            }
        </script>

        <div class="navPanel">
            <asp:Panel ID="navPanel" runat="server" CssClass="navPnl">
                <asp:Image ID="imgLogo" runat="server" CssClass="imgLogo" ImageUrl="~/Resources/NWind_Logo.png" />
                <br />
                    <div class="searchWrapper">
                        <asp:ImageButton ID="btnSearch" CssClass="searchbox_btn" ImageUrl="~/Resources/search_icon.png" runat="server" Style="z-index: 1;" OnClick="btnSearch_Click" />
                        <asp:TextBox ID="txtSearch" CssClass="searchbox_bar" runat="server" Style="z-index: 1;"></asp:TextBox>
                        
                    </div>
                <br />
                <h2>Categories:</h2>
                <asp:Panel ID="catPnl" runat="server" CssClass="catPnl"></asp:Panel>

            </asp:Panel>
        </div>

        <asp:Panel runat="server" CssClass="body">
            <header>
                <!-- Shopping cart button. View items in cart -->
                <div class="cartWrapper">
                    <asp:Label ID="lblNumItems" runat="server" CssClass="cartLabel" Text="0"></asp:Label>
                    <asp:ImageButton ID="shoppingCartButton" runat="server" CssClass="shopCart" ImageUrl="~/Resources/shopping_cart.png" OnClick="shoppingCartButton_Click" ToolTip="View Cart"/>
                </div>
                <br />
                <br />
                <asp:Panel ID="shoppingPanel" runat="server" CssClass="Shopping-Panel" Width="319px">
                    <asp:GridView ID="shoppingCartList" runat="server" CssClass="shoppingCartList" GridLines="None" Style="height: 0px;" BackColor="#CCCCCC" Width="309px" CellPadding="2" align="center">
                        <AlternatingRowStyle BackColor="White" />
                        <HeaderStyle BackColor="White" />
                        <RowStyle HorizontalAlign="Center" />
                    </asp:GridView>
                    <br />
                    <asp:Button ID="btnGoToCart" runat="server" Text="View Cart" CssClass="Shopping-Button" OnClick="btnGoToCart_Click" Width="309px" Height="25px" />
                </asp:Panel>
            </header>

            <section>
                <asp:Panel ID="ContentPanel" runat="server" CssClass="ContentPanel"></asp:Panel>
                <asp:Panel ID="pagePanel" runat="server" CssClass="pagePanel"></asp:Panel>
            </section>
        </asp:Panel>
    </form>
</body>
</html>
