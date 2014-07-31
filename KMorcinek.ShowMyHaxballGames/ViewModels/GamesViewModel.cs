using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Models;

namespace KMorcinek.ShowMyHaxballGames.ViewModels
{
    public class GamesViewModel
    {
        public int HaxballLeagueId { get; set; }
        public string LeagueTitle { get; set; }
        public string Name { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }
}