using Avalonia;
using AvaloniaBrush = Avalonia.Media.Brush;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaTabbedPage : AvaloniaMultiContentPage
{
    public static readonly StyledProperty<AvaloniaBrush> BarBackgroundColorProperty = AvaloniaProperty.Register<AvaloniaTabbedPage, AvaloniaBrush>(nameof(BarBackgroundColor));
    public static readonly StyledProperty<AvaloniaBrush> BarTextColorProperty = AvaloniaProperty.Register<AvaloniaTabbedPage, AvaloniaBrush>(nameof(BarTextColor));

    static AvaloniaTabbedPage()
    {
    }

    protected override Type StyleKeyOverride => typeof(AvaloniaTabbedPage);


    public AvaloniaBrush BarBackgroundColor
    {
        get => GetValue(BarBackgroundColorProperty);
        set => SetValue(BarBackgroundColorProperty, value);
    }

    public AvaloniaBrush BarTextColor
    {
        get => GetValue(BarTextColorProperty);
        set => SetValue(BarTextColorProperty, value);
    }
}