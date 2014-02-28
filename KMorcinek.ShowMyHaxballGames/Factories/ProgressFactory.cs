using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;

namespace KMorcinek.ShowMyHaxballGames.Factories
{
    public class ProgressFactory
    {
        public Progress Create(League league)
        {
            var progress = new Progress
            {
                Played = league.Games.Count,
                Total = TotalGamesCalculator.Calculate(league.Players.Count)
            };

            return progress;
        } 
    }
}