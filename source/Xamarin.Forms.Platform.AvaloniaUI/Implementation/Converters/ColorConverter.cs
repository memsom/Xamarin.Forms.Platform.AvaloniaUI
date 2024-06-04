using System.Globalization;
using Avalonia.Media;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using AvaloniaSolidColorBrush = Avalonia.Media.SolidColorBrush;
using AvaloniaBrush = Avalonia.Media.Brush;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public sealed class ColorConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var color = (Color)value;
        var defaultColorKey = (string)parameter;

        var defaultBrush = defaultColorKey != null && global::Avalonia.Application.Current.Resources.ContainsKey(defaultColorKey)
            ? (AvaloniaBrush)global::Avalonia.Application.Current.Resources[defaultColorKey] :
            new AvaloniaSolidColorBrush(Colors.Transparent);
        return color == Color.Default ? defaultBrush : color.ToNativeBrush();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}