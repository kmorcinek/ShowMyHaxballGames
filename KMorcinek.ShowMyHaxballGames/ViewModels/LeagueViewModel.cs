using System.Collections.Generic;

namespace KMorcinek.ShowMyHaxballGames.ViewModels
{
    public class LeagueViewModel
    {
        public int LeagueId { get; set; }
        public string Title { get; set; }
        public List<string> Players { get; set; } 
    }
}