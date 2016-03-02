using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Kumo.Web.Startup))]
namespace Kumo.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
