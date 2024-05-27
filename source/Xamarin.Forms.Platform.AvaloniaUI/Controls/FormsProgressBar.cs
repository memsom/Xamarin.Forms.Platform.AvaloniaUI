using Avalonia;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsProgressBar : Avalonia.Controls.ProgressBar
{
    public static readonly StyledProperty<double> ElementOpacityProperty = AvaloniaProperty.Register<FormsProgressBar, double>(nameof(ElementOpacity));

    public double ElementOpacity
    {
        get => GetValue(ElementOpacityProperty);
        set => SetValue(ElementOpacityProperty, value);
    }
}