using Nancy;
using Nancy.ModelBinding;

namespace KMorcinek.ShowMyHaxballGames.WebApi
{
    public class ConfigurationModule : NancyModule
    {
        public const int HardcodedConfigurationId = 1;

        public ConfigurationModule()
            : base("api/configurations")
        {
            Post["/{id:int}"] = _ =>
            {
                var db = DbRepository.GetDb();
                int id = (int) _.id.Value;

                Configuration configuration = db.UseOnceTo().GetById<Configuration>(id);

                this.BindTo(configuration);

                db.UseOnceTo().Update(configuration);

                return HttpStatusCode.OK;
            };

            Get["/{id:int}"] = _ =>
            {
                var id = (int)_.id.Value;

                var db = DbRepository.GetDb();
                var eventEntry = db.UseOnceTo().GetById<Configuration>(id);

                return eventEntry;
            };
        }
    }
}