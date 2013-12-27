using KMorcinek.ShowMyHaxballGames.ViewModelFactories;
using NUnit.Framework;

namespace KMorcinek.ShowMyHaxballGames.Tests
{
    [TestFixture]
    public class ExploratoryTests
    {
        [Test]
        public void AgilityTests()
        {
            var gamesViewModelFactory = new GamesViewModelFactory();
            gamesViewModelFactory.Create("Filip");
        }
    }
}