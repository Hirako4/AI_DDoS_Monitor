using Npgsql;

namespace AI_DDoS_Monitor.Database
{
    public static class DbConnection
    {
        public static string ConnectionString { get; set; } = "Host=localhost;Database=ai_ddos_db;Username=postgres;Password=weerok";
    }
}