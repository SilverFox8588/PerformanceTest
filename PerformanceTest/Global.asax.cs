using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure.Interception;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using DevExpress.Web.Mvc;
using Repository;

namespace PerformanceTest
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            DbInterception.Add(new Interceptor());

            AreaRegistration.RegisterAllAreas();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            AuthConfig.RegisterAuth();

            ModelBinders.Binders.DefaultBinder = new DevExpressEditorsBinder();
            
            DevExpress.Web.ASPxWebControl.CallbackError += Application_Error;
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            //Exception exception = System.Web.HttpContext.Current.Server.GetLastError();
            //TODO: Handle Exception
        }

        protected void Application_PreRequestHandlerExecute(object sender, EventArgs e)
        {
            //DevExpressHelper.Theme = ;
        }
    }
}