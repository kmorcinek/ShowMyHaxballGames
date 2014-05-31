using SisoDb;
using SisoDb.SqlCe4;

namespace KMorcinek.ShowMyHaxballGames
{
    public class DbRepository
    {
        public static ISisoDatabase GetDb()
        {
            return "Data source=|DataDirectory|Haxball.sdf;".CreateSqlCe4Db();
        }

        public static void Initialize()
        {
            var db = DbRepository.GetDb();

            if (db.Exists() == false)
            {
                db.EnsureNewDatabase();
            }
        }
    }
}