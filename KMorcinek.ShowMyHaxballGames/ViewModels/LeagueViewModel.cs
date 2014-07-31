using System.Collections.Generic;
using KMorcinek.ShowMyHaxballGames.Models;
using System;

namespace KMorcinek.ShowMyHaxballGames.ViewModels
{
    public class LeagueViewModel
    {
        public int  Id { get; set; }
        public int HaxballLeagueId { get; set; }
        public string Title { get; set; }
        public string SeasonNumber { get; set; }
        public List<string> Players { get; set; }
        public IEnumerable<Game> NewestGames { get; set; }
        public string Winner { get; set; }
        public string WrittenProgress { get; set; }
        public bool IsFromHaxball { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        public string HolidayImageUrl { get; set; }

        public LeagueViewModel(Event eventEntry)
        {
            IsFromHaxball = eventEntry.IsFromHaxball;

            if (IsFromHaxball)
            {
                Id = eventEntry.Id;
                HaxballLeagueId = eventEntry.HaxballLeagueId;
                SeasonNumber = eventEntry.SeasonNumber;

                League league = eventEntry.HaxballLeague;
                Title = league.Title;
                Players = league.Players;
                Winner = league.Winner;

                bool isFinished = league.Progress.Played >= league.Progress.Total;
                WrittenProgress = isFinished
                    ? "Finished"
                    : string.Format("In progress ({0}/{1})", league.Progress.Played, league.Progress.Total);
                HolidayImageUrl = isFinished
                    ? null
                    : string.Format("/HolidayImages/{0}.png", Id);
            }
            else
            {
                SeasonNumber = eventEntry.SeasonNumber;
                Status = eventEntry.Status;
                Url = eventEntry.Url;
                Title = eventEntry.Title;
            }
        }
    }
}