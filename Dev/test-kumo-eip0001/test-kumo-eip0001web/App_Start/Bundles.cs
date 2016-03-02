using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace test_kumo_eip0001web
{
    public static class Bundles
    {

        private static readonly Dictionary<JavaScriptBundle, string> jsBundles;

        static Bundles()
        {

            jsBundles = new Dictionary<JavaScriptBundle, string>
            {
                {JavaScriptBundle.Libraries, "~/bundles/scripts/libraries"}, 
                {JavaScriptBundle.ApplicationPreRequisuites, "~/bundles/scripts/application/prereq"},
                {JavaScriptBundle.ApplicationServices, "~/bundles/scripts/application/services"},
                {JavaScriptBundle.ApplicationView, "~/bundles/scripts/application/view"},
            };
        }

        public static Bundle GetJavaScriptBundle(JavaScriptBundle bundle)
        {
            var result = BundleTable.Bundles.SingleOrDefault(x => x.Path == jsBundles[bundle]) ?? CreateJavaScriptBundle(bundle);

            return result;
        }

        public static Bundle CreateJavaScriptBundle(JavaScriptBundle bundle)
        {
            var jsBundle = new Bundle(jsBundles[bundle]) { Orderer = new BundleOrderer() };

            BundleTable.Bundles.Add(jsBundle);

            return jsBundle;
        }

        public enum JavaScriptBundle
        {
            ApplicationView,
            Libraries,
            ApplicationPreRequisuites,
            ApplicationServices
        }
    }
}