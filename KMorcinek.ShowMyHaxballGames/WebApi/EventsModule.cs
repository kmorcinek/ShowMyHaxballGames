using System.IO;
using System.Linq;
using KMorcinek.ShowMyHaxballGames.Extensions;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;
using Nancy;
using Nancy.ModelBinding;

namespace KMorcinek.ShowMyHaxballGames.WebApi
{
    public class EventsModule : NancyModule
    {
        public EventsModule(IRootPathProvider pathProvider)
            : base("api/events")
        {
            Get["/"] = _ =>
            {
                var db = DbRepository.GetDb();

                var events = db.UseOnceTo().Query<Event>().ToArray();

                var thinEvents = events.Select(p =>
                    new
                    {
                        p.Id,
                        p.Title
                    });

                return Response.AsJson(thinEvents);
            };

            Get["/{id:int}"] = _ =>
            {
                var id = (int)_.id.Value;

                var db = DbRepository.GetDb();
                var eventEntry = db.UseOnceTo().GetById<Event>(id);

                eventEntry.HaxballLeague = null;

                return Response.AsJson(eventEntry);
            };

            Post["/"] = _ =>
            {
                var eventEntry = this.Bind<Event>();
                var db = DbRepository.GetDb();

                db.UseOnceTo().Insert(eventEntry);

                return HttpStatusCode.Created;
            };

            Post["/{id:int}"] = _ =>
            {
                var id = (int)_.id.Value;

                var db = DbRepository.GetDb();
                var eventEntry = db.UseOnceTo().GetById<Event>(id);

                this.BindTo(eventEntry, ReflectionHelper.GetPropertyName<Event>(p => p.HaxballLeague));

                db.UseOnceTo().Update(eventEntry);

                return HttpStatusCode.OK;
            };

            Post["/imageupload/{Id:int}"] = _ =>
            {
                var id = (int)_.id.Value;
                var file = this.Request.Files.FirstOrDefault();

                if (file != null)
                {
                    var filename = Path.Combine(pathProvider.GetRootPath(), "HolidayImages", id + ".png");

                    DirectoryUtils.EnsureDirectoryExistst(filename);

                    using (var fileStream = new FileStream(filename, FileMode.Create))
                    {
                        file.Value.CopyTo(fileStream);
                    }

                    return Response.AsRedirect("/admin/events/");
                }

                return HttpStatusCode.NotFound;
            };

            Delete["/{id:int}"] = parameters =>
            {
                int eventId = (int)parameters.id.Value;

                var db = DbRepository.GetDb();
                db.UseOnceTo().DeleteById<Event>(eventId);

                return HttpStatusCode.NoContent;
            };
        }
    }
}