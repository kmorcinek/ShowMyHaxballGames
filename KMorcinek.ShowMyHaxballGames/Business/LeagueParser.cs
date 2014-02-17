using System.Collections.Generic;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.ViewModels;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueParser
    {
        public LeagueViewModel ParseLeague(HtmlNode leagueNode)
        {
            var htmlNodeCollection = leagueNode.SelectNodes("div[@class='standings-row']");
            var playerNames = new List<string>();

            foreach (var node in htmlNodeCollection)
            {
                var playerName = node.SelectSingleNode("div[@class='standings-team']").InnerText;
                playerNames.Add(playerName);
            }

            return new LeagueViewModel
            {
                Players = playerNames
            };
        } 
    }
}