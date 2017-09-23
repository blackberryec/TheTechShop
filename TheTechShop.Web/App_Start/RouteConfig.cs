﻿using System.Web.Mvc;
using System.Web.Routing;

namespace TheTechShop.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
         routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

          routes.MapRoute(
              name: "Search",
              url: "Tim-Kiem",
              defaults: new { controller = "Product", action = "Search", id = UrlParameter.Optional },
              namespaces: new string[] { "TheTechShop.Web.Controllers" }
          );
            routes.MapRoute(
         name: "Login",
         url: "dang-nhap.html",
         defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
         namespaces: new string[] { "TheTechShop.Web.Controllers" }
        );
        // routes.MapRoute(
        // name: "About",
        // url: "gioi-thieu.html",
        // defaults: new { controller = "About", action = "Index", id = UrlParameter.Optional },
        // namespaces: new string[] { "TheTechShop.Web.Controllers" }
        //);
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