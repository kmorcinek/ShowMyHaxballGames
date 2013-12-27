using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.ViewModels;
using KMorcinek.ShowMyHaxballGames.Extensions;

namespace KMorcinek.ShowMyHaxballGames.ViewModelFactories
{
    public class GamesViewModelFactory
    {
        private readonly GameParser _gameParser = new GameParser();

        public GamesViewModel Create(string name)
        {
                        HtmlDocument document = new HtmlWeb().Load("http://www.haxball.gr/league/view/121729");
//            var document = new HtmlDocument();
//            document.Load("wholeHaxballPage.htm");

            var someNodes = document.DocumentNode.SelectNodes("//div[@id='fixtures']//div[@class='fixture-row']");

            var involvedInGames = new List<Game>();

            foreach (var gameNode in someNodes)
            {
                var game = _gameParser.Parse(gameNode);
                if (game.HomePlayer.Contains(name, StringComparison.CurrentCultureIgnoreCase) || game.AwayPlayer.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    involvedInGames.Add(game);
                }
            }

            var gamesViewModel = new GamesViewModel()
            {
                Name = name,
                Games = involvedInGames.OrderBy(p => p.Result),
            };

            return gamesViewModel;
        }
    }
}