using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Business;
using NUnit.Framework;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    [TestFixture]
    public class LeagueParsingTests
    {
        private readonly LeagueParser _leagueParser = new LeagueParser();

        [Test]
        public void GetNamesTest()
        {
            const string standingsNode = @"
                <div id=""standings"">
                    <div id=""standings-header"">
                        <div class=""standings-aa"">#</div>
                        <div class=""standings-team"">Team</div>
                        <div class=""standings-matches"">Matches</div>
                        <div class=""standings-wins"">Wins</div>
                        <div class=""standings-draws"">Draws</div>
                        <div class=""standings-losses"">Losses</div>
                        <div class=""standings-gf"">GF</div>
                        <div class=""standings-ga"">GA</div>
                        <div class=""standings-diff"">Diff</div>
                        <div class=""standings-points"">Points</div>
                    </div>
                                        <div class=""standings-row"">
                        <div class=""standings-aa"">1</div>
                        <div class=""standings-team"">Anders</div>
                        <div class=""standings-matches"">5</div>
                        <div class=""standings-wins"">2</div>
                        <div class=""standings-draws"">3</div>
                        <div class=""standings-losses"">0</div>
                        <div class=""standings-gf"">6</div>
                        <div class=""standings-ga"">3</div>
                        <div class=""standings-diff"">3</div>
                        <div class=""standings-points"">9</div>
                    </div>
                                        <div class=""standings-row"">
                        <div class=""standings-aa"">2</div>
                        <div class=""standings-team"">TomekM</div>
                        <div class=""standings-matches"">6</div>
                        <div class=""standings-wins"">2</div>
                        <div class=""standings-draws"">2</div>
                        <div class=""standings-losses"">2</div>
                        <div class=""standings-gf"">5</div>
                        <div class=""standings-ga"">5</div>
                        <div class=""standings-diff"">0</div>
                        <div class=""standings-points"">8</div>
                    </div>

                    <div style=""clear:both"">&nbsp;</div>
                </div>
            ";

            var doc = HtmlParsingHelper.GetHtmlDocument(standingsNode);

            HtmlNode parent = doc.DocumentNode.SelectSingleNode("//div[@id='standings']");

            var league = _leagueParser.ParseLeague(parent);

            Assert.AreEqual("Anders", league.Players[0]);
            Assert.AreEqual("TomekM", league.Players[1]);
        }
    }
}