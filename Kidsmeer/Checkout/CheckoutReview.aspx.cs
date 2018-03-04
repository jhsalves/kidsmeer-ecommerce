using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Kidsmeer.Models;
using Kidsmeer.Logic;

namespace Kidsmeer.Checkout
{
    public partial class CheckoutReview : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                NVPAPICaller paypalCaller = new NVPAPICaller();

                string retMsg = "";
                string token = "";
                string PayerID = "";
                NVPCodec decoder = new NVPCodec();
                token = Session["token"].ToString();

                bool ret = paypalCaller.GetCheckoutDetails(token, ref PayerID, ref decoder, ref retMsg);

                if (ret)
                {
                    Session["payerId"] = PayerID;

                    var myOrder = new Order();

                    myOrder.OrderDate = Convert.ToDateTime(decoder["TIMESTAMP"].ToString());
                    myOrder.Username = User.Identity.Name;
                    myOrder.FirstName = decoder["FIRSTNAME"].ToString();
                    myOrder.LastName = decoder["LASTNAME"].ToString();
                    myOrder.Address = decoder["SHIPTOSTREET"].ToString();
                    myOrder.City = decoder["SHIPTOCITY"].ToString();
                    myOrder.State = decoder["SHIPTOSTATE"].ToString();
                    myOrder.PostalCode = decoder["SHIPTOZIP"].ToString();
                    myOrder.Country = decoder["SHIPTOCOUNTRYCODE"].ToString();
                    myOrder.Email = decoder["EMAIL"].ToString();
                    myOrder.Total = Convert.ToDecimal(decoder["AMT"].ToString());

                    try
                    {
                        decimal paymentAmountOnCheckout = Convert.ToDecimal(Session["payment_amt"].ToString());
                        decimal paymentAmountFromPayPal = Convert.ToDecimal(decoder["AMT"].ToString());
                        if (paymentAmountOnCheckout != paymentAmountFromPayPal/100)
                        {
                            Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                        }
                    }
                    catch(Exception)
                    {
                        Response.Redirect("CheckoutError.aspx?" + "Desc=Amount%20total%20mismatch.");
                    }

                    ProductContext _db = new ProductContext();

                    _db.Orders.Add(myOrder);
                    _db.SaveChanges();

                    using (ShoppingCartActions userShoppingCart = new ShoppingCartActions())
                    {
                        List<CartItem> myOrderList = userShoppingCart.GetCartItems();
                        foreach (var item in myOrderList) {
                            var myOrderDetail = new OrderDetail();
                            myOrderDetail.OrderId = myOrder.OrderId;
                            myOrderDetail.Username = User.Identity.Name;
                            myOrderDetail.ProductId = item.ProductId;
                            myOrderDetail.Quantity = item.Quantity;
                            myOrderDetail.UnitPrice = item.Product.UnitPrice;

                            _db.OrderDetails.Add(myOrderDetail);
                            _db.SaveChanges();
                        }

                        Session["currentOrderId"] = myOrder.OrderId;

                        List<Order> orderList = new List<Order>();
                        orderList.Add(myOrder);
                        ShipInfo.DataSource = orderList;
                        ShipInfo.DataBind();

                        OrderItemList.DataSource = myOrderList;
                        OrderItemList.DataBind();
                    }
                }
                else
                {
                    Response.Redirect("CheckoutError.aspx?" + retMsg);
                }
            }
        }

        protected void CheckoutConfirm_Click(object sender, EventArgs e)
        {
            Session["userCheckoutCompleted"] = "true";
            Response.Redirect("~/Checkout/CheckoutComplete.aspx");
        }
    }
}