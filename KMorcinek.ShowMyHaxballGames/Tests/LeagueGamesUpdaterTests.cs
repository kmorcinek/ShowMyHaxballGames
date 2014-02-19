using System;
using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;
using Moq;
using NUnit.Framework;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    [TestFixture]
    public class LeagueGamesUpdaterTests
    {
        private readonly DateTime _earlierDate = new DateTime(2013, 1, 1);
        private readonly DateTime _currentDate = new DateTime(2014, 5, 5);
        private readonly LeagueGamesUpdater _leagueGamesScheduler;

        public LeagueGamesUpdaterTests()
        {
            var timeProvider = new Mock<ITimeProvider>();
            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_currentDate);

            _leagueGamesScheduler = new LeagueGamesUpdater(timeProvider.Object);
        }

        [Test]
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

            Assert.AreEqual(_currentDate, oldGame.PlayedDate);
        }

        [Test]
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

            Assert.AreEqual(null, oldGame.PlayedDate);
        }

        [Test]
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

            Assert.AreEqual(_earlierDate, oldGame.PlayedDate);
        }

        [Test]
        public void WhenLeagueDoesNotExistsNewPlayedGamesAreSetToCurrentDate()
        {
            var db = DbRepository.GetDb();
            db.EnsureNewDatabase();
            
            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            var leagueId = 0;
            _leagueGamesScheduler.UpdateLeague(leagueId, "", newGames);

            var league = db.UseOnceTo().Query<League>().Where(t => t.LeagueNumer == leagueId).SingleOrDefault();

            Assert.IsNotNull(league);
            Assert.AreEqual(_currentDate, league.Games[0].PlayedDate);
            db.EnsureNewDatabase();
        }

        [Test]
        public void WhenLeagueDoesNotExistsNotPlayedGamesAreNotSet()
        {
            var db = DbRepository.GetDb();
            db.EnsureNewDatabase();
            
            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed }
                );

            var leagueId = 0;
            _leagueGamesScheduler.UpdateLeague(leagueId, "", newGames);

            var league = db.UseOnceTo().Query<League>().Where(t => t.LeagueNumer == leagueId).SingleOrDefault();

            Assert.IsNotNull(league);
            Assert.AreEqual(null, league.Games[0].PlayedDate);
            db.DeleteIfExists();
            db.EnsureNewDatabase();
        }

        [Test]
        public void WhenLeagueExistsWithPlayedGamesDatesShouldNotBeUpdated()
        {
            var timeProvider = new Mock<ITimeProvider>();
            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_earlierDate);

            var leagueGamesScheduler = new LeagueGamesUpdater(timeProvider.Object);

            var db = DbRepository.GetDb();
            db.EnsureNewDatabase();
            
            var previousGames = new List<Game>();
            previousGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            var leagueId = 0;
            leagueGamesScheduler.UpdateLeague(leagueId, "", previousGames);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_currentDate);

            leagueGamesScheduler.UpdateLeague(leagueId, "", newGames);

            var league = db.UseOnceTo().Query<League>().Where(t => t.LeagueNumer == leagueId).SingleOrDefault();

            Assert.IsNotNull(league);
            Assert.AreEqual(_earlierDate, league.Games[0].PlayedDate);
            db.DeleteIfExists();
            db.EnsureNewDatabase();
        }
    }
}