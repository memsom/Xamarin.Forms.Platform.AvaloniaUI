using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public class DefaultViewRenderer : ViewRenderer<View, UserControl>
{
    protected override void OnElementChanged(ElementChangedEventArgs<View> e)
    {
        base.OnElementChanged(e);
        SetNativeControl(new UserControl());
    }
}