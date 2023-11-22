using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FourthWebApp
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "ShowTime", action = "Index", id = UrlParameter.Optional }
            );

            //            routes.MapRoute(
            //    name: "ShowTime",
            //    url: "ShowTime/{action}/{id}",
            //    defaults: new { controller = "ShowTime", action = "Index", id = UrlParameter.Optional }
            //);

        }
    }
}
