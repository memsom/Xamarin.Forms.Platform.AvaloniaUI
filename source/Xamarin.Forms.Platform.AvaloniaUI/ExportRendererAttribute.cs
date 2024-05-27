namespace Xamarin.Forms.Platform.AvaloniaUI;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ExportRendererAttribute(Type handler, Type target, Type[] supportedVisuals) : HandlerAttribute(handler, target, supportedVisuals)
{
    public ExportRendererAttribute(Type handler, Type target) : this(handler, target, null)
    {
    }
}