

namespace KMorcinek.ShowMyHaxballGames.Models
{
    public class Event
    {
        public int Id { get; set; }
        public int HaxballLeagueId { get; set; }
        public string SeasonNumber { get; set; }
        public string Title { get; set; }
        public bool IsFromHaxball { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string HardcodedWinner { get; set; }
        public League HaxballLeague { get; set; }
    }
}