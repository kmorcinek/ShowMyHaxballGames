using System;
using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.Factories;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;
using Moq;
using Xunit;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    public class LeagueGamesUpdaterTests
    {
        private readonly DateTime _earlierDate = new DateTime(2013, 1, 1);
        private readonly DateTime _currentDate = new DateTime(2014, 5, 5);
        private readonly LeagueGamesUpdater _leagueGamesScheduler;
        private readonly Mock<ProgressFactory> _progressFactoryMock;

        public LeagueGamesUpdaterTests()
        {
            var timeProvider = new Mock<ITimeProvider>();
            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_currentDate);

            _progressFactoryMock = new Mock<ProgressFactory>();
            _progressFactoryMock.Setup(p => p.Create(It.IsAny<League>()))
                .Returns(new Progress());

            _leagueGamesScheduler = new LeagueGamesUpdater(timeProvider.Object, _progressFactoryMock.Object, new GamesUpdater(timeProvider.Object));
        }

        [Fact]
        public void WhenLeagueDoesNotExistsNewPlayedGamesAreSetToCurrentDate()
        {
            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            var league = _leagueGamesScheduler.GetNewLeague(0, "", newGames, null, "0", null);

            Assert.NotNull(league);
            Assert.Equal(_currentDate, league.Games[0].PlayedDate);
        }

        [Fact]
        public void WhenLeagueDoesNotExistsNotPlayedGamesAreNotSet()
        {
            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed }
                );

            var league = _leagueGamesScheduler.GetNewLeague(0, "", newGames, null, "0", null);

            Assert.NotNull(league);
            Assert.Equal(null, league.Games[0].PlayedDate);
        }

        [Fact]
        public void WhenLeagueExistsWithPlayedGamesDatesShouldNotBeUpdated()
        {
            var timeProvider = new Mock<ITimeProvider>();
            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_earlierDate);

            var leagueGamesScheduler = new LeagueGamesUpdater(timeProvider.Object, _progressFactoryMock.Object, new GamesUpdater(timeProvider.Object));

            var previousGames = new List<Game>();
            previousGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            League league =  leagueGamesScheduler.GetNewLeague(0, "", previousGames, null, "0", null);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_currentDate);

            League updatedLeague = leagueGamesScheduler.UpdateLeague(league, 0, "", newGames, null, "0", null);

            Assert.NotNull(league);
            Assert.Equal(_earlierDate, updatedLeague.Games[0].PlayedDate);
        }
    }
}