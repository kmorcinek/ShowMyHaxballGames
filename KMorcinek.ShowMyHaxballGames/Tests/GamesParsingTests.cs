using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Business;
using Xunit;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    public class GamesParsingTests
    {
        private readonly GameParser _gameParser = new GameParser();

        [Fact]
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

            Assert.Equal("Sylwek", game.HomePlayer);
            Assert.Equal("Filip", game.AwayPlayer);
        }

        [Fact]
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

            Assert.Equal("Marian", game.HomePlayer);
            Assert.Equal("Derk", game.AwayPlayer);
        }

        [Fact]
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

            Assert.Equal("Filip", game.HomePlayer);
            Assert.Equal("Marian", game.AwayPlayer);
        }

        [Fact]
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

            Assert.Equal("-", game.Result);
        }

        [Fact]
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

            Assert.Equal("0 - 4", game.Result);
        }
    }
}