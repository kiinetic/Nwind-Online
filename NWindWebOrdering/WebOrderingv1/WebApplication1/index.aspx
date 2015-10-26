<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="WebApplication1.WebForm1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" type="text/css" href="StyleSheet1.css" />

    <link rel="stylesheet" href="http://localhost:41104/code.jquery.com/ui/1.11.3/themes/smoothness/jquery-ui.css" />
    <script src="//code.jquery.com/jquery-1.10.2.js"></script>
    <script src="//code.jquery.com/ui/1.11.3/jquery-ui.js"></script>

    <script type="text/javascript">
        function initializePanel() {
            $("#shoppingPanel").slideUp(0);
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">

        <asp:ScriptManager runat="server"></asp:ScriptManager>
        <script type="text/javascript">
            function animatePanel(isVisible) {
                if (isVisible == true) {
                    $("#shoppingPanel").slideDown(2000, "easeOutBounce");
                } else {
                    $("#shoppingPanel").slideUp(2000, "easeOutElastic");
                }
            }
        </script>


        <div class="navPanel">
            <asp:Panel ID="navPanel" runat="server" CssClass="navPnl"></asp:Panel>
        </div>

        <asp:Panel runat="server" CssClass="body">
            <header>
                <div id="searchwrapper">
                    <asp:DropDownList ID="DropDownList2" CssClass="searchbox-drop" runat="server" Style="z-index: 1; right: 794px">
                        <asp:ListItem>Test 1</asp:ListItem>
                        <asp:ListItem>Test 2</asp:ListItem>
                        <asp:ListItem>Test 3</asp:ListItem>
                    </asp:DropDownList>
                    <asp:TextBox ID="TextBox1" CssClass="searchbox" runat="server" Style="z-index: 1;"></asp:TextBox>
                    <asp:Button ID="Button1" CssClass="searchbox_submit" runat="server" Style="z-index: 1; height: 22px;" Text="Search" align="right" />
                </div>

                <!-- Shopping cart button. View items in cart -->
                <asp:ImageButton ID="shoppingCartButton" runat="server" CssClass="shopCart" ImageUrl="~/Shopping Cart-40x40.png" ToolTip="View Cart" OnClick="shoppingCartButton_Click" Width="36px" Height="36px" />
                <br /><br />
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

        </asp:Panel>
    </form>
</body>
</html>
