namespace Proton.Frequency;

internal abstract class ObjectDecoder {
    protected void Init() { }

    protected void OnMessageEvent() { }

    protected object HandleEmptyEvent() {
        return null;
    }

    protected abstract object Decode(object msg);
}
