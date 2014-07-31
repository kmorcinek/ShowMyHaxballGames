using Nancy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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
            Get["/admin"] = _ => { return AdminPage(); };
            Get["/admin/events"] = _ => { return AdminPage(); };
            Get["/admin/events/new"] = _ => { return AdminPage(); };
            Get["/admin/events/{id:int}"] = _ => { return AdminPage(); };
        }
    }
}