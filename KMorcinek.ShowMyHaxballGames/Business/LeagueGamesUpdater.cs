using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Factories;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueGamesUpdater
    {
        private readonly ITimeProvider _timeProvider;
        private readonly ProgressFactory _progressFactory;
        private readonly GamesUpdater _gamesUpdater;

        public LeagueGamesUpdater(ITimeProvider timeProvider, ProgressFactory progressFactory, GamesUpdater gamesUpdater)
        {
            _timeProvider = timeProvider;
            _progressFactory = progressFactory;
            _gamesUpdater = gamesUpdater;
        }

        public League GetNewLeague(int leagueId, string title, List<Game> newGames, List<string> players, string seasonNumber, string winner)
        {
            League league = new League
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

            return league;
        }

        public League UpdateLeague(League league, int leagueId, string title, List<Game> newGames, List<string> players, string seasonNumber, string winner)
        {
            _gamesUpdater.UpdateGames(league.Games, newGames);

            league.Players = players;
            league.Progress = _progressFactory.Create(league);
            league.Winner = winner;

            return league;
        }
    }
}