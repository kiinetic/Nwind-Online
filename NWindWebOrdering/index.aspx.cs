using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using NorthWindBusinessLayer;

namespace NorthWindShopping {
    public partial class index : System.Web.UI.Page {
        const int MAX_ITEMS_PER_PAGE = 10;
        const int MAX_ITEMS_IN_CART_GRIDVIEW = 5;

        protected void Page_Load(object sender, EventArgs e) {
            if(Session["shoppingCart"] == null) {
                BusinessLayer.LoadShoppingCart((string)Session["CusID"]);
                Session["shoppingCart"] = BusinessLayer.CurrentCart;
            }
            if(ViewState["shopView"] == null)
                ViewState["shopView"] = false;
            updateShoppingCartList(); // need to update the list every time the page loads
            if(Request.QueryString["cat"] != null || Request.QueryString["search_query"] != null) {
                displayItems();
                if(!this.IsPostBack)
                    if(Request.QueryString["search_query"] != null)
                        txtSearch.Text = Request.QueryString["search_query"]; // trying to search a second time while trying to do this without checking if it's not a postback wouldn't actually work, kinda odd
                try {
                    displayCategoryLinks(int.Parse(Request.QueryString["cat"]));
                } catch(Exception) {
                    displayCategoryLinks(-1);
                }
            } else
                displayCategoryLinks(-1);
            if((bool)ViewState["shopView"] == false)
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:initializePanel();", true);
        }

        void displayCategoryLinks(int categoryID) { // categoryID arg to bold what category a user is viewing
            foreach(Category category in BusinessLayer.GetCategories()) {
                LinkButton categoryBtn = new LinkButton();
                categoryBtn.ID = "navLinkCat" + category.CategoryID;
                categoryBtn.Text = category.CategoryName;
                if(categoryID == category.CategoryID)
                    categoryBtn.Attributes.Add("style", "font-weight: bold; font-size: 1.5em;");
                categoryBtn.CssClass = "navLink";
                categoryBtn.Click += category_Click;
                catPnl.Controls.Add(categoryBtn);
            }
        }

        void category_Click(object sender, EventArgs e) {
            string name = ((LinkButton)sender).ID;
            int categoryID = int.Parse(Regex.Match(name, @"\d+").Value);
            Response.Redirect("index.aspx?cat=" + categoryID + "&page=1");
        }

        void displayItems() {
            int categoryID;
            try {
                categoryID = int.Parse(Request.QueryString["cat"]);
            } catch(Exception) {
                categoryID = -1;
            }
            string searchingOrViewing = Request.QueryString["cat"] == null ? Request.QueryString["search_query"] : Request.QueryString["cat"]; // so we can check which query is being used to see if it's empty
            if(validCategoryID(categoryID) || Request.QueryString["search_query"] != null && searchingOrViewing != "") {
                int count;
                try {
                    count = int.Parse(Request.QueryString["page"]) * MAX_ITEMS_PER_PAGE;
                } catch(Exception) {
                    count = 0;
                }
                int top = 210;
                int left = 15;
                int start = count - MAX_ITEMS_PER_PAGE; // i.e. viewing page 2: 20-10 = show products 11-20 or however many products are left to show
                int productCount = 0; // count up the number of products so we know which product to start and end at
                int panelCount = 0;
                List<Product> products = Request.QueryString["search_query"] == null ? BusinessLayer.CategoryProductList(categoryID) : BusinessLayer.GetSearchItems(Request.QueryString["search_query"]); //BusinessLayer.searchForProducts(Request.QueryString["search_query"])
                foreach(Product p in products) {
                    productCount++;
                    if(Request.QueryString["page"] != null) { // viewing the page with the search query / category ID and page number, i.e. cat=2&page=2 or search_query=chai&page=2
                        if(productCount > start && productCount <= count) {
                            addPanelAndControls(left, top, p.ProductID, p.ProductName, p.UnitPrice, p.UnitsInStock, p.QuantityPerUnit);
                            if(panelCount % 2 != 0) { 
                                top += 150;
                                left = 15;
                            } else
                                left += 415;
                            panelCount++;
                        }
                    } else if(Request.QueryString["page"] == null) { // viewing the page with just the search query or categoryID, i.e. cat=2 or search_query=chai
                        if(count < MAX_ITEMS_PER_PAGE) { // count will start at 0
                            addPanelAndControls(left, top, p.ProductID, p.ProductName, p.UnitPrice, p.UnitsInStock, p.QuantityPerUnit);
                            if(panelCount % 2 != 0) {
                                top += 150;
                                left = 15;
                            } else
                                left += 415;
                            count++;
                            panelCount++;
                        }
                    }
                }
                if(productCount > MAX_ITEMS_PER_PAGE) { // add buttons to show the user that there are more pages to view
                    int numberOfPages = productCount / MAX_ITEMS_PER_PAGE;
                    top += 50;
                    for(int i = 0; i <= numberOfPages; i++) {
                        left += 15;
                        addLinkButtons(left, top, i + 1);
                    }
                }
            }
        }

