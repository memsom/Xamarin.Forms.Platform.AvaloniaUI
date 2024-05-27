using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(ActivityIndicator), typeof(ActivityIndicatorRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ActivityIndicatorRenderer : ViewRenderer<ActivityIndicator, FormsProgressBar>
{
    protected override void OnElementChanged(ElementChangedEventArgs<ActivityIndicator> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsProgressBar());
            }

            UpdateIsIndeterminate();
            UpdateColor();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == ActivityIndicator.IsRunningProperty.PropertyName)
            UpdateIsIndeterminate();
        else if (e.PropertyName == ActivityIndicator.ColorProperty.PropertyName)
            UpdateColor();
    }

    void UpdateColor()
    {
        Control.UpdateDependencyColor(FormsProgressBar.ForegroundProperty, Element.Color);
    }

    void UpdateIsIndeterminate()
    {
        Control.IsIndeterminate = Element.IsRunning;
    }
}