using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(IndicatorView), typeof(IndicatorViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class IndicatorViewRenderer : ViewRenderer<IndicatorView, FormsIndicatorView>
{
    protected override void OnElementChanged(ElementChangedEventArgs<IndicatorView> e)
    {
        if (e.NewElement != null && Control == null)
        {
            SetNativeControl(new FormsIndicatorView());
            // TODO:
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        // TODO:
    }
}