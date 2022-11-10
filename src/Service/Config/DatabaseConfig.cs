namespace Proton.Frequency.Config;

public class DatabaseConfig : IConfig {
    public const string Key = "database";
    public const bool IsList = false;
    public DatabasesAllow Type { get; set; } = DatabasesAllow.SQLITE;
    public string Store { get; set; } = "Data Source=app.sqlite3;cache=shared";
    public bool Persist { get; set; } = true;
    public int PersistDuration { get; set; } = 86400;
}

public enum DatabasesAllow {
    SQLITE,
    POSTGRES
}
