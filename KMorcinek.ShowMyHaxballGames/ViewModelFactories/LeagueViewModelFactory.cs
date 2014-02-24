using System.Linq;
using HtmlAgilityPack;
using KMorcinek.ShowMyHaxballGames.Business;
using KMorcinek.ShowMyHaxballGames.Models;
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

            leagueViewModel.Title = LeagueTitleParser.GetLeagueTitle(document);

            var db = DbRepository.GetDb();
            var league = db.UseOnceTo().GetByQuery<League>(t => t.LeagueNumer == leagueId);

            var games = league.Games
                .OrderByDescending(g => g.PlayedDate)
                .Take(8)
                .Where(g => g.Result != Constants.NotPlayed);

            leagueViewModel.NewestGames = games;

            return leagueViewModel;
        }
    }
}