using bcast.common;
using bcast.data.sqlserver;
using bcast.log4net;
using Microsoft.Practices.Unity;
using System.Web.Http;
using Unity.WebApi;

namespace bcast.server
{
    public static class UnityConfig
    {
        public static UnityContainer RegisterComponents()
        {
			var container = new UnityContainer();
            
            container.RegisterType<IDataAccess, DataAccessImpl>();
            container.RegisterType<ICacheAccess>(new InjectionFactory(c =>
                {
                    var ca = new AspNetCacheAccess();
                    ca.Persist = (account, id, dest, data) =>
                    {
                        var da = new DataAccessImpl();
                        // TODO: Call method on data access to save cache item in Items table
                        return true;
                    };
                    ca.Remove = (id) =>
                    {
                        // TODO: Call method on data access to remove the item from the Items table
                        return true;
                    };
                    return ca;
                }));
            container.RegisterType<ILogger, Logger>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);

            return container;
        }

        private static string BackupCache()
        {
            throw new System.NotImplementedException();
        }
    }
}