using System.Collections.Generic;

namespace KMorcinek.ShowMyHaxballGames.Models
{
    public class League
    {
        public int Id { get; set; }
        public int LeagueNumer { get; set; }
        public string Title { get; set; }
        public List<Game> Games { get; set; } 
    }
}