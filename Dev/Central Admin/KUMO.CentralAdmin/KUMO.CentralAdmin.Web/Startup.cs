using Hangfire;
using Hangfire.Dashboard;
using KUMO.CentralAdmin.Application;
using KUMO.CentralAdmin.Web.Utilities;
using Microsoft.Owin;
using Owin;
using System.Collections.Generic;

[assembly: OwinStartupAttribute(typeof(KUMO.CentralAdmin.Web.Startup))]
namespace KUMO.CentralAdmin.Web
{
    public class MyRestrictiveAuthorizationFilter : IAuthorizationFilter
    {
        public bool Authorize(IDictionary<string, object> owinEnvironment)
        {
            // In case you need an OWIN context, use the next line,
            // `OwinContext` class is the part of the `Microsoft.Owin` package.
            var context = new OwinContext(owinEnvironment);

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            return context.Authentication.User.Identity.IsAuthenticated && context.Authentication.User.IsInRole("Administrator");
        }
    }

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            SetupHangfireServer(app);

        }

        private static void SetupHangfireServer(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangFireConnection");

            var options = new DashboardOptions
            {
                AuthorizationFilters = new IAuthorizationFilter[] 
                {
                        new MyRestrictiveAuthorizationFilter(),
                    new AuthorizationFilter { Users = "alice.huynh@kumo-eip.com", Roles = "Administrator" },
                    //new ClaimsBasedAuthorizationFilter("name", "value")
                }
            };
            app.UseHangfireDashboard("/hangfire", options);

            //app.UseHangfireDashboard();
            app.UseHangfireServer();
            //run every 2 minutes
            //http://en.wikipedia.org/wiki/Cron

            RecurringJob.AddOrUpdate(() => (new ClientService()).ValidateTenantDatabase(), "*/1 * * * *");// Cron.Minutely);

            RecurringJob.AddOrUpdate(() => (new TrialUserHelper()).CreateTrialDB(), "*/1 * * * *");// Cron.Minutely);

            RecurringJob.AddOrUpdate(() => (new TestMe()).Test(), "*/1 * * * *");// Cron.Minutely);


        }
    }
}
