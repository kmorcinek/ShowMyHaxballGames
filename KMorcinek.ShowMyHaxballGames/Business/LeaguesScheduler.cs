using System;
using System.Collections.Generic;
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
        private readonly LeagueGamesUpdater _leagueGamesUpdater = new LeagueGamesUpdater(new RealTimeProvider(), new ProgressFactory(), new GamesUpdater(new RealTimeProvider()));
        private static readonly ILog logger = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public void Run()
        {
            var db = DbRepository.GetDb();
            Event[] events = db.UseOnceTo().Query<Event>().ToArray();

            foreach (var eventEntry in events)
            {
                if (false == eventEntry.IsFromHaxball)
                    continue;

                RunLeague(eventEntry);
            }
        }

        private void RunLeague(Event eventEntry)
        {
            try
            {
                if (IsLeagueFinished(eventEntry.HaxballLeague))
                {
                    return;
                }

                logger.DebugFormat("Started parsing/updating leagueNumber: {0}", eventEntry.HaxballLeagueId);

                var url = string.Format("http://www.haxball.gr/league/view/{0}?g={1}",
                    eventEntry.HaxballLeagueId,
                    Guid.NewGuid().ToString("N"));

                HtmlDocument document =
                    new HtmlWeb().Load(url);

                var gamesNodes = document.DocumentNode.SelectNodes("//div[@id='fixtures']//div[@class='fixture-row']");

                if (gamesNodes == null)
                {
                    logger.DebugFormat(
                        "Probably league was removed from Haxball before it was finished. Id: {0}",
                        eventEntry.HaxballLeagueId);
                    return;
                }

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
                             ?? eventEntry.HardcodedWinner;

                if (eventEntry.HaxballLeague == null)
                {
                    eventEntry.HaxballLeague = _leagueGamesUpdater.GetNewLeague(eventEntry.HaxballLeagueId, title,
                        newGames,
                        players, eventEntry.SeasonNumber, winner);
                }
                else
                {
                    eventEntry.HaxballLeague = _leagueGamesUpdater.UpdateLeague(eventEntry.HaxballLeague,
                        eventEntry.HaxballLeagueId, title,
                        newGames, players, eventEntry.SeasonNumber, winner);
                }

                var db = DbRepository.GetDb();
                db.UseOnceTo().Update(eventEntry);
            }
            catch (System.Exception ex)
            {
                logger.Warn("leagueNumber: " + eventEntry.HaxballLeagueId, ex);
            }
            finally
            {
                logger.DebugFormat("Finished parsing/updating leagueNumber: {0}", eventEntry.HaxballLeagueId);
            }
        }

        private bool IsLeagueFinished(League haxballLeague)
        {
            return haxballLeague != null && haxballLeague.Progress.Played >= haxballLeague.Progress.Total;
        }
    }
}