using KMorcinek.ShowMyHaxballGames.ViewModelFactories;

namespace KMorcinek.ShowMyHaxballGames
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ => @"This website make it easier to find the people we haven't played so far (or check all results for a player). 
                                <br />
                                Try 'yourLeagueId/yourName' i.ee '/121729/Marian' or click <a href='/121729/Marian'>/121729/Marian</a>
            ";

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