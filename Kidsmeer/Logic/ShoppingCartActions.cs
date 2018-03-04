using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kidsmeer.Models;

namespace Kidsmeer.Logic
{
    public class ShoppingCartActions : IDisposable
    {
        public string ShoppingCartId { get; set; }

        private ProductContext _db = new ProductContext();

        public const string CartSessionKey = "CartId";

        public void AddToCart(int id)
        {
            ShoppingCartId = GetCartId();

            var cartItem = _db.ShoppingCartItems.SingleOrDefault(
                c => c.CartId == ShoppingCartId
                && c.ProductId == id
                );
            if (cartItem == null)
            {
                cartItem = new CartItem
                {
                    ItemId = Guid.NewGuid().ToString(),
                    ProductId = id,
                    CartId = ShoppingCartId,
                    Product = _db.Products.SingleOrDefault(
                        p => p.ProductID == id),
                    Quantity = 1,
                    DateCreated = DateTime.Now
                };

                _db.ShoppingCartItems.Add(cartItem);
            }
            else {
                cartItem.Quantity++;
            }
            _db.SaveChanges();
        }

        public string GetCartId() {
            if(HttpContext.Current.Session[CartSessionKey] == null)
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.User.Identity.Name))
                {
                    HttpContext.Current.Session[CartSessionKey] = HttpContext.Current.User.Identity.Name;
                }
                else
                {
                    Guid tempCartId = Guid.NewGuid();
                    HttpContext.Current.Session[CartSessionKey] = tempCartId.ToString();
                }
            }
            return HttpContext.Current.Session[CartSessionKey].ToString();
        }

        public List<CartItem> GetCartItems()
        {
            ShoppingCartId = GetCartId();

            return _db.ShoppingCartItems.Where(
                sc => sc.CartId == ShoppingCartId
                ).ToList();
        }

        public void Dispose()
        {
            if (_db != null)
            {
                _db.Dispose();
                _db = null;
            }
        }

        public decimal GetTotal() {
            ShoppingCartId = GetCartId();
            decimal? total = decimal.Zero;
            total = (decimal?)(from cartItems in _db.ShoppingCartItems
                               where cartItems.CartId == ShoppingCartId
                               select (int?)cartItems.Quantity * cartItems.Product.UnitPrice).Sum();
            return total ?? decimal.Zero;
        }

        public struct ShoppingCartUpdates
        {
            public int ProductId;
            public int PurchaseQuantity;
            public bool RemoveItem;
        }

        public void UpdateShoppingCartDatabase(String cartId, List<ShoppingCartUpdates> cartUpdates)
        {
            using(var db = new Kidsmeer.Models.ProductContext())
            {
                try
                {
                    List<CartItem> myCart = GetCartItems();
                    foreach(var cartUpdateItem in cartUpdates)
                    {
                        var currentCartItem = myCart.SingleOrDefault(ci => ci.ProductId == cartUpdateItem.ProductId);
                        if (currentCartItem != null) {
                            if (cartUpdateItem.PurchaseQuantity < 1 || cartUpdateItem.RemoveItem)
                            {
                                RemoveItem(currentCartItem.CartId, currentCartItem.ProductId);
                            }
                            else {
                                UpdateItem(cartId, currentCartItem.ProductId, cartUpdateItem.PurchaseQuantity);
                            }
                        }
                    }
                }
                catch(Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Database - " + exp.Message.ToString(), exp);
                }
            }
        }

        public int GetCount() {
            ShoppingCartId = GetCartId();
            int? count = (from cartItems in _db.ShoppingCartItems
                          where cartItems.CartId == ShoppingCartId
                          select (int?)cartItems.Quantity).Sum();

            return count ?? 0;
        }

        public void UpdateItem(string updateCartID,int updateProductID, int quantity)
        {
            using (var _db = new Kidsmeer.Models.ProductContext())
            {
                try
                {
                    var myItem = (from c in _db.ShoppingCartItems
                                  where c.CartId == updateCartID && c.ProductId == updateProductID
                                  select c).FirstOrDefault();
                    if (myItem != null) {
                        myItem.Quantity = quantity;
                        _db.SaveChanges();
                    }
                }catch(Exception exp)
                {
                    throw new Exception("ERROR: Unable to Update Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void RemoveItem(string removeCartID, int removeProductID)
        {
            using (var _db = new Kidsmeer.Models.ProductContext())
            {
                try {
                    var myItem = (from c in _db.ShoppingCartItems
                                 where c.CartId == removeCartID && c.ProductId == removeProductID
                                 select c).FirstOrDefault();
                    if(myItem != null)
                    {
                        _db.ShoppingCartItems.Remove(myItem);
                        _db.SaveChanges();
                    }
                }
                catch(Exception exp)
                {
                    throw new Exception("ERROR: Unable to Remove Cart Item - " + exp.Message.ToString(), exp);
                }
            }
        }

        public void EmptyCart()
        {
            string ShoppingCartId = GetCartId();
            var cartItems = _db.ShoppingCartItems;

            var cartItemsResult = cartItems.Where(ci => ci.CartId == ShoppingCartId);
            foreach(var CartItem in cartItemsResult)
            {
                cartItems.Remove(CartItem);
            }
            _db.SaveChanges();
        }
    }
}