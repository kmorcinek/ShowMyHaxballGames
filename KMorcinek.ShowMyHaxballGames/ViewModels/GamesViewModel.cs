using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Models;

namespace KMorcinek.ShowMyHaxballGames.ViewModels
{
    public class GamesViewModel
    {
        public string Name { get; set; }
        public IEnumerable<Game> Games { get; set; }
    }
}