using Avalonia;
using Avalonia.Media;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class PathIcon : ElementIcon
{
    public static readonly StyledProperty<Geometry> DataProperty = AvaloniaProperty.Register<PathIcon, Geometry>(nameof(Data));

    public Geometry Data
    {
        get => (Geometry)GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }
}