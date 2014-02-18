using System.Collections.Generic;
using System.Linq;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueGamesUpdater
    {
        private readonly ITimeProvider _timeProvider;

        public LeagueGamesUpdater(ITimeProvider timeProvider)
        {
            _timeProvider = timeProvider;
        }

        public void UpdateLeague(int leagueId, string title, List<Game> newGames)
        {
            var db = DbRepository.GetDb();
            var league = db.UseOnceTo().Query<League>().Where(t => t.LeagueNumer == leagueId).SingleOrDefault();

            if (league == null)
            {
                league = new League
                {
                    LeagueNumer = leagueId,
                    Title = title,
                    Games = new List<Game>(),
                };

                foreach (var newGame in newGames)
                {
                    var gameCopy = newGame.GetDeepCopy();

                    if (newGame.Result != Constants.NotPlayed)
                        gameCopy.PlayedDate = _timeProvider.GetCurrentTime();

                    league.Games.Add(gameCopy);
                }

                db.UseOnceTo().Insert(league);
            }
            else
            {
                UpdateLeague(league, newGames);
                db.UseOnceTo().Update(league);
            }
        }

        public void UpdateLeague(League league, List<Game> newGames)
        {
            var notPlayedOldGames = league.Games.Where(g => g.Result == Constants.NotPlayed);

            foreach (var oldGame in notPlayedOldGames)
            {
                var matchingNewGame =
                    newGames.SingleOrDefault(
                        g => g.HomePlayer == oldGame.HomePlayer && g.AwayPlayer == oldGame.AwayPlayer);

                if (matchingNewGame != null && matchingNewGame.Result != Constants.NotPlayed)
                {
                    oldGame.Result = matchingNewGame.Result;
                    oldGame.PlayedDate = _timeProvider.GetCurrentTime();
                }
            }

            league.Games = newGames;
        }
    }
}