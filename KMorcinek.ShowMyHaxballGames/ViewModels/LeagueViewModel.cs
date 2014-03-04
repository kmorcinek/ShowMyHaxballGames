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
        public string Winner { get; set; }
        public string WrittenProgress { get; set; }

        public LeagueViewModel()
        {
        }

        public LeagueViewModel(League league)
        {
            LeagueId = league.LeagueNumer;
            Title = league.Title;
            SeasonNumber = league.SeasonNumber;
            Players = league.Players;
            Progress = league.Progress;
            Winner = league.Winner;
            WrittenProgress = league.Progress.Played >= league.Progress.Total 
                ? "Finished" 
                : string.Format("In progress ({0}/{1})", league.Progress.Played, league.Progress.Total );
        }
    }
}