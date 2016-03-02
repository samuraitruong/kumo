using System.Web;
using System.Web.Optimization;

namespace test_kumo_eip0001web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            
            RegisterLibraries();
            RegisterApplication();
            RegisterStyles(bundles);
        }

        private static void RegisterStyles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/themes/bootstrap/bootstrap.min.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-table.min.css",
                "~/Content/site.css",
                "~/Content/main.css",
                "~/Content/toaster.css",
                "~/Content/jquery.fancybox.css",
                "~/Content/jquery.fancybox-buttons.css",
                "~/Content/jquery.fancybox-thumbs.css",
                "~/Content/bootstrap-vertical-tabs-1.2.1/bootstrap.vertical-tabs.css"
                ));
        }

        private static void RegisterLibraries()
        {
            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.Libraries)
                .IncludeDirectory("~/Scripts/Libraries", "*.js", false);
            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.Libraries)
                .IncludeDirectory("~/Scripts/tinymce", "*.js", false);

            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.Libraries)
                .IncludeDirectory("~/Scripts/fancybox", "*.js", false);

        }

        private static void RegisterApplication()
        {
            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.ApplicationPreRequisuites)
                .IncludeDirectory("~/Scripts/Application/Prereq", "*.js", true);
            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.ApplicationServices)
                .IncludeDirectory("~/Scripts/Application/Services", "*.js", true);
            Bundles.GetJavaScriptBundle(Bundles.JavaScriptBundle.ApplicationView)
                .IncludeDirectory("~/Scripts/Application/View", "*.js", true);

        }
    }
}
