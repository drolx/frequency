namespace Frequency;

internal abstract class Protocol : IProtocol {
    private readonly string _name;

    // SupportedCommands

    internal Protocol() {
        _name = GetName();
    }

    public string GetName() {
        return _name;
    }

    public IEnumerable<string> GetSupportedCommands() {
        throw new NotImplementedException();
    }

    public void SendCommands() {
        throw new NotImplementedException();
    }
}