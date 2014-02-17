using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.ViewModelFactories;
using KMorcinek.ShowMyHaxballGames.ViewModels;

namespace KMorcinek.ShowMyHaxballGames
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ =>
            {
                var leagues = new List<LeagueViewModel>();
                leagues.AddRange(
                    new[]
                    {
                        new LeagueViewModel { LeagueId = 126002, Title = "MW League S02 Div1" },
                        new LeagueViewModel { LeagueId = 126003, Title = "MW League S02 Div2" },
                        new LeagueViewModel { LeagueId = 126004, Title = "MW League S02 Div3A" },
                        new LeagueViewModel { LeagueId = 126005, Title = "MW League S02 Div3B" },
                        new LeagueViewModel { LeagueId = 121729, Title = "MW League Season 1" },
                    });

                return View["Index", leagues];
            };

            Get["/{leagueId}"] = parameters =>
            {
                var leagueId = int.Parse(parameters.leagueId.Value);

                var leagueViewModelFactory = new LeagueViewModelFactory();
                var leagueViewModel = leagueViewModelFactory.Create(leagueId);
                return View["League", leagueViewModel];
            };

            Get["/{leagueId}/{name}"] = parameters =>
            {
                var leagueId = int.Parse(parameters.leagueId.Value);
                var name = parameters.name.Value as string;
                var gamesViewModelFactory = new GamesViewModelFactory();
                var gamesViewModel = gamesViewModelFactory.Create(leagueId, name);
                return View["Games", gamesViewModel];
            };
        }
    }
}