using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PresentationLayer.DreamWedds.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();

            routes.MapRoute(
                 name: "BlogPage",
                 url: "{blogId}/{blogName}",
                 defaults: new { controller = "Blog", action = "BlogDetail" },
                 constraints: new { blogId = "\\d+" }
);

            routes.MapRoute(
                name: "TemplatePage",
                url: "{template}/{templateID}",
                defaults: new { controller = "Template", action = "Details" },
                constraints: new { templateId = "\\d+" }
);

            routes.MapRoute(
                name: "WeddingPage",
                url: "{Wedding}/{Id}/{title}",
                defaults: new { controller = "Wedding", action = "Index" }
//constraints: new { Id = "\\d+" }
);

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
 
        }
    }
}
