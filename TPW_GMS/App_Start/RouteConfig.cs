using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.AspNet.FriendlyUrls;

namespace TPW_GMS
{
    public static class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            var settings = new FriendlyUrlSettings();
            settings.AutoRedirectMode = RedirectMode.Permanent;
            routes.EnableFriendlyUrls(settings);

            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapPageRoute(
                "WebFormDefault",
                "",
                "~/SignIn.aspx"
            );

            routes.MapRoute(
                    name: "SignIn",
                    url: "{controller}/{action}/{id}",
                    defaults: new { action = "SignIn", id = UrlParameter.Optional }
                );  
        }
    }
}
