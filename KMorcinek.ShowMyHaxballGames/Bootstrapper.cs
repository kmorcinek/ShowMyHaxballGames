using System;
using System.Web;
using System.Web.Caching;
using KMorcinek.ShowMyHaxballGames.Business;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;

namespace KMorcinek.ShowMyHaxballGames
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private static CacheItemRemovedCallback _onCacheRemove;
        
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            AddTask("DoStuff", 1);
        }

        private void AddTask(string name, int seconds)
        {
            _onCacheRemove = CacheItemRemoved;
            HttpRuntime.Cache.Insert(name, seconds, null,
                DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                CacheItemPriority.NotRemovable, _onCacheRemove);
        }

        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            new LeaguesScheduler().Run();

            AddTask(k, 50);
        }
    }
}