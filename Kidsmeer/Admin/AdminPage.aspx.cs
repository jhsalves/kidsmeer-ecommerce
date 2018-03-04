using Kidsmeer.Logic;
using Kidsmeer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Kidsmeer.Admin
{
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string productAction = Request.QueryString["ProductAction"];
            if (productAction == "add")
            {
                LabelAddStatus.Text = "Product added!";
            }

            if (productAction == "remove")
            {
                LabelRemoveStatus.Text = "Product removed!";
            }
        }

        public IQueryable<Product> GetProducts()
        {
            var _db = new ProductContext();
            IQueryable<Product> query = _db.Products;
            return query;
        }

        public IQueryable<Category> GetCategories() {
            var _db = new ProductContext();
            return _db.Categories;
        }

        protected void AddProductButton_Click(object sender, EventArgs e)
        {
            string path = Server.MapPath("~/Catalog/Images/");
            if (ProductImage.HasFile)
            {
                string fileExtension = System.IO.Path.GetExtension(ProductImage.FileName).ToLower();
                string[] allowedextensions = { ".gif", ".png", ".jpeg", ".jpg" };
                if (allowedextensions.Contains(fileExtension))
                {
                    try
                    {
                        ProductImage.PostedFile.SaveAs(path + ProductImage.FileName);
                        ProductImage.PostedFile.SaveAs(path + "Thumbs/" + ProductImage.FileName);
                    }
                    catch (Exception ex)
                    {
                        LabelAddStatus.Text = ex.Message;
                    }
                }
            }
            else
            {
                LabelAddStatus.Text = "Unable to accept file type.";
            }

            AddProducts products = new AddProducts();

            bool addSuccess = products.AddProduct(AddProductName.Text, AddProductDescription.Text, AddProductPrice.Text, DropDownAddCategory.SelectedValue, ProductImage.FileName);
            if (addSuccess)
            {
                string pageUrl = Request.Url.AbsoluteUri.Substring(0,
 Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                Response.Redirect(pageUrl + "?ProductAction=add");
            }
            else
            {
                LabelAddStatus.Text = "Unable to add new product to database.";
            }
        }

        protected void RemoveProductButton_Click(object sender, EventArgs e) {
            var _db = new ProductContext();
            var productId = Convert.ToInt32(DropDownRemoveProduct.SelectedValue);

            var product = _db.Products.FirstOrDefault(p => p.ProductID == productId);

            if(product != null)
            {
                _db.Products.Remove(product);
                _db.SaveChanges();
                string pageUrl = Request.Url.AbsoluteUri.Substring(0,
Request.Url.AbsoluteUri.Count() - Request.Url.Query.Count());
                Response.Redirect(pageUrl + "?ProductAction=remove");
            }
            else
            {
                LabelRemoveStatus.Text = "Unable to locate product.";
            }
        }
    }
}