using bcast.common;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Unity.WebApi;

namespace bcast.server
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
            var container = UnityConfig.RegisterComponents();
            
            // TODO: Call data access method to get backed up cache id table
            var backup = "";

            if(!String.IsNullOrWhiteSpace(backup))
            {
                var cache = (ICacheAccess)container.Resolve(typeof(ICacheAccess),null);
                cache.Restore(backup);
            }
            var logger = (ILogger)container.Resolve(typeof(ILogger), null);
            logger.Initialize();

            Application["unity"] = container;
        }

        protected void Application_End()
        {
            var container = Application["unity"] as UnityContainer;
            if(container!=null)
            {
                var cache = (ICacheAccess)container.Resolve(typeof(ICacheAccess), null);
                var data = (IDataAccess)container.Resolve(typeof(IDataAccess), null);

                var backup = cache.Backup();
                //TODO: call method on data access to save backup in db
            }
        }
    }
}
