using System.Web;
using System.Web.Optimization;

namespace PresentationLayer.DreamWedds.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/JsScripts").Include(
                      "~/content/js/jquery/jquery.min.js",
                      "~/content/js/bootstrap/bootstrap.min.js",
                      "~/content/js/bootstrap/bootstrap-hover-dropdown.js",
                      "~/content/js/wow/wow.min.js",
                      "~/content/js/imagesloaded/imagesloaded.pkgd.min.js",
                      "~/content/js/SmoothScroll/SmoothScroll.min.js",
                      "~/content/js/waypoints.min.js",
                      "~/content/js/jquery.counterup.min.js",
                      "~/content/js/hero-slider/modernizr.js",
                      "~/content/js/hero-slider/main.js",
                      "~/content/js/owl.carousel.js",
                      "~/content/js/jquery/jquery.filterizr.js",
                      "~/content/js/edura.js"));

            bundles.Add(new StyleBundle("~/Content/css/styles").Include(
                "~/Content/css/styles/lunar.css",
                      "~/Content/css/style.css",
                      "~/Content/css/custom.css",
                      "~/Content/css/demo.css"));
        }
    }
}
