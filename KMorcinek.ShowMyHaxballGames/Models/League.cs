using System.Collections.Generic;

namespace KMorcinek.ShowMyHaxballGames.Models
{
    public class League
    {
        public string Title { get; set; }
        public List<string> Players { get; set; }
        public List<Game> Games { get; set; }
        public Progress Progress { get; set; }
        public string Winner { get; set; }
    }
}