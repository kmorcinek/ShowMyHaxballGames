using System;

namespace KMorcinek.ShowMyHaxballGames.Utils
{
    class RealTimeProvider : ITimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.Now;
        }
    }
}