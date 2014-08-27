using KMorcinek.ShowMyHaxballGames.Models;
using Newtonsoft.Json;
using SisoDb;
using SisoDb.SqlCe4;
using System;
using System.IO;
using System.Linq;

namespace KMorcinek.ShowMyHaxballGames
{
    public class DbRepository
    {
        public static ISisoDatabase GetDb()
        {
            return "Data source=|DataDirectory|Haxball.sdf;".CreateSqlCe4Db();
        }

        public static void Initialize()
        {
            var db = DbRepository.GetDb();

            if (db.Exists() == false)
            {
                db.EnsureNewDatabase();
            }

            if (db.UseOnceTo().Query<Event>().Count() == 0)
            {
                string path = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\" + "leagues.json";

                if (File.Exists(path) == false)
                    return;

                var jsonLeagues = File.ReadAllText(path);

                var deserializedEvents = JsonConvert.DeserializeObject<Event[]>(jsonLeagues);

                foreach (var eventEntry in deserializedEvents.Reverse())
                {
                    if (string.IsNullOrEmpty(eventEntry.Url))
                    {
                        eventEntry.IsFromHaxball = true;
                    }

                    db.UseOnceTo().Insert(eventEntry);
                }
            }

            if (db.UseOnceTo().Query<Configuration>().Count() == 0)
            {
                Configuration configuration = new Configuration
                {
                    ShowLastGamesCount = 12,
                };

                db.UseOnceTo().Insert(configuration);
            }
        }
    }
}