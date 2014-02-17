using System;
using HtmlAgilityPack;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeagueTitleParser
    {
        public static string GetLeagueTitle(HtmlDocument document)
        {
            var title = document.DocumentNode.SelectSingleNode("//title").InnerText;
            var shorterTitle = title.Replace("League & Cup Generator for Football Game HaxBall - ", "");

            var s = String.IsNullOrWhiteSpace(shorterTitle)
                ? title
                : shorterTitle;
            return s;
        }
    }
}