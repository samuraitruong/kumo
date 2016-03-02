using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using test_kumo_eip0001web.Utility;

namespace test_kumo_eip0001web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(name: KumoConstants.DMS_ROOT_ROUTE_NAME,
              url: "dms",
              defaults: new { controller = "DMS", action = "index"});

            routes.MapRoute(name: KumoConstants.DOCUMENT_ROOT_ROUTE_NAME,
               url: "dms/{doclib}/{action}/{uuid}",
               defaults: new { controller = "Document", action = "index", id = 1, doclib = "" ,  uuid=""});

            routes.MapRoute(name: "Documents1",
                url: "dms/{doclib}/{folder}/{action}",
                defaults: new { controller = "Document", action = "index", id = 1, doclib = "", folder = "" });


            routes.MapRoute(name: "Documents",
                url: "dms/{doclib}/{folder}/{subfolder1}/{action}",
                defaults: new { controller = "Document", action = "index", id = 1, doclib="", folder="" });


            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
