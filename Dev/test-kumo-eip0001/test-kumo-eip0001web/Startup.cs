using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(test_kumo_eip0001web.Startup))]
namespace test_kumo_eip0001web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
