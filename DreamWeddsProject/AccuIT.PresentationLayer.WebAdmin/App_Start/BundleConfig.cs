using System.Web;
using System.Web.Optimization;

namespace AccuIT.PresentationLayer.WebAdmin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.UseCdn = true;
            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));



            bundles.Add(new StyleBundle("~/Exclusive/css").Include(
                           "~/Content/Exclusive/bower_components/bootstrap/dist/css/bootstrap.min.css",
                           "~/Content/Exclusive/bower_components/font-awesome/css/font-awesome.min.css",
                           "~/Content/Exclusive/bower_components/Ionicons/css/ionicons.min.css",
                           "~/Content/Exclusive/bower_components/datatables.net-bs/css/dataTables.bootstrap.min.css",
                           "~/Content/Exclusive/dist/css/AdminLTE.min.css",
                           "~/Content/Exclusive/dist/css/skins/_all-skins.min.css"));


            bundles.Add(new ScriptBundle("~/Exclusive/scripts").Include(
                        "~/Content/Exclusive/bower_components/jquery/dist/jquery.min.js",
                        "~/Content/Exclusive/bower_components/bootstrap/dist/js/bootstrap.min.js",
                        "~/Content/Exclusive/bower_components/jquery-slimscroll/jquery.slimscroll.min.js",
                        "~/Content/Exclusive/bower_components/datatables.net/js/jquery.dataTables.min.js",
                        "~/Content/Exclusive/bower_components/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                        "~/Content/Exclusive/bower_components/datatables.net-bs/js/dataTables.bootstrap.min.js",
                        "~/Content/Exclusive/bower_components/fastclick/lib/fastclick.js",
                        "~/Content/Exclusive/dist/js/adminlte.min.js",
                        "~/Content/Exclusive/dist/js/demo.js"));

            bundles.Add(new StyleBundle("~/Classic/css").Include(
                        "~/Content/Classic/bootstrap/css/bootstrap.min.css",
                        "~/Content/Classic/plugins/datatables/dataTables.bootstrap.css",
                        "~/Content/Classic/dist/css/AdminLTE.min.css",
                        "~/Content/Classic/dist/css/skins/_all-skins.min.css"));

            bundles.Add(new ScriptBundle("~/Classic/scripts").Include(
                      "~/Content/Classic/plugins/jQuery/jQuery-2.1.4.min.js",
                      "~/Content/Classic/bootstrap/js/bootstrap.min.js",
                      "~/Content/Classic/plugins/datatables/jquery.dataTables.min.js",
                      "~/Content/Classic/plugins/datatables/dataTables.bootstrap.min.js",
                      "~/Content/Classic/plugins/datepicker/bootstrap-datepicker.js",
                      "~/Content/Classic/plugins/slimScroll/jquery.slimscroll.min.js",
                      "~/Content/Classic/plugins/fastclick/fastclick.min.js",
                      "~/Content/Classic/dist/js/app.min.js",
                      "~/Content/Classic/dist/js/demo.js"));



            BundleTable.EnableOptimizations = true;
        }
    }
}