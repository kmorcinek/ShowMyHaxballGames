using Nancy;
using System.Web.Optimization;

namespace KMorcinek.ShowMyHaxballGames
{
    public class AdminModule : NancyModule
    {
        dynamic AdminPage()
        {
            return View["Admin", new
            {
                BundledVendorScripts = Scripts.Render("~/Scripts/bundleVendorScripts.js").ToString(),
                BundledScripts = Scripts.Render("~/Scripts/bundledJs.js").ToString(),
            }];
        }

        public AdminModule()
        {
            Get["/admin"] = _ => AdminPage();
            Get["/admin/events"] = _ => AdminPage();
            Get["/admin/events/new"] = _ => AdminPage();
            Get["/admin/events/{id:int}"] = _ => AdminPage();

            Get["/admin/configuration/"] = _ => AdminPage();
        }
    }
}