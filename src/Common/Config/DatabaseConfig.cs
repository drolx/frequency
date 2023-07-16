using Frequency.Common.Interfaces;

namespace Frequency.Common.Config;

public sealed class DatabaseConfig : IConfig {
    public const string Key = "database";
    public DatabaseType Type { get; set; } = DatabaseType.SQLITE;
    public string Store { get; set; } = "Data Source=app.sqlite3;cache=shared";
    public int Persist { get; set; } = 0;
}

public enum DatabaseType {
    SQLITE,
    POSTGRES
}