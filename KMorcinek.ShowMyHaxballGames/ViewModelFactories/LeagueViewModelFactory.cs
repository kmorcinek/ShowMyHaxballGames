using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.ViewModels;

namespace KMorcinek.ShowMyHaxballGames.ViewModelFactories
{
    public class LeagueViewModelFactory
    {
        private readonly LeagueParser _leagueParser = new LeagueParser();

        public LeagueViewModel Create(int leagueId)
        {
            HtmlDocument document = new HtmlWeb().Load("http://www.haxball.gr/league/view/" + leagueId);

            var someNodes = document.DocumentNode.SelectSingleNode("//div[@id='standings']");
            var leagueViewModel = _leagueParser.ParseLeague(someNodes);
            leagueViewModel.LeagueId = leagueId;

            return leagueViewModel;
        }
    }
}