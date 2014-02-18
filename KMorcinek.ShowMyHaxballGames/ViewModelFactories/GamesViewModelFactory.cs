using System;
using System.Collections.Generic;
using System.Linq;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.ViewModels;
using KMorcinek.ShowMyHaxballGames.Extensions;

namespace KMorcinek.ShowMyHaxballGames.ViewModelFactories
{
    public class GamesViewModelFactory
    {
        public GamesViewModel Create(int leagueId, string name)
        {
            var involvedInGames = new List<Game>();

            var db = DbRepository.GetDb();
            var league = db.UseOnceTo().Query<League>().Where(t => t.LeagueNumer == leagueId).SingleOrDefault();

            foreach (var game in league.Games)
            {
                if (game.HomePlayer.Contains(name, StringComparison.CurrentCultureIgnoreCase) || game.AwayPlayer.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    involvedInGames.Add(game);
                }
            }

            var gamesViewModel = new GamesViewModel
            {
                LeagueId = leagueId,
                Name = name,
                Games = involvedInGames.OrderBy(p => p.Result),
            };

            return gamesViewModel;
        }
    }
}