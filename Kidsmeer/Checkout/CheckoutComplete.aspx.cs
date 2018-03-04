using Kidsmeer.Logic;
using Kidsmeer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kidsmeer.Checkout
{
    public partial class CheckoutComplete : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if((string)Session["userCheckoutCompleted"] != "true")
                {
                    Session["userCheckoutCopleted"] = string.Empty;
                    Response.Redirect("CheckoutError.aspx?" + "Desc=Unvalidate%20Checkout.");
                }

                NVPAPICaller payPalCaller = new NVPAPICaller();

                string retMsg = "";
                string token = "";
                string finalPaymentAmount = "";
                string PayerID = "";
                NVPCodec decoder = new NVPCodec();

                token = Session["token"].ToString();
                PayerID = Session["payerID"].ToString();
                finalPaymentAmount = (Convert.ToInt32(Session["payment_amt"]) * 100).ToString() ;

                bool ret = payPalCaller.DoCheckoutPayment(finalPaymentAmount, token, PayerID, ref decoder, ref retMsg);
                if (ret)
                {
                    string PaymentConfirmation = decoder["PAYMENTINFO_0_TRANSACTIONID"].ToString();
                    TransactionId.Text = PaymentConfirmation;

                    ProductContext _db = new ProductContext();
                    int currentOrderId = -1;
                    if(Session["currentOrderID"].ToString() != string.Empty)
                    {
                        currentOrderId = Convert.ToInt32(Session["currentOrderID"]);
                    }
                    Order myCurrentOrder;
                    if(currentOrderId >= 0)
                    {
                        myCurrentOrder = _db.Orders.Single(
                            o => o.OrderId == currentOrderId);

                        myCurrentOrder.PaymentTransactionId = PaymentConfirmation;
                        _db.SaveChanges();
                    }

                    using (ShoppingCartActions usersShoppingCart = new ShoppingCartActions())
                    {
                        usersShoppingCart.EmptyCart();
                    }

                    Session["currentOrderId"] = string.Empty;
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
        }

        protected void Continue_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }
    }
}