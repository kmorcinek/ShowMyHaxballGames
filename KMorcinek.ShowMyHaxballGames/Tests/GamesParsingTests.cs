using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Business;
using NUnit.Framework;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    [TestFixture]
    public class GamesParsingTests
    {
        private readonly GameParser _gameParser = new GameParser();

        [Test]
        public void NotPlayedGameTest()
        {
            const string nodeHtml = @"
                        <div class=""fixture-row"">
                            <div class=""fixture-replay"">
                                &nbsp;                            </div>
                            <div class=""fixture-home"">Sylwek</div>
                            <div class=""fixture-score"">
                                 -                                 
                            </div>
                            <div class=""fixture-away"">Filip</div>

                            <div class=""fixture-set-replay"">                                
                                 &nbsp;
                            </div>
                            
                            <div class=""fixture-set-score"">
                                &nbsp;
                            </div>
                            
                        </div>";

            var doc = HtmlParsingHelper.GetHtmlDocument(nodeHtml);

            HtmlNode gameParent = doc.DocumentNode.SelectSingleNode("//div[@class='fixture-row']");

            var game = _gameParser.Parse(gameParent);

            Assert.AreEqual("Sylwek", game.HomePlayer);
            Assert.AreEqual("Filip", game.AwayPlayer);
        }

        [Test]
        public void AlreadyPlayedHomeWonGameTest()
        {
            const string nodeHtml = @"<div class=""fixture-row"">
                            <div class=""fixture-replay"">
                                &nbsp;                            </div>
                            <div class=""fixture-home-win"">Marian</div>
                            <div class=""fixture-score"">
                                7 - 0                                
                            </div>
                            <div class=""fixture-away"">Derk</div>

                            <div class=""fixture-set-replay"">                                
                                 &nbsp;
                            </div>
                            
                            <div class=""fixture-set-score"">
                                &nbsp;
                            </div>

                            
                            
                        </div>";

            var doc = HtmlParsingHelper.GetHtmlDocument(nodeHtml);

            HtmlNode gameParent = doc.DocumentNode.SelectSingleNode("//div[@class='fixture-row']");

            var game = _gameParser.Parse(gameParent);

            Assert.AreEqual("Marian", game.HomePlayer);
            Assert.AreEqual("Derk", game.AwayPlayer);
        }

        [Test]
        public void AlreadyPlayedAwayWonGameTest()
        {
            const string nodeHtml = @"<div class=""fixture-row"">
                            <div class=""fixture-replay"">
                                &nbsp;                            </div>
                            <div class=""fixture-home"">Filip</div>
                            <div class=""fixture-score"">
                                0 - 4                                
                            </div>
                            <div class=""fixture-away-win"">Marian</div>

                            <div class=""fixture-set-replay"">                                
                                 &nbsp;
                            </div>
                            
                            <div class=""fixture-set-score"">
                                &nbsp;
                            </div>

                            
                            
                        </div>";

            var doc = HtmlParsingHelper.GetHtmlDocument(nodeHtml);

            HtmlNode gameParent = doc.DocumentNode.SelectSingleNode("//div[@class='fixture-row']");

            var game = _gameParser.Parse(gameParent);

            Assert.AreEqual("Filip", game.HomePlayer);
            Assert.AreEqual("Marian", game.AwayPlayer);
        }

        [Test]
        public void NotPlayedGameResultTest()
        {
            const string nodeHtml = @"<div class=""fixture-row"">
                            <div class=""fixture-replay"">
                                &nbsp;                            </div>
                            <div class=""fixture-home"">Sylwek</div>
                            <div class=""fixture-score"">
                                 -                                 
                            </div>
                            <div class=""fixture-away"">Filip</div>

                            <div class=""fixture-set-replay"">                                
                                 &nbsp;
                            </div>
                            
                            <div class=""fixture-set-score"">
                                &nbsp;
                            </div>
                            
                        </div>";

            var doc = HtmlParsingHelper.GetHtmlDocument(nodeHtml);

            HtmlNode gameParent = doc.DocumentNode.SelectSingleNode("//div[@class='fixture-row']");

            var game = _gameParser.Parse(gameParent);

            Assert.AreEqual("-", game.Result);
        }

        [Test]
        public void AlreadyPlayedGameResultTest()
        {
            const string nodeHtml = @"<div class=""fixture-row"">
                            <div class=""fixture-replay"">
                                &nbsp;                            </div>
                            <div class=""fixture-home"">Filip</div>
                            <div class=""fixture-score"">
                                0 - 4                                
                            </div>
                            <div class=""fixture-away-win"">Marian</div>

                            <div class=""fixture-set-replay"">                                
                                 &nbsp;
                            </div>
                            
                            <div class=""fixture-set-score"">
                                &nbsp;
                            </div>

                        </div>";

            var doc = HtmlParsingHelper.GetHtmlDocument(nodeHtml);

            HtmlNode gameParent = doc.DocumentNode.SelectSingleNode("//div[@class='fixture-row']");

            var game = _gameParser.Parse(gameParent);

            Assert.AreEqual("0 - 4", game.Result);
        }
    }
}