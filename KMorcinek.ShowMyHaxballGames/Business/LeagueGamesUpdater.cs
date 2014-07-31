using System.Collections.Generic;
using System.Linq;
using KMorcinek.ShowMyHaxballGames.Factories;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueGamesUpdater
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ProgressFactory _progressFactory;

        public LeagueGamesUpdater(ITimeProvider timeProvider, ProgressFactory progressFactory)
        {
            _timeProvider = timeProvider;
            _progressFactory = progressFactory;
        }

        public void UpdateLeague(int leagueId, string title, List<Game> newGames, List<string> players, string seasonNumber, string winner)
        {
            var db = DbRepository.GetDb();
            Event eventEntry = db.UseOnceTo().GetByQuery<Event>(t => t.HaxballLeagueId == leagueId);
            League league = eventEntry.HaxballLeague;

            if (league == null)
            {
                league = new League
                {
                    Title = title,
                    Players = players,
                    Games = new List<Game>(),
                    Winner = winner,
                };

                foreach (var newGame in newGames)
                {
                    var gameCopy = newGame.CreateDeepCopy();

                    if (newGame.Result != Constants.NotPlayed)
                        gameCopy.PlayedDate = _timeProvider.GetCurrentTime();

                    league.Games.Add(gameCopy);
                }

                league.Progress = _progressFactory.Create(league);

                eventEntry.HaxballLeague = league;

                db.UseOnceTo().Update(eventEntry);
            }
            else
            {
                UpdateLeague(league, newGames);

                league.Players = players;
                league.Progress = _progressFactory.Create(league);
                league.Winner = winner;

                db.UseOnceTo().Update(league);
            }
        }

        public void UpdateLeague(League league, List<Game> newGames)
        {
            foreach (var oldGame in league.Games)
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