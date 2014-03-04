using System.Collections.Generic;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Factories;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeaguesScheduler
    {
        private readonly GameParser _gameParser = new GameParser();
        private readonly LeagueGamesUpdater _leagueGamesUpdater = new LeagueGamesUpdater(new RealTimeProvider(), new ProgressFactory());

        public void Run()
        {
            var leaguesProvider = new LeaguesProvider();
            foreach (var league in leaguesProvider.Get())
            {
                RunLeague(league.LeagueId, league.SeasonNumber);
            }
        }

        private void RunLeague(int leagueId, int seasonNumber)
        {
            HtmlDocument document = new HtmlWeb().Load("http://www.haxball.gr/league/view/" + leagueId);

            var gamesNodes = document.DocumentNode.SelectNodes("//div[@id='fixtures']//div[@class='fixture-row']");

            var newGames = new List<Game>();

            foreach (var gameNode in gamesNodes)
            {
                var game = _gameParser.Parse(gameNode);
                if (FakePlayersHelper.IsNotFake(game.HomePlayer) && FakePlayersHelper.IsNotFake(game.AwayPlayer))
                    newGames.Add(game);
            }

            var leagueParser = new LeagueParser();

            var playersNode = document.DocumentNode.SelectSingleNode("//div[@id='standings']");
            var players = leagueParser.GetPlayers(playersNode);
            var title = LeagueTitleParser.GetLeagueTitle(document);
            var winner = leagueParser.GetWinner(document.DocumentNode);

            _leagueGamesUpdater.UpdateLeague(leagueId, title, newGames, players, seasonNumber, winner);
        }         
    }
}