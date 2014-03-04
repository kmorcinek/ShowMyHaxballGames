using System;
using System.IO;
using KMorcinek.ShowMyHaxballGames.ViewModels;
using Newtonsoft.Json;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeaguesProvider
    {
        public LeagueViewModel[] Get()
        {
            string path = AppDomain.CurrentDomain.GetData("DataDirectory") + "\\" + "leagues.json";
            var jsonLeagues = File.ReadAllText(path);

            var deserializedLeagues = JsonConvert.DeserializeObject<LeagueViewModel[]>(jsonLeagues);
            return deserializedLeagues;
        }
    }
}