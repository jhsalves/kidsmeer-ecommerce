using Kidsmeer.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Kidsmeer.Logic;

namespace Kidsmeer
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            // Code that runs on application startup
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Initialize the product database.
            Database.SetInitializer(new ProductDatabaseInitializer());

            RoleActions roleActions = new RoleActions();
            roleActions.CreateAdmin();

            RegisterCustomRoutes(RouteTable.Routes);
        }

        void RegisterCustomRoutes(RouteCollection routes)
        {
            routes.MapPageRoute(
                    "ProductsByCategoryRoute",
                    "Category/{categoryName}",
                    "~/ProductList.aspx"
                );

            routes.MapPageRoute(
                "ProductByNameRoute",
                "Product/{productName}",
                "~/ProductDetails.aspx"
                );
        }

        void Application_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();

            if(exc is HttpUnhandledException)
            {
                if (exc.InnerException != null)
                {
                    exc = new Exception(exc.InnerException.Message);
                    Server.Transfer("ErrorPage.aspx?handler=Application_Error%20-%20Global.asax",
true);
                }
            }
        }
    }
}