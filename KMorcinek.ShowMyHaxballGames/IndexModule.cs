using KMorcinek.ShowMyHaxballGames.ViewModelFactories;

namespace KMorcinek.ShowMyHaxballGames
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => @"This website make it easier to find the people we haven't played so far (or check all results by a player). 
                                <br />
                                Try 'yourLeagueId/yourName' i.ee '/121729/Marian' or click <a href='/121729/Marian'>/121729/Marian</a>
                                <br />
                                Loading takes time (I am not caching, to serve always up to date results).";

            Get["/{leagueId}"] = parameters =>
            {
                var leagueId = int.Parse(parameters.leagueId.Value);
                return View["Index", leagueId];
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