        bool validCategoryID(int categoryID) { 
            foreach(Category category in BusinessLayer.GetCategories()) {
                if(category.CategoryID == categoryID)
                    return true;
            }
            return false;
        }

        void addLinkButtons(int left, int top, int ID) {
            LinkButton btnNextPage = new LinkButton();
            btnNextPage.ID = "btnNextPage" + ID;
            btnNextPage.Text = ID.ToString();
            int pageID = 0;
            if(int.TryParse(Request.QueryString["page"], out pageID)) 
                btnNextPage.CssClass = pageID == ID ? "btnPagesActive" : "btnPages";
            btnNextPage.Width = 20;
            btnNextPage.Click += btnNextPage_Click;
            pagePanel.Controls.Add(btnNextPage);
        }

        void btnNextPage_Click(object sender, EventArgs e) {
            string name = ((LinkButton)sender).ID;
            int ID = int.Parse(Regex.Match(name, @"\d+").Value);
            if(Request.QueryString["search_query"] != null) 
                if(Request.QueryString["page"] == null)
                    Response.Redirect("index.aspx?search_query=" + Request.QueryString["search_query"] + "&page=2");
                else
                    Response.Redirect("index.aspx?search_query=" + Request.QueryString["search_query"] + "&page=" + ID);
            else if(Request.QueryString["cat"] != null) 
                if(Request.QueryString["page"] == null)
                    Response.Redirect("index.aspx?cat=" + Request.QueryString["cat"] + "&page=2");
                else
                    Response.Redirect("index.aspx?cat=" + Request.QueryString["cat"] + "&page=" + ID);
        }

        void addPanelAndControls(int left, int top, int productID, string productName, decimal unitPrice, int unitsInStock, string quantityPerU) {
            ShoppingCart shopCart = Session["shoppingCart"] != null ? (ShoppingCart)Session["shoppingCart"] : null;
            Panel panel = new Panel();
            panel.ID = "pnlID" + productID;
            panel.CssClass = "panelItem";

            Label label = new Label(); // product name
            label.ID = "lblProd" + productID;
            label.CssClass = "productName";
            label.Width = 400;
            label.Text = productName;
            panel.Controls.Add(label);

            label = new Label(); // qty per unit
            label.CssClass = "productQuantityPerU";
            label.Width = 450;
            label.Text = "Quantity per unit: " + quantityPerU;
            panel.Controls.Add(label);

            label = new Label(); // price
            label.ID = "lblPrice" + productID;
            label.CssClass = "productPrice";
            label.Width = 150;
            label.Text = "Price: $" + Math.Round(unitPrice, 2);
            panel.Controls.Add(label);

            label = new Label(); // stock
            label.Width = 100;
            label.CssClass = "productStock";
            label.Text = unitsInStock != 0 ? "In stock" : "Out of stock";
            label.ForeColor = unitsInStock != 0 ? Color.Green : Color.Red;
            panel.Controls.Add(label);

            label = new Label(); // qty label
            label.Width = 10;
            label.CssClass = "productQuantlbl";
            label.Text = "Qty";
            panel.Controls.Add(label);

            string quantity = "";
            string buttonText = "";
            if(shopCart != null) {
                foreach(CartItem p in shopCart.Cart) {
                    if(productID == p.ProductID) {
                        quantity = p.Quantity.ToString();
                        buttonText = "Update";
                    }
                }
            }

            TextBox txtBox = new TextBox(); // qty
            txtBox.ID = "txtBoxQuantityProd" + productID;
            txtBox.CssClass = "productQuanttxt";
            txtBox.Width = 60;
            txtBox.Text = quantity == "" ? "1" : quantity;
            txtBox.Enabled = unitsInStock != 0 ? true : false;
            txtBox.MaxLength = 5; 
            panel.Controls.Add(txtBox);

            Button addToCart = new Button();
            addToCart.ID = "btnAddToCartProd" + productID;
            addToCart.CssClass = "productAdd";
            addToCart.Text = buttonText == "" ? "Add To Cart" : buttonText;
            addToCart.Width = 150;
            addToCart.Click += addToCart_Click;
            addToCart.Enabled = unitsInStock != 0 ? true : false;
            panel.Controls.Add(addToCart);

            ContentPanel.Controls.Add(panel);
        }

