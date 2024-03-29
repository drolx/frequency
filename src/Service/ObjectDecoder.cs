namespace Frequency;

internal abstract class ObjectDecoder {
    protected void Init() { }

    protected void OnMessageEvent() { }

    protected object HandleEmptyEvent() => new();

    protected abstract object Decode(object msg);
}