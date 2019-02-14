using System;
using System.Web.Mvc;
using System.Web.Optimization;

namespace TeknikServis.Web.UI.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
            RegisterBundle();
        }

        private void RegisterBundle()
        {
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}