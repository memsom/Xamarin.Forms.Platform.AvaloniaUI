using System.Globalization;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public class KeyboardConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var keyboard = value as Keyboard;
        if (keyboard == null)
            return null;

        return keyboard.ToInputScope();
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}