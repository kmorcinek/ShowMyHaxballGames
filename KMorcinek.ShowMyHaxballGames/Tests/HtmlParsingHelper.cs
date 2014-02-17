using System;
using HtmlAgilityPack;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    static internal class HtmlParsingHelper
    {
        public static HtmlDocument GetHtmlDocument(string nodeHtml)
        {
            string html = String.Format("<html><head></head><body>{0}</body></html>", nodeHtml);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc;
        }
    }
}