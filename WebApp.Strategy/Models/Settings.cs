namespace WebApp.Strategy.Models
{
    public class Settings
    {
        public static string claimDbType = "databaseType";
        public EnumDatabaseType DatabaseType;
        public EnumDatabaseType GetDefaultDbType => EnumDatabaseType.SqlServer;
    }
}
