namespace Frequency;

internal interface IProtocol {
    string GetName();

    IEnumerable<string> GetSupportedCommands();

    void SendCommands();
}