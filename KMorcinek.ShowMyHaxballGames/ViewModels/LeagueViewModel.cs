using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Models;

namespace KMorcinek.ShowMyHaxballGames.ViewModels
{
    public class LeagueViewModel
    {
        public int LeagueId { get; set; }
        public string Title { get; set; }
        public int SeasonNumber { get; set; }
        public List<string> Players { get; set; }
        public IEnumerable<Game> NewestGames { get; set; }
        public Progress Progress { get; set; }

        public LeagueViewModel()
        {
        }

        public LeagueViewModel(League league)
        {
            LeagueId = league.LeagueNumer;
            Title = league.Title;
            SeasonNumber = 444;
            //            Players = league.Players;
            Progress = league.Progress;
        }
    }
}