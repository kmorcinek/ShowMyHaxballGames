using System;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Models;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class GameParser
    {
        public Game Parse(HtmlNode gameParent)
        {
            var homePlayerNode = gameParent.SelectSingleNode("div[@class='fixture-home']")
                                 ?? gameParent.SelectSingleNode("div[@class='fixture-home-win']");

            var awayPlayerNode = gameParent.SelectSingleNode("div[@class='fixture-away']")
                                 ?? gameParent.SelectSingleNode("div[@class='fixture-away-win']");

            var resultNodeText = gameParent.SelectSingleNode("div[@class='fixture-score']").InnerText;
            resultNodeText = resultNodeText.Replace(Environment.NewLine, "");
            resultNodeText = resultNodeText.Trim();

            var game = new Game
            {
                HomePlayer = homePlayerNode.InnerText,
                AwayPlayer = awayPlayerNode.InnerText,
                Result = resultNodeText,
            };

            return game;
        }
    }
}