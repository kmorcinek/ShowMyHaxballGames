using System;
using System.Text;
using KMorcinek.ShowMyHaxballGames.ViewModelFactories;
using KMorcinek.ShowMyHaxballGames.ViewModels;

namespace KMorcinek.ShowMyHaxballGames
{
    using Nancy;

    public class IndexModule : NancyModule
    {
        public IndexModule()
        {
            Get["/"] = parameters => View["index"];

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