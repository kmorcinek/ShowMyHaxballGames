using System;
using System.IO;
using KMorcinek.ShowMyHaxballGames.Models;
using Newtonsoft.Json;
using System.Linq;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeaguesProvider
    {
        public Event[] Get()
        {
            var db = DbRepository.GetDb();

            Event[] events = db.UseOnceTo().Query<Event>().ToArray();
            return events;
        }
    }
}