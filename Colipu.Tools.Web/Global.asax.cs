using Colipu.Tools.Business;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Colipu.Tools.Web
{
    public class MvcApplication : HttpApplication
    {
        public static readonly string AppVersion;

        static MvcApplication()
        {
            AppVersion = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);             
            LogBll.Jobs();
            
        }
    }
}