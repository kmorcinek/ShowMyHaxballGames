using System;
using System.Web;
using System.Web.Caching;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace KMorcinek.ShowMyHaxballGames
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private static CacheItemRemovedCallback OnCacheRemove = null;
        
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            AddTask("DoStuff", 60);
            // your customization goes here
        }

        private void AddTask(string name, int seconds)
        {
            OnCacheRemove = new CacheItemRemovedCallback(CacheItemRemoved);
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, OnCacheRemove);
        }

        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {


            // do stuff here if it matches our taskname, like WebRequest
            // re-add our task so it recurs
            AddTask(k, Convert.ToInt32(v));
        }
    }
}