        void addToCart_Click(object sender, EventArgs e) {
            int categoryID;
            try {
                categoryID = int.Parse(Request.QueryString["cat"]);
            } catch(Exception) {
                categoryID = 0; // should almost never be 0
            }
            string name = ((Button)sender).ID;
            int productID = int.Parse(Regex.Match(name, @"\d+").Value);
            Product p = BusinessLayer.GetProduct(productID);
            decimal price = p.UnitPrice;
            Control textBox = FindControl("txtBoxQuantityProd" + productID);
            Control button = FindControl(name);
            int quantity = 0;
            if(int.TryParse((Regex.Match(((TextBox)textBox).Text, @"\d+").Value), out quantity)) { // actual textchanged event fires when the text changes BETWEEN posts
                updateShoppingCart(new CartItem(productID, price, quantity));
                ((Button)button).Text = "Update";
            }
            //System.Web.UI.WebControls.Image checkmark = new System.Web.UI.WebControls.Image();
            //checkmark.ImageUrl = "checkmark.png";
            //checkmark.Attributes.Add("style", "top: " + (getPanelStyleAttribute(name, "top") + 25) + "px; left: " + (getPanelStyleAttribute(name, "left") + 300 + 10) + "px; position: absolute;"); // left: 300px because of the panel width + a few pixels to the left so it's not in the panel
            //Page.Form.Controls.Add(checkmark);
        }

        int getPanelStyleAttribute(string ctrlName, string attribute) {
            Control ctrl = FindControl(ctrlName);
            System.Web.UI.WebControls.Panel panel = (System.Web.UI.WebControls.Panel)((Button)ctrl).Parent;
            string styleAttribute = panel.Attributes["style"];
            string leftSubstring = styleAttribute.Substring(styleAttribute.IndexOf(attribute), 11); // 11 is a safe length
            return int.Parse(Regex.Match(leftSubstring, @"\d+").Value);
        }

        void updateShoppingCart(CartItem product) {
            ShoppingCart shopCart = Session["shoppingCart"] != null ? (ShoppingCart)Session["shoppingCart"] : null;
            bool productNotFound = true;
            if(product.Quantity <= short.MaxValue) {
                if(shopCart != null) {
                    foreach(CartItem p in shopCart.Cart) {
                        if(product.ProductID == p.ProductID) {
                            productNotFound = false;
                            BusinessLayer.UpdateCartItem((string)Session["CusID"], product.ProductID, product.Quantity);
                        }
                    }
                    if(productNotFound)
                        BusinessLayer.AddCartItem(product.ProductID, product.Quantity, "");
                    Session["shoppingCart"] = BusinessLayer.CurrentCart;
                    updateShoppingCartList();
                }
            }
        }

        void updateShoppingCartList() { // update the grid view - only show a max of 5 products?
            shoppingCartList.Columns.Clear();
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            ShoppingCart shopCart = Session["shoppingCart"] != null ? (ShoppingCart)Session["shoppingCart"] : null;
            if(shopCart != null) {
                lblNumItems.Text = shopCart.Cart.Count.ToString();
                btnGoToCart.Text = "View Cart (" + shopCart.Cart.Count + " items)";
                dt.Columns.Add("Product");
                dt.Columns.Add("Quantity");
                shoppingCartList.DataSource = dt;
                if(shopCart.Cart.Count != 0) {
                    int count = 0;
                    //List<string> data = new List<string>();
                    foreach(CartItem p in shopCart.Cart) {
                        if(count < MAX_ITEMS_IN_CART_GRIDVIEW) {
                            dr = dt.NewRow();
                            dr["Product"] = getProductName(p.ProductID);
                            dr["Quantity"] = p.Quantity;
                            dt.Rows.Add(dr);
                            shoppingCartList.DataBind();
                            //data.Add(p.Value + "," + count + "," + getProductName(p.Key)); 
                            count++;
                        }
                    }
                    //incase we decide to put back

                    //string[] split;
                    //TextBox txtBox;
                    //Button btn;
                    //foreach(string d in data) { // data list is in the format of rowID, cellID, name - i.e. 1,3,Chai 
                    //    split = d.Split(',');
                    //    txtBox = new TextBox();
                    //    txtBox.Width = 50;
                    //    txtBox.Style["text-align"] = "center";
                    //    txtBox.Text = split[0];
                    //    txtBox.ID = "txtBoxProd" + getProductID(split[2]);
                    //    //bindControlsToGridView(int.Parse(split[1]), 1, txtBox);  // rowID, cellID, control
                    //    btn = new Button();
                    //    btn.Width = 60;
                    //    btn.Text = "Update";
                    //    btn.ID = "btnProd" + getProductID(split[2]);
                    //    btn.Click += btn_Click;
                    //    //bindControlsToGridView(int.Parse(split[1]), 3, btn); // rowID, cellID, control
                    //}
                } else {
                    dr = dt.NewRow();
                    dr["Product"] = "No items";
                    dr["Quantity"] = "";
                    dt.Rows.Add(dr);
                    shoppingCartList.DataSource = dt;
                }
            } else {
                dr = dt.NewRow();
                dr["Product"] = "An error occured";
                dr["Quantity"] = "";
                dt.Rows.Add(dr);
                shoppingCartList.DataSource = dt;
            }
        }

        string getProductName(int productID) {
            Product p = BusinessLayer.GetProduct(productID);
            return p.ProductName;
        }

        //incase we decide to put back updating the quantity in the 5 item cart

        //void btn_Click(object sender, EventArgs e) { // update the quantity in the 5 item shopping cart
        //    string buttonName = ((Button)sender).ID;
        //    int productID = int.Parse(Regex.Match(buttonName, @"\d+").Value);
        //    GridViewRow gvr = (GridViewRow)(sender as Control).Parent.Parent;
        //    int index = gvr.RowIndex;
        //    string textBoxName = "txtBoxProd" + productID;
        //    Control textBox = shoppingCartList.Rows[index].Cells[1].FindControl(textBoxName);
        //    int quantity = 0;
        //    try { // need to do more checking for when they update the quantity
        //        quantity = int.Parse(((TextBox)textBox).Text);
        //        Dictionary<int, int> shoppingCartSession = Session["shoppingCart"] != null ? (Dictionary<int, int>)Session["shoppingCart"] : null;
        //        if(shoppingCartSession != null) {
        //            if(quantity == 0) // assume that they're wanting to remove the product from the cart?
        //                shoppingCartSession.Remove(productID);
        //            else if(quantity <= 500)
        //                shoppingCartSession[productID] = quantity;
        //            Session["shoppingCart"] = shoppingCartSession;
        //            updateShoppingCartList();
        //        }
        //    } catch(Exception) {

        //    }
        //}

        void bindControlsToGridView(int rowID, int cellID, Control control) {
            shoppingCartList.Rows[rowID].Cells[cellID].Controls.Add(control);
        }

        protected void btnGoToCart_Click(object sender, EventArgs e) {
            Response.Redirect("cart.aspx");
        }

        protected void shoppingCartButton_Click(object sender, ImageClickEventArgs e) {
            updateShoppingCartList();
            if((bool)ViewState["shopView"])
                ViewState["shopView"] = false;
            else
                ViewState["shopView"] = true;
            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "myFunc", "animatePanel(" + (((bool)ViewState["shopView"] == true) ? "true" : "false") + ")", true);
        }

        protected void btnSearch_Click(object sender, ImageClickEventArgs e) {
            Response.Redirect("index.aspx?search_query=" + txtSearch.Text.Trim() + "&page=1");
        }
    }
}