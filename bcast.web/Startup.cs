using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(bcast.web.Startup))]
namespace bcast.web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
