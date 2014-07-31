﻿using System.Collections.Generic;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Factories;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.Utils;
using log4net;
using System.Reflection;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeaguesScheduler
    {
        private readonly GameParser _gameParser = new GameParser();
        private readonly LeagueGamesUpdater _leagueGamesUpdater = new LeagueGamesUpdater(new RealTimeProvider(), new ProgressFactory());
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Run()
        {
            var leaguesProvider = new LeaguesProvider();
            foreach (var league in leaguesProvider.Get())
            {
                RunLeague(league);
            }
        }

        private void RunLeague(Event league)
        {
            try
            {
                if (IsLeagueFinished(league.Id))
                {
                    return;
                }

                logger.DebugFormat("Started parsing/updating leagueNumber: {0}", league.HaxballLeagueId);

                HtmlDocument document = new HtmlWeb().Load("http://www.haxball.gr/league/view/" + league.HaxballLeagueId);

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
                var winner = leagueParser.GetWinner(document.DocumentNode)
                    ?? league.HardcodedWinner;

                _leagueGamesUpdater.UpdateLeague(league.HaxballLeagueId, title, newGames, players, league.SeasonNumber, winner);

                logger.DebugFormat("Finished parsing/updating leagueNumber: {0}", league.HaxballLeagueId);
            }
            catch (System.Exception ex)
            {
                logger.Warn("leagueNumber: " + league.HaxballLeagueId, ex);
            }
        }

        private bool IsLeagueFinished(int eventId)
        {
            var db = DbRepository.GetDb();
            var leagueFromDB = db.UseOnceTo().GetById<Event>(eventId).HaxballLeague;

            return leagueFromDB != null && leagueFromDB.Progress.Played >= leagueFromDB.Progress.Total;
        }         
    }
}