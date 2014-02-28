using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;
using System.Linq;

namespace KMorcinek.ShowMyHaxballGames.Factories
{
    public class ProgressFactory
    {
        public virtual Progress Create(League league)
        {
            var progress = new Progress
            {
                Played = league.Games.Count(g => g.Result != Constants.NotPlayed),
                Total = TotalGamesCalculator.Calculate(league.Players.Count(FakePlayersHelper.IsNotFake))
            };

            return progress;
        } 
    }
}