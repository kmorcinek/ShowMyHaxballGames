using KMorcinek.ShowMyHaxballGames.ViewModelFactories;

namespace KMorcinek.ShowMyHaxballGames
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = _ =>   @"This website make it easier to find the people we haven't played so far (or check all results by a player). 
                                <br />
                                Try '/yourName' i.ee '/Marian' or click <a href='/Marian'>/Marian</a>
                                <br />
                                Loading takes time (I am not caching, to serve always up to date results).";

            Get["/{name}"] = parameters =>
            {
                var name = parameters.name.Value as string;
                var gamesViewModelFactory = new GamesViewModelFactory();
                var gamesViewModel = gamesViewModelFactory.Create(name);
                return View["Games", gamesViewModel];
            };
        }
    }
}