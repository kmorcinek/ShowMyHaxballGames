using System;
using System.Web;
using System.Web.Caching;
using KMorcinek.ShowMyHaxballGames.Business;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using log4net.Config;
using log4net;
using System.Reflection;

namespace KMorcinek.ShowMyHaxballGames
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        private static CacheItemRemovedCallback _onCacheRemove;
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        // The bootstrapper enables you to reconfigure the composition of the framework,
        // by overriding the various methods and properties.
        // For more information https://github.com/NancyFx/Nancy/wiki/Bootstrapper

        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            XmlConfigurator.Configure();

            logger.Info("Starting");

            DbRepository.Initialize();
            AddTask("DoStuff", 1);
        }

        private void AddTask(string name, int seconds)
        {
            try
            {
                _onCacheRemove = CacheItemRemoved;
                HttpRuntime.Cache.Insert(name, seconds, null,
                    DateTime.Now.AddSeconds(seconds), Cache.NoSlidingExpiration,
                    CacheItemPriority.NotRemovable, _onCacheRemove);
            }
            catch (Exception ex)
            {
                logger.Warn("AddTask", ex);
            }
        }

        public void CacheItemRemoved(string k, object v, CacheItemRemovedReason r)
        {
            new LeaguesScheduler().Run();

            AddTask(k, 50);
        }
    }
}