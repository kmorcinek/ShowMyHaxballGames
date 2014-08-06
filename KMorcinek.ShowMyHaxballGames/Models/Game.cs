using System;

namespace KMorcinek.ShowMyHaxballGames.Models
{
    public class Game
    {
        public string HomePlayer { get; set; }
        public string AwayPlayer { get; set; }
        public string Result { get; set; }
        public DateTime? PlayedDate { get; set; }

        // Normally it should be a struct, 
        // but structs are not serializable to SisoDB
        public Game CreateDeepCopy()
        {
            var game = new Game
            {
                HomePlayer = HomePlayer,
                AwayPlayer = AwayPlayer,
                Result = Result,
                PlayedDate = PlayedDate
            };

            return game;
        }
    }
}