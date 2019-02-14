using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace TeknikServis.Web.UI.Areas.Admin
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            ///Areas/Admin
            // Vendor scripts
            bundles.Add(new ScriptBundle("~/Areas/Admin/bundles/jquery").Include(
                        "~/Areas/Admin/Scripts/jquery-2.1.1.min.js"));

            // jQuery Validation
            bundles.Add(new ScriptBundle("~/Areas/Admin/bundles/jqueryval").Include(
            "~/Areas/Admin/Scripts/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/Areas/Admin/bundles/bootstrap").Include(
                      "~/Areas/Admin/Scripts/bootstrap.min.js"));

            // Inspinia script
            bundles.Add(new ScriptBundle("~/Areas/Admin/bundles/inspinia").Include(
                      "~/Areas/Admin/Scripts/app/inspinia.js"));

            // SlimScroll
            bundles.Add(new ScriptBundle("~/Areas/Admin/plugins/slimScroll").Include(
                      "~/Areas/Admin/Scripts/plugins/slimScroll/jquery.slimscroll.min.js"));

            // jQuery plugins
            bundles.Add(new ScriptBundle("~/Areas/Admin/plugins/metsiMenu").Include(
                      "~/Areas/Admin/Scripts/plugins/metisMenu/metisMenu.min.js"));

            bundles.Add(new ScriptBundle("~/Areas/Admin/plugins/pace").Include(
                      "~/Areas/Admin/Scripts/plugins/pace/pace.min.js"));

            // CSS style (bootstrap/inspinia)
            bundles.Add(new StyleBundle("~/Areas/Admin/Content/css").Include(
                      "~/Areas/Admin/Content/bootstrap.min.css",
                      "~/Areas/Admin/Content/animate.css",
                      "~/Areas/Admin/Content/style.css"));

            // Font Awesome icons
            bundles.Add(new StyleBundle("~/Areas/Admin/font-awesome/css").Include(
                      "~/fonts/font-awesome/css/font-awesome.min.css", new CssRewriteUrlTransform()));

        }
    }
}