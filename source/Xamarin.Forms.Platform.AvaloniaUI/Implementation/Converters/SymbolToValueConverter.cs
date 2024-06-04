using System.Globalization;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public class SymbolToValueConverter : IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture) => value is Symbol symbol ? Char.ConvertFromUtf32((int)symbol) : null;

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => null;
}