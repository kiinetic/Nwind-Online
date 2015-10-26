using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Text.RegularExpressions;
using NwindDB;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {

        const sbyte MAX_ITEMS_PER_PAGE = 10;
        const sbyte MAX_ITEMS_IN_CART = 5;

        Dictionary<int, int> shoppingCart = new Dictionary<int, int>(); // productID, quantity
        Dictionary<int, string> dbCategories = new Dictionary<int, string>(); // categoryID, categoryName
        List<string> dbProductInfo = new List<string>(); // a list because we can separate data using comma separation

        DataTable dtProducts;
        DataTable dtCategories;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["shopView"] == null)
                ViewState["shopView"] = false;

            if (Session["shoppingCart"] == null)
                Session["shoppingCart"] = shoppingCart;
            updateShoppingCartList(); // need to update the list every time the page loads
            if (!Page.IsPostBack)
            {
                if (Session["dbProducts"] == null || Session["dbCategories"] == null)
                {
                    try
                    {
                        Nwind.TestConnection();
                    }
                    catch (Exception)
                    {
                        string msg = "Unable to connect to database";
                        Response.Write("<script>alert('" + msg + "')</script>");
                    }
                    dtProducts = Nwind.ProductTable();
                    dtCategories = Nwind.CategoryTable();
                    addProducts();
                    addCategories();
                    Session["dbProducts"] = dbProductInfo;
                    Session["dbCategories"] = dbCategories;
                }
            }
            if (Request.QueryString["cat"] != null)
                displayCategoryLinks(int.Parse(Request.QueryString["cat"]));
            else
                displayCategoryLinks(-1);
            displayItems();

            if ((bool)ViewState["shopView"] == false)
                ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:initializePanel();", true);
        }

        void displayCategoryLinks(int categoryID)
        { // categoryID arg to highlight what category they're viewing
            int top = 10;
            Dictionary<int, string> categoriesDictionary = Session["dbCategories"] != null ? (Dictionary<int, string>)Session["dbCategories"] : null;
            if (categoriesDictionary != null)
            {
                foreach (var p in categoriesDictionary)
                {
                    LinkButton category = new LinkButton();
                    category.ID = "navLinkCat" + p.Key; // id instead of name because the name will most likely cause errors
                    category.Text = p.Value.ToString();
                    category.CssClass = "navLink";
                    category.Click += category_Click;
                    navPanel.Controls.Add(category);
                    top += 50;
                }
            }
        }

        void category_Click(object sender, EventArgs e)
        {
            string name = ((LinkButton)sender).ID;
            int categoryID = int.Parse(Regex.Match(name, @"\d+").Value);
            Response.Redirect("index.aspx?cat=" + categoryID);
        }

        void displayItems()
        {
            List<string> dbProducts = Session["dbProducts"] != null ? (List<string>)Session["dbProducts"] : null;
            int count;
            int categoryID; // ID instead of name because it's easier to work with
            try
            {
                count = int.Parse(Request.QueryString["page"]) * MAX_ITEMS_PER_PAGE;
            }
            catch (Exception)
            {
                count = 0;
            }
            // putting both in 1 try/catch doesn't work
            try
            {
                categoryID = int.Parse(Request.QueryString["cat"]);
            }
            catch (Exception)
            {
                categoryID = 0;
            }
            int top = 130;
            int left = 200;
            int start = count - MAX_ITEMS_PER_PAGE; // i.e. viewing page 2: 20-10 = go from product 10-19 or however many products are left to show
            int productCount = 0; // count up the number of products so we know which product to start and end at
            int panelCount = 0;
            if (dbProducts != null)
            {
                string[] split;
                foreach (string p in dbProducts)
                {
                    split = p.Split(',');
                    if (split[4] != "")
                    { // some products have a null for its category, it'll be an empty string
                        if (int.Parse(split[4]) == categoryID)
                        {
                            if (Request.QueryString["page"] != null && Request.QueryString["cat"] != null)
                            { // viewing the page with a category ID and page number, i.e. cat=2&page=2
                                productCount++;
                                if (productCount > start && productCount <= count)
                                {
                                    if (split[2] != "")
                                    { // don't display products that have a null unit price
                                        addPanelAndControls(left, top, int.Parse(split[0]), split[1], double.Parse(split[2]), int.Parse(split[3]));
                                        left += 415;
                                        if (panelCount % 2 != 0)
                                        {
                                            top += 150;
                                            left = 200;
                                        }
                                        panelCount++;
                                        if (productCount == count - 1)
                                        {
                                            Button btnNext = new Button();
                                            btnNext.ID = "btnNext";
                                            btnNext.Text = "Next Page";
                                            btnNext.Width = 60;
                                            btnNext.Click += btnNext_Click;
                                            Page.Form.Controls.Add(btnNext);
                                        }
                                    }
                                }
                            }
                            else if (Request.QueryString["cat"] != null && Request.QueryString["page"] == null)
                            { // viewing the page with just the categoryID, i.e. cat=2
                                if (count < MAX_ITEMS_PER_PAGE)
                                { // count will start at 0
                                    if (split[2] != "")
                                    { // don't display products that have a null unit price
                                        addPanelAndControls(left, top, int.Parse(split[0]), split[1], double.Parse(split[2]), int.Parse(split[3]));
                                        left += 415;
                                        if (panelCount % 2 != 0)
                                        {
                                            top += 150;
                                            left = 200;
                                        }
                                        count++;
                                        panelCount++;
                                    }
                                }
                                if (count == MAX_ITEMS_PER_PAGE)
                                {
                                    Button btnNext = new Button();
                                    btnNext.ID = "btnNext";
                                    btnNext.Text = "Next Page";
                                    btnNext.Width = 80;
                                    btnNext.Attributes.Add("style", "left: " + (left + 300) + "px; position: absolute; top: " + top + "px;");
                                    btnNext.Click += btnNext_Click;
                                    Page.Form.Controls.Add(btnNext);
                                    count++;
                                }
                            }
                        }
                    }
                }
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["cat"] != null && Request.QueryString["page"] == null)
            {
                Response.Redirect("index.aspx?cat=" + Request.QueryString["cat"] + "&page=2");
            }
            else
            {
                Response.Redirect("index.aspx?cat=1" + Request.QueryString["cat"] + "&page=" + Request.QueryString["page"] + 1);
            }
        }

        void addPanelAndControls(int left, int top, int productID, string productName, double unitPrice, int unitsInStock)
        {
            Panel panel = new Panel();
            panel.Height = 90;
            panel.Width = 400;
            panel.Attributes.Add("style", "left: " + left + "px; position: absolute; top: " + top + "px;");
            panel.BorderStyle = BorderStyle.Solid;
            Label label = new Label(); // product name
            label.ID = "lblProd" + productID;
            label.CssClass = "productList";
            label.Attributes.Add("style", "font-weight: bold;");
            label.Width = 400;
            label.Text = productName;
            panel.Controls.Add(label);
            label = new Label(); // price
            label.ID = "lblPrice" + productID;
            label.CssClass = "productList";
            label.Width = 150;
            label.Attributes.Add("style", "position: absolute; left: 1px; top: 33px;");
            label.Text = "Price: $" + Math.Round(unitPrice, 2);
            panel.Controls.Add(label);
            label = new Label(); // stock
            label.Width = 85;
            label.CssClass = "productList";
            label.Attributes.Add("style", "position: absolute; left: 1px; top: 66px;");
            label.Text = unitsInStock != 0 ? "In stock" : "Out of stock";
            label.ForeColor = unitsInStock != 0 ? Color.Green : Color.Red;
            panel.Controls.Add(label);
            TextBox txtBox = new TextBox();
            txtBox.ID = "txtBoxQuantityProd" + productID;
            txtBox.Width = 30;
            txtBox.Text = "1";
            txtBox.Attributes.Add("style", "top: 35px; left: 110px; position: absolute;");
            txtBox.Enabled = unitsInStock != 0 ? true : false;
            panel.Controls.Add(txtBox);
            Button addToCart = new Button();
            addToCart.ID = "btnAddToCartProd" + productID;
            addToCart.Text = "Add To Cart";
            addToCart.Width = 100;
            addToCart.Attributes.Add("style", "top: 35px; left: 150px; position: absolute;");
            addToCart.Click += addToCart_Click;
            addToCart.Enabled = unitsInStock != 0 ? true : false;
            panel.Controls.Add(addToCart);
            Page.Form.Controls.Add(panel);
        }

        void addToCart_Click(object sender, EventArgs e)
        {
            string name = ((Button)sender).ID;
            int productID = int.Parse(Regex.Match(name, @"\d+").Value);
            Control textBox = FindControl("txtBoxQuantityProd" + productID);
            int quantity = int.Parse(Regex.Match(((TextBox)textBox).Text, @"\d+").Value);
            //string msg = "txtbox = " + ((TextBox)textBox).ID;
            //Response.Write("<script>alert('" + msg + "')</script>");
            updateShoppingCart(productID, quantity);

        }

        void addProducts()
        {
            int count = dtProducts.Rows.Count;
            DataRow currentRow = dtProducts.Rows[0];
            for (int i = 0; i < count; i++)
            {
                currentRow = dtProducts.Rows[i];
                dbProductInfo.Add(currentRow["ProductID"] + "," + currentRow["ProductName"] + "," + currentRow["UnitPrice"] + "," + currentRow["UnitsInStock"] + "," + currentRow["CategoryID"]);
                // productID,productName,UnitPrice,UnitsInStock,CategoryID - i.e. 1,Chai,28.81,44,1
            }
        }

        void addCategories()
        {
            int count = dtCategories.Rows.Count;
            DataRow currentRow = dtCategories.Rows[0];
            for (int i = 0; i < count; i++)
            {
                currentRow = dtCategories.Rows[i];
                dbCategories.Add((int)currentRow["CategoryID"], (string)currentRow["CategoryName"]);
            }
        }

        int getProductStock(int productID)
        {
            List<string> dbProducts = Session["dbProducts"] != null ? (List<string>)Session["dbProducts"] : null;
            string[] split;
            if (dbProducts != null)
            {
                foreach (string p in dbProducts)
                {
                    split = p.Split(',');
                    if (int.Parse(split[0]) == productID)
                        return int.Parse(split[3]);
                }
            }
            return 0;
        }

        float getProductPrice(int productID)
        {
            List<string> dbProducts = Session["dbProducts"] != null ? (List<string>)Session["dbProducts"] : null;
            string[] split;
            if (dbProducts != null)
            {
                foreach (string p in dbProducts)
                {
                    split = p.Split(',');
                    if (int.Parse(split[0]) == productID)
                        return float.Parse(split[2]);
                }
            }
            return 0.00f;
        }

        string getProductName(int productID)
        {
            List<string> dbProducts = Session["dbProducts"] != null ? (List<string>)Session["dbProducts"] : null;
            string[] split;
            if (dbProducts != null)
            {
                foreach (string p in dbProducts)
                {
                    split = p.Split(',');
                    if (int.Parse(split[0]) == productID)
                        return split[1];
                }
            }
            return "";
        }

        void updateShoppingCart(int productID, int quantity)
        {
            Dictionary<int, int> shoppingCartDictionary = Session["shoppingCart"] != null ? (Dictionary<int, int>)Session["shoppingCart"] : null;
            if (shoppingCartDictionary != null)
            {
                bool productNotFound = true;
                foreach (var p in shoppingCartDictionary)
                    if (productID == p.Key)
                        productNotFound = false;
                if (productNotFound)
                    shoppingCartDictionary.Add(productID, quantity); // not in the dictionary, add it with the quantity
                else // product is in the dictionary, update the quantity
                    shoppingCartDictionary[productID] = shoppingCartDictionary[productID] + 1;
                Session["shoppingCart"] = shoppingCartDictionary;
                updateShoppingCartList();
            }
        }

        void updateShoppingCartList()
        { // update the grid view 
            shoppingCartList.Columns.Clear();
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();
            Dictionary<int, int> shoppingCartDictionary = Session["shoppingCart"] != null ? (Dictionary<int, int>)Session["shoppingCart"] : null;
            if (shoppingCartDictionary != null)
            {
                dt.Columns.Add("Product");
                dt.Columns.Add("Quantity");
                dt.Columns.Add("Price");
                dt.Columns.Add(" "); // for update button
                shoppingCartList.DataSource = dt;
                if (shoppingCartDictionary.Count != 0)
                {
                    int count = 0;
                    List<string> data = new List<string>();
                    foreach (var p in shoppingCartDictionary)
                    {
                        if (count < MAX_ITEMS_IN_CART)
                        {
                            dr = dt.NewRow();
                            dr["Product"] = getProductName(p.Key);
                            double price = Math.Round(getProductPrice(p.Key) * p.Value, 2);
                            dr["Price"] = "$" + price;
                            dt.Rows.Add(dr);
                            shoppingCartList.DataBind();
                            data.Add(p.Value + "," + count + "," + getProductName(p.Key));
                            count++;
                        }
                    }
                    string[] split;
                    TextBox txtBox;
                    Button btn;
                    foreach (string d in data)
                    { // data list is in the format of rowID, cellID, name - i.e. 1,3,Chai 
                        split = d.Split(',');
                        txtBox = new TextBox();
                        txtBox.Width = 50;
                        txtBox.Style["text-align"] = "center";
                        txtBox.Text = split[0];
                        txtBox.ID = "txtBoxProd" + getProductID(split[2]);
                        bindControlsToGridView(int.Parse(split[1]), 1, txtBox);  // rowID, cellID, control
                        btn = new Button();
                        btn.Width = 60;
                        btn.Text = "Update";
                        btn.ID = "btnProd" + getProductID(split[2]);
                        btn.Click += btn_Click;
                        bindControlsToGridView(int.Parse(split[1]), 3, btn); // rowID, cellID, control
                    }
                }
                else
                {
                    dr = dt.NewRow();
                    dr["Product"] = "";
                    dr["Quantity"] = "No items";
                    dr["Price"] = "";
                    dt.Rows.Add(dr);
                    shoppingCartList.DataBind();
                }
            }
            else
            {
                dr = dt.NewRow();
                dr["Product"] = "";
                dr["Quantity"] = "An error occured";
                dr["Price"] = "";
                dt.Rows.Add(dr);
                shoppingCartList.DataBind();
            }
        }

        void btn_Click(object sender, EventArgs e)
        {
            string buttonName = ((Button)sender).ID;
            int productID = int.Parse(Regex.Match(buttonName, @"\d+").Value);
            GridViewRow gvr = (GridViewRow)(sender as Control).Parent.Parent;
            int index = gvr.RowIndex;
            string textBoxName = "txtBoxProd" + productID;
            Control textBox = shoppingCartList.Rows[index].Cells[1].FindControl(textBoxName);
            int quantity = 0;
            try
            { // need to do more checking for when they update the quantity
                quantity = int.Parse(((TextBox)textBox).Text);
                Dictionary<int, int> shoppingCartDictionary = Session["shoppingCart"] != null ? (Dictionary<int, int>)Session["shoppingCart"] : null;
                if (shoppingCartDictionary != null)
                {
                    if (quantity == 0) // assume that they're wanting to remove the product from the cart?
                        shoppingCartDictionary.Remove(productID);
                    else if (quantity <= 500)
                        shoppingCartDictionary[productID] = quantity;
                    Session["shoppingCart"] = shoppingCartDictionary;
                    updateShoppingCartList();
                }
            }
            catch (Exception)
            {

            }
        }

        int getProductID(string productName)
        {
            List<string> dbProducts = Session["dbProducts"] != null ? (List<string>)Session["dbProducts"] : null;
            string[] split;
            if (dbProducts != null)
            {
                foreach (string p in dbProducts)
                {
                    split = p.Split(',');
                    if (split[1] == productName)
                        return int.Parse(split[0]);
                }
            }
            return 0;
        }

        void bindControlsToGridView(int rowID, int cellID, Control control)
        {
            shoppingCartList.Rows[rowID].Cells[cellID].Controls.Add(control);
        }

        protected void btnGoToCart_Click(object sender, EventArgs e)
        {
            Response.Redirect("webform2.aspx");
        }

        protected void shoppingCartButton_Click(object sender, ImageClickEventArgs e)
        {
            updateShoppingCartList();

            if ((bool)ViewState["shopView"])
                ViewState["shopView"] = false;
            else
                ViewState["shopView"] = true;

            ScriptManager.RegisterStartupScript(this.Page, Page.GetType(), "myFunc", "animatePanel(" + (((bool)ViewState["shopView"] == true) ? "true" : "false") + ")", true);
        }
    }

}