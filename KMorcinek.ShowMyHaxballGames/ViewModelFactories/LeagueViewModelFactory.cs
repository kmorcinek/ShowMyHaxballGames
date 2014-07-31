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
            var eventEntry = db.UseOnceTo().GetByQuery<Event>(t => t.HaxballLeagueId == leagueId);

            var games = eventEntry.HaxballLeague.Games
                .OrderByDescending(g => g.PlayedDate)
                .Take(ShownLastGamesCount)
                .Where(g => g.Result != Constants.NotPlayed);

            var leagueViewModel = new LeagueViewModel(eventEntry)
            {
                NewestGames = games
            };

            return leagueViewModel;
        }
    }
}