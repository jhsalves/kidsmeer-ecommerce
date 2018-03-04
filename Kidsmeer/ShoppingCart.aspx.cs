using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kidsmeer.Logic;
using Kidsmeer.Models;
using System.Collections.Specialized;
using System.Collections;
using System.Web.ModelBinding;

namespace Kidsmeer
{
    public partial class WebForm2 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {
                decimal cartTotal = 0;
                cartTotal = usersShoppingCart.GetTotal();
                if (cartTotal > 0)
                {
                    lblTotal.Text = String.Format("{0:c}", cartTotal);
                }
                else
                {
                    LabelTotalText.Text = "";
                    lblTotal.Text = "";
                    ShoppingCartTitle.InnerText = "Shopping Cart is Empty";
                    UpdateBtn.Visible = false;
                    CheckoutImageBtn.Visible = false;
                }
            }
        }

        public List<CartItem> GetShoppingCartItems()
        {
            ShoppingCartActions actions = new ShoppingCartActions();
            return actions.GetCartItems();
        }

        public List<CartItem> UpdateCartItems()
        {
            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {
                String cartId = usersShoppingCart.GetCartId();

                List<ShoppingCartActions.ShoppingCartUpdates> cartUpdates = new List<ShoppingCartActions.ShoppingCartUpdates>();
                foreach (GridViewRow row in CartList.Rows)
                {
                    IOrderedDictionary rowValues = new OrderedDictionary();
                    rowValues = GetValues(row);
                    var ControlCheckbox = (CheckBox)row.FindControl("Remove");
                    var TextBox = (TextBox)row.FindControl("PurchaseQuantity");
                    ShoppingCartActions.ShoppingCartUpdates cartUpdate = new ShoppingCartActions.ShoppingCartUpdates
                    {
                        ProductId = Convert.ToInt32(rowValues["ProductID"]),
                        RemoveItem = ControlCheckbox.Checked,
                        PurchaseQuantity = Convert.ToInt16(TextBox.Text.ToString())
                    };
                    cartUpdates.Add(cartUpdate);
                }
                usersShoppingCart.UpdateShoppingCartDatabase(cartId,cartUpdates);
                CartList.DataBind();
                lblTotal.Text = String.Format("{0:c}",usersShoppingCart.GetTotal());
                return usersShoppingCart.GetCartItems();
            }
        }

        public static IOrderedDictionary GetValues(GridViewRow row)
        {
            IOrderedDictionary values = new OrderedDictionary();
            foreach (DataControlFieldCell cell in row.Cells)
            {
                if (cell.Visible)
                {
                    cell.ContainingField.ExtractValuesFromCell(values, cell, row.RowState, true);
                }

            }
            return values;
        }

        protected void UpdateBtn_Click(object sender, EventArgs e)
        {
            UpdateCartItems();
        }

        protected void CheckoutBtn_Click(object sender, ImageClickEventArgs e)
        {
            using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
            {
                Session["payment_amt"] = usersShoppingCart.GetTotal();
            }
            Response.Redirect("Checkout/CheckoutStart.aspx");
        }
    }
}