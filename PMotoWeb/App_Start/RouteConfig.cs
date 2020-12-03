using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PMotoWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

           /* routes.MapRoute(
                name: "MotoStats",
                url: "{controller}/{action}/{selectedSeasonId}",
                defaults: new { controller = "MotoStats", action = "Index", selectedSeasonId = UrlParameter.Optional }
            );*/
            routes.MapRoute(
                name: "RiderSessionsToChart",
                url: "GpResults/RiderSessionsToChart/{idSession}/{nbLapsForAvg}/{nbLapsForAvgWithTyres}/{minTyreLapsFront}/{minTyreLapsRear}/{riderRange}",
                defaults: new { controller = "GpResults", action = "RiderSessionsToChart",
                    idSession = UrlParameter.Optional
                ,
                    nbLapsForAvg = UrlParameter.Optional
                ,
                    nbLapsForAvgWithTyres = UrlParameter.Optional
                    ,
                    minTyreLapsFront = UrlParameter.Optional,
                    minTyreLapsRear = UrlParameter.Optional,
                    riderRange = UrlParameter.Optional
                }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            
        }
    }
}
