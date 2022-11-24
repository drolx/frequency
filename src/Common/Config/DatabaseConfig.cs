using Proton.Frequency.Common.Interfaces;

namespace Proton.Frequency.Common.Config;

public class DatabaseConfig : IConfig {
    public const string Key = "database";
    public DatabaseType Type { get; set; } = DatabaseType.SQLITE;
    public string Store { get; set; } = "Data Source=app.sqlite3;cache=shared";
    public bool Persist { get; set; } = true;
    public int PersistDuration { get; set; } = 86400;
}

public enum DatabaseType {
    SQLITE,
    POSTGRES
}
