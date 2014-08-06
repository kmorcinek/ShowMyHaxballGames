using System;
using System.Collections.Generic;
using FluentAssertions;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;
using Moq;
using Xunit;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    public class GamesUpdaterTests
    {
        private readonly DateTime _earlierDate = new DateTime(2013, 1, 1);
        private readonly DateTime _currentDate = new DateTime(2014, 5, 5);
        private readonly GamesUpdater _gamesScheduler;

        public GamesUpdaterTests()
        {
            var timeProvider = new Mock<ITimeProvider>();
            timeProvider.Setup(p => p.GetCurrentTime())
                .Returns(_currentDate);

            _gamesScheduler = new GamesUpdater(timeProvider.Object);
        }

        [Fact]
        public void NotPlayedGameIsPlayedAndGetCurrentDate()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed, PlayedDate = null };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "2-2" }
                );

            _gamesScheduler.UpdateGames(oldGames, newGames);

            Assert.Equal(_currentDate, oldGame.PlayedDate);
        }

        [Fact]
        public void NotPlayedGameIsStillNotPlayedAndDateDoesNotChange()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed, PlayedDate = null };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed }
                );

            _gamesScheduler.UpdateGames(oldGames, newGames);

            Assert.Equal(null, oldGame.PlayedDate);
        }

        [Fact]
        public void AlreadyPlayedGameDoesNotUpdateItsDate()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1", PlayedDate = _earlierDate };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1" }
                );

            _gamesScheduler.UpdateGames(oldGames, newGames);

            Assert.Equal(_earlierDate, oldGame.PlayedDate);
        }

        [Fact]
        public void Game_removed_from_haxball_is_removed_from_league()
        {
            var oldGame = new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = "1-1", PlayedDate = _earlierDate };
            var oldGames = new List<Game>();
            oldGames.Add(oldGame);

            var newGames = new List<Game>();
            newGames.Add(
                new Game { HomePlayer = "Sylwek", AwayPlayer = "Filip", Result = Constants.NotPlayed }
                );

            _gamesScheduler.UpdateGames(oldGames, newGames);

            oldGame.PlayedDate.Should().Be(null);
            oldGame.Result.Should().Be(Constants.NotPlayed);
        }
    }
}