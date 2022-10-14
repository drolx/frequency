namespace Proton.Frequency;

internal abstract class ObjectDecoder
{
    private string _config;

    protected void Init() { }

    protected void OnMessageEvent() { }

    protected object HandleEmptyEvent()
    {
        return null;
    }

    protected abstract object Decode(object msg);
}
