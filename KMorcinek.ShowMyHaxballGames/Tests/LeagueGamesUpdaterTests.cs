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

            _leagueGamesScheduler = new LeagueGamesUpdater(timeProvider.Object, _progressFactoryMock.Object);
        }

        [Fact]
        public void NotPlayedGameIsPlayedAndGetCurrentDate()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed, PlayedDate = null };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var league = new League
            {
                Games = oldGames
            };

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "2-2" }
                );

            _leagueGamesScheduler.UpdateLeague(league, newGames);

            Assert.Equal(_currentDate, oldGame.PlayedDate);
        }

        [Fact]
        public void NotPlayedGameIsStillNotPlayedAndDateDoesNotChange()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed, PlayedDate = null };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var league = new League
            {
                Games = oldGames
            };

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed }
                );

            _leagueGamesScheduler.UpdateLeague(league, newGames);

            Assert.Equal(null, oldGame.PlayedDate);
        }

        [Fact]
        public void AlreadyPlayedGameDoesNotUpdateItsDate()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1", PlayedDate = _earlierDate };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var league = new League
            {
                Games = oldGames
            };

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            _leagueGamesScheduler.UpdateLeague(league, newGames);

            Assert.Equal(_earlierDate, oldGame.PlayedDate);
        }

        [Fact]
        public void WhenLeagueDoesNotExistsNewPlayedGamesAreSetToCurrentDate()
        {
            var db = DbRepository.GetDb();
            db.EnsureNewDatabase();

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            var leagueId = 0;
            _leagueGamesScheduler.UpdateLeague(leagueId, "", newGames, null, 0, null);

            var league = db.UseOnceTo().GetByQuery<League>(t => t.LeagueNumer == leagueId);

            Assert.NotNull(league);
            Assert.Equal(_currentDate, league.Games[0].PlayedDate);
            db.EnsureNewDatabase();
        }

        [Fact]
        public void WhenLeagueDoesNotExistsNotPlayedGamesAreNotSet()
        {
            var db = DbRepository.GetDb();
            db.EnsureNewDatabase();

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed }
                );

            var leagueId = 0;
            _leagueGamesScheduler.UpdateLeague(leagueId, "", newGames, null, 0, null);

            var league = db.UseOnceTo().GetByQuery<League>(t => t.LeagueNumer == leagueId);

            Assert.NotNull(league);
            Assert.Equal(null, league.Games[0].PlayedDate);
            db.DeleteIfExists();
            db.EnsureNewDatabase();
        }

        [Fact]
        public void WhenLeagueExistsWithPlayedGamesDatesShouldNotBeUpdated()
        {
            var timeProvider = new Mock<ITimeProvider>();
            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_earlierDate);

            var leagueGamesScheduler = new LeagueGamesUpdater(timeProvider.Object, _progressFactoryMock.Object);

            var db = DbRepository.GetDb();
            db.EnsureNewDatabase();

            var previousGames = new List<Game>();
            previousGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            var leagueId = 0;
            leagueGamesScheduler.UpdateLeague(leagueId, "", previousGames, null, 0, null);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_currentDate);

            leagueGamesScheduler.UpdateLeague(leagueId, "", newGames, null, 0, null);

            var league = db.UseOnceTo().GetByQuery<League>(t => t.LeagueNumer == leagueId);
            
            Assert.NotNull(league);
            Assert.Equal(_earlierDate, league.Games[0].PlayedDate);
            db.DeleteIfExists();
            db.EnsureNewDatabase();
        }
    }
}