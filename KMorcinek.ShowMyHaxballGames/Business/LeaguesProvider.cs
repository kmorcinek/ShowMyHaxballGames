using System;
using System.IO;
using KMorcinek.ShowMyHaxballGames.Models;
using Newtonsoft.Json;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeaguesProvider
    {
        public League[] Get()
        {
            string path = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\" + "leagues.json";
            var jsonLeagues = File.ReadAllText(path);

            var deserializedLeagues = JsonConvert.DeserializeObject<League[]>(jsonLeagues);
            return deserializedLeagues;
        }
    }
}