using KMorcinek.ShowMyHaxballGames.ViewModels;

namespace KMorcinek.ShowMyHaxballGames.Business
{
    public class LeaguesProvider
    {
        public LeagueViewModel[] Get()
        {
            return new[]
            {
                new LeagueViewModel {LeagueId = 126002, Title = "MW League S02 Div1", SeasonNumber = 2},
                new LeagueViewModel {LeagueId = 126003, Title = "MW League S02 Div2", SeasonNumber = 2},
                new LeagueViewModel {LeagueId = 126004, Title = "MW League S02 Div3A", SeasonNumber = 2},
                new LeagueViewModel {LeagueId = 126005, Title = "MW League S02 Div3B", SeasonNumber = 2},
                new LeagueViewModel {LeagueId = 121729, Title = "MW League Season 1", SeasonNumber = 1},
            };
        }
    }
}