using System.Globalization;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public class HorizontalTextAlignmentConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var textAlign = (TextAlignment)value;
        return textAlign.ToNativeTextAlignment();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotSupportedException();
}