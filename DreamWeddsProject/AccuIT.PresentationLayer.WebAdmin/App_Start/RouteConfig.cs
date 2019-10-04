using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace AccuIT.PresentationLayer.WebAdmin
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                name: "WeddingPage",
                url: "{wedding}/{Id}",
                defaults: new { controller = "Wedding", action = "Index" },
                constraints: new { Id = "\\d+" }
);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                 defaults: new { controller = "Account", action = "Login", id = UrlParameter.Optional },
                namespaces: new[] { "AccuIT.PresentationLayer.WebAdmin.Controllers" }
            );
        }
    }
}