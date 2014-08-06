﻿using System;
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
            var league = db.UseOnceTo().GetByQuery<Event>(t => t.HaxballLeagueId == leagueId);

            foreach (var game in league.HaxballLeague.Games)
            {
                if (game.HomePlayer.Contains(name, StringComparison.CurrentCultureIgnoreCase) || game.AwayPlayer.Contains(name, StringComparison.CurrentCultureIgnoreCase))
                {
                    involvedInGames.Add(game);
                }
            }

            var notPlayed = involvedInGames.Where(g => g.Result == Constants.NotPlayed);
            var played = involvedInGames
                .Where(g => g.Result != Constants.NotPlayed)
                .OrderByDescending(g => g.PlayedDate);

            var gamesViewModel = new GamesViewModel
            {
                HaxballLeagueId = leagueId,
                LeagueTitle = league.Title,
                Name = name,
                Games = notPlayed.Concat(played)
            };

            return gamesViewModel;
        }
    }
}