using System.Collections.Generic;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Models;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueParser
    {
        public League ParseLeague(HtmlNode gameParent)
        {
            var htmlNodeCollection = gameParent.SelectNodes("div[@class='standings-row']");
            var playerNames = new List<string>();

            foreach (var node in htmlNodeCollection)
            {
                var playerName = node.SelectSingleNode("div[@class='standings-team']").InnerText;
                playerNames.Add(playerName);
            }

            return new League()
            {
                Players = playerNames
            };
        } 
    }
}