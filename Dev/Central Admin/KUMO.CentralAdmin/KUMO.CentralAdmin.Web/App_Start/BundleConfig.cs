using System.Web;
using System.Web.Optimization;

namespace KUMO.CentralAdmin.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyles(bundles);
            RegisterLibraries();

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-1.10.2.js", 
                        "~/Scripts/jquery-migrate-1.2.1.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/bootstrap/bootstrap.min.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-table.min.css",
                "~/Content/site.css",
                "~/Content/main.css",
                "~/Content/pagedlist.css"
                ));
        }

        private static void RegisterLibraries()
        {
            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.Libraries)
                .IncludeDirectory("~/Scripts", "*.js", false);
        }

    }
}
