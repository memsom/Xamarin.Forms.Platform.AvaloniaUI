using Avalonia;
using AvaloniaProgressBar = Avalonia.Controls.ProgressBar;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsProgressBar : AvaloniaProgressBar
{
    protected override Type StyleKeyOverride => typeof(AvaloniaProgressBar);

    public static readonly StyledProperty<double> ElementOpacityProperty = AvaloniaProperty.Register<FormsProgressBar, double>(nameof(ElementOpacity));

    public double ElementOpacity
    {
        get => GetValue(ElementOpacityProperty);
        set => SetValue(ElementOpacityProperty, value);
    }
}