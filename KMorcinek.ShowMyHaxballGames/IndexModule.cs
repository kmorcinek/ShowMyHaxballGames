using KMorcinek.ShowMyHaxballGames.ViewModelFactories;

namespace KMorcinek.ShowMyHaxballGames
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ =>
            {
                return View["Index"];
            };

            Get["/{name}"] = parameters =>
            {
                var leagueId = 126004;
                var name = parameters.name.Value as string;
                var gamesViewModelFactory = new GamesViewModelFactory();
                var gamesViewModel = gamesViewModelFactory.Create(leagueId, name);
                return View["Games", gamesViewModel];
            };
        }
    }
}