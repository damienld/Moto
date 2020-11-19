using System.Web;
using System.Web.Optimization;

namespace PMotoWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery.unobtrusive-ajax.js"))
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
   "~/Scripts/bootstrap.js",
   "~/Scripts/bootstrap.min.js",
   "~/Scripts/moment.min.js",
   "~/Scripts/respond.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap-table").Include(
   "~/Scripts/bootstrap-table/tableExport.min.js",
   "~/Scripts/bootstrap-table/bootstrap-table.min.js"
   //"~/Scripts/bootstrap-table/bootstrap-table-locale-all.min.js",
   //"~/Scripts/bootstrap-table/extensions/export/bootstrap-table-export.min.js"
   ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
   "~/Content/bootstrap.min.css",
   "~/Content/bootstrap-table.min.css",
   "~/Content/site.css"));
        }
    }
}
