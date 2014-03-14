using KMorcinek.ShowMyHaxballGames.ViewModelFactories;
using Xunit;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    public class ExploratoryTests
    {
        [Fact(Skip = "Exploratory Test")]
        public void ExploratoryTest()
        {
            var gamesViewModelFactory = new GamesViewModelFactory();
            gamesViewModelFactory.Create(0, "Filip");
        }
    }
}