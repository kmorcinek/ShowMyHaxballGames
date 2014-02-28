namespace KMorcinek.ShowMyHaxballGames.Utils
{
    public class TotalGamesCalculator
    {
        public static int Calculate(int playersCount)
        {
            return (playersCount - 1) * playersCount;
        }
    }
}