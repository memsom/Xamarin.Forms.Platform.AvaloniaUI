using System.Globalization;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public sealed class CollapseWhenEmptyConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) =>
        value switch
        {
            string s => s.Length > 0,
            int i => i > 0,
            _ => false
        };

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}