using System.Web.Mvc;
using System.Web.Routing;

namespace TheTechShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            // BotDetect requests must not be routed
            routes.IgnoreRoute("{*botdetect}", new { botdetect = @"(.*)BotDetectCaptcha\.ashx" });

            routes.MapRoute(
               name: "Search",
               url: "Search",
               defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Login",
               url: "Login",
               defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
            );
            routes.MapRoute(
                name: "Cart",
                url: "Cart",
                defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
                namespaces: new string[] { "TheTechShop.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Register",
               url: "Register",
               defaults: new { controller = "Account", action = "Register", id = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Checkout",
               url: "Checkout",
               defaults: new { controller = "ShoppingCart", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Page",
               url: "trang/{alias}",
               defaults: new { controller = "Page", action = "Index", alias = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
             );
            routes.MapRoute(
               name: "TagList",
               url: "tag/{tagId}",
               defaults: new { controller = "Product", action = "ListByTag", tagId = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
             );
            routes.MapRoute(
               name: "Product Category",
               url: "{alias}/productcategory/{id}",
               defaults: new { controller = "Product", action = "Category", id = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
             );
            routes.MapRoute(
               name: "Product",
               url: "{alias}/p/{productId}",
               defaults: new { controller = "Product", action = "Detail", productId = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
            );
            routes.MapRoute(
               name: "Default",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
               namespaces: new string[] { "TheTechShop.Web.Controllers" }
           );
        }
    }
}