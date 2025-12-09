using Business.Model;
using BusinessLayer.Model;
using BusinessLayer.Model.API;
using BusinessLayer.Repository;
using LSPosMVC.App_Start;
using LSPosMVC.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.UI.WebControls;
using Ticket;
using Ticket.Utils;

namespace LSPosMVC
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private SqlDataHelper helper = new SqlDataHelper();
        protected void Application_End()
        {
            //Stop SQL dependency
            SqlDependency.Stop(helper.GetConnectionString());

        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            //SwaggerConfig.Register();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            SqlDependency.Start(helper.GetConnectionString());
        }
    }
}
