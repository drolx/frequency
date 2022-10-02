namespace Proton.Frequency.Config;

public class DatabaseConfig : IConfig
{
    public static string Key { get; set; } = "database";
    private static bool List { get; set; } = false;
    public string Identifier => Key;
    public bool IsList => List;
    public DatabasesAllow Type { get; set; } = DatabasesAllow.SQLITE;
    public string Store { get; set; } = "Data Source=app.sqlite3;cache=shared";
    public bool Persist { get; set; } = true;
    public int PersistDuration { get; set; } = 86400;
}

public enum DatabasesAllow
{
    SQLITE,
    POSTGRES
}
