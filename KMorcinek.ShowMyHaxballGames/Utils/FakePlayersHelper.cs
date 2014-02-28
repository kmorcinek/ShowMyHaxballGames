namespace KMorcinek.ShowMyHaxballGames.Utils
{
    public class FakePlayersHelper
    {
        public static bool IsNotFake(string playerName)
        {
            return playerName.StartsWith("Fake") == false;
        }
    }
}