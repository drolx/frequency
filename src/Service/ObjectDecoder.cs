namespace Proton.Frequency;

internal abstract class ObjectDecoder {
    protected void Init() { }

    protected void OnMessageEvent() { }

    protected object HandleEmptyEvent() => new object();

    protected abstract object Decode(object msg);
}
