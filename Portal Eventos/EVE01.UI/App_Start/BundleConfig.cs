using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace EVE01.UI.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").Include(
            "~/vendor/jquery/jquery.min.js",
            "~/vendor/popper.js/umd/popper.min.js",
            "~/vendor/bootstrap/js/bootstrap.min.js",
            "~/vendor/jquery.cookie/jquery.cookie.js",
            "~/vendor/jquery-validation/jquery.validate.min.js",
            "~/js/front.js"));

            bundles.Add(new StyleBundle("~/bundles/styles").Include(
            //"~/vendor/bootstrap/css/bootstrap.min.css",
            "~/fonts/iconos_boostrap.css",
            "~/vendor/font-awesome/css/font-awesome.min.css",
            "~/css/style.blue.css"
            //,
            //"~/css/custom.css"
            ));

        }
    }
}