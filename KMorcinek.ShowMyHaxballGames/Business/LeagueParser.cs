using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueParser
    {
        public List<string> GetPlayers(HtmlNode leagueNode)
        {
            var htmlNodeCollection = leagueNode.SelectNodes("div[@class='standings-row']");

            return htmlNodeCollection.Select(node => node.SelectSingleNode("div[@class='standings-team']").InnerText).ToList();
        }
    }
}