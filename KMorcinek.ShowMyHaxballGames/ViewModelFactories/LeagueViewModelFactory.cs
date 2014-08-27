using System.Linq;
using KMorcinek.ShowMyHaxballGames.Models;
using KMorcinek.ShowMyHaxballGames.ViewModels;
using KMorcinek.ShowMyHaxballGames.WebApi;

namespace KMorcinek.ShowMyHaxballGames.ViewModelFactories
{
    public class LeagueViewModelFactory
    {
        public LeagueViewModel Create(int leagueId)
        {
            var db = DbRepository.GetDb();
            var eventEntry = db.UseOnceTo().GetByQuery<Event>(t => t.HaxballLeagueId == leagueId);

            Configuration configuration = db.UseOnceTo().GetById<Configuration>(ConfigurationModule.HardcodedConfigurationId);

            var games = eventEntry.HaxballLeague.Games
                .OrderByDescending(g => g.PlayedDate)
                .Take(configuration.ShowLastGamesCount)
                .Where(g => g.Result != Constants.NotPlayed);

            var leagueViewModel = new LeagueViewModel(eventEntry)
            {
                NewestGames = games
            };

            return leagueViewModel;
        }
    }
}