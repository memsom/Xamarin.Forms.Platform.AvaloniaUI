using Avalonia;
using Avalonia.Media;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaPathIcon : AvaloniaElementIcon
{
    public static readonly StyledProperty<Geometry> DataProperty = AvaloniaProperty.Register<AvaloniaPathIcon, Geometry>(nameof(Data));

    public Geometry Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}