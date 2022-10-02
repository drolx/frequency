namespace Proton.Frequency.Config;

internal interface IConfig
{
    public static string Key { get; set; }
    private static bool List { get; set; }
    public string Identifier => Key;
    public bool IsList => List;
}
