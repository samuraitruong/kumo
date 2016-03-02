using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace KUMO.CentralAdmin.Web
{
    public static class Bundles
    {

        private static readonly Dictionary<JavaScriptBundle, string> jsBundles;

        static Bundles()
        {

            jsBundles = new Dictionary<JavaScriptBundle, string>
            {
                {JavaScriptBundle.Libraries, "~/bundles/scripts/libraries"}, 
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
            Libraries,
        }
    }
}