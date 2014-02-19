using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.ViewModelFactories;
using KMorcinek.ShowMyHaxballGames.ViewModels;
using Nancy.Responses;

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
                var leaguesProvider = new LeaguesProvider();
                leagues.AddRange(leaguesProvider.Get());

                return View["Index", leagues];
            };

            Get["/{leagueId}"] = _ =>
            {
                var unparsedLeagueId = _.leagueId.Value;

                int leagueId;
                if (int.TryParse(unparsedLeagueId, out leagueId) == false)
                {
                    // legacy for previous versions
                    return new RedirectResponse("/121729/" + unparsedLeagueId);
                }

                var leagueViewModelFactory = new LeagueViewModelFactory();
                var leagueViewModel = leagueViewModelFactory.Create(leagueId);
                return View["League", leagueViewModel];
            };

            Get["/{leagueId}/{name}"] = _ =>
            {
                var leagueId = int.Parse(_.leagueId.Value);
                var name = _.name.Value as string;
                var gamesViewModelFactory = new GamesViewModelFactory();
                var gamesViewModel = gamesViewModelFactory.Create(leagueId, name);
                return View["Games", gamesViewModel];
            };
        }
    }
}