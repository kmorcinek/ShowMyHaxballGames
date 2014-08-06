using System.Collections.Generic;
using System.Linq;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class GamesUpdater
    {
        private readonly ITimeProvider _timeProvider;

        public GamesUpdater(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void UpdateGames(List<Game> oldGames, List<Game> newGames)
        {
            foreach (var oldGame in oldGames)
            {
                var matchingNewGame =
                    newGames.SingleOrDefault(
                        g => g.HomePlayer == oldGame.HomePlayer && g.AwayPlayer == oldGame.AwayPlayer);

                if (matchingNewGame != null)
                {
                    if (matchingNewGame.Result != Constants.NotPlayed)
                    {
                        if (oldGame.Result == Constants.NotPlayed)
                            oldGame.PlayedDate = _timeProvider.GetCurrentTime();

                        oldGame.Result = matchingNewGame.Result;
                    }
                    else
                    {
                        oldGame.Result = Constants.NotPlayed;
                        oldGame.PlayedDate = null;
                    }
                }
            }
        }
    }
}