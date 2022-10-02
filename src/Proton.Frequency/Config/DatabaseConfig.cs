namespace Proton.Frequency.Config; 

public class DatabaseConfig {
    public const string Key = "database";
    public string type { get; set; } = "sqlite";
    public string Store { get; set; } = "Data Source=app.sqlite3;cache=shared";
    public bool Persist { get; set; } = true;
    public int PersistDuration { get; set; } = 86400;
}
