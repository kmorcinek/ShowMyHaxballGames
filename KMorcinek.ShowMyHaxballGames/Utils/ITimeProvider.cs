using System;

namespace KMorcinek.ShowMyHaxballGames.Utils
{
    public interface ITimeProvider
    {
        DateTime GetCurrentTime();
    }
}