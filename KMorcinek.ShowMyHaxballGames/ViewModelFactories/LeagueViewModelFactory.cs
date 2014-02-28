using System.Linq;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.ViewModels;

namespace KMorcinek.ShowMyHaxballGames.ViewModelFactories
{
    public class LeagueViewModelFactory
    {
        private const int ShownLastGamesCount = 8;

        public LeagueViewModel Create(int leagueId)
        {
            var db = DbRepository.GetDb();
            var league = db.UseOnceTo().GetByQuery<League>(t => t.LeagueNumer == leagueId);

            var games = league.Games
                .OrderByDescending(g => g.PlayedDate)
                .Take(ShownLastGamesCount)
                .Where(g => g.Result != Constants.NotPlayed);

            var leagueViewModel = new LeagueViewModel
            {
                LeagueId = leagueId,
                Title = league.Title,
                Players = league.Players,
                NewestGames = games
            };

            return leagueViewModel;
        }
    }
}