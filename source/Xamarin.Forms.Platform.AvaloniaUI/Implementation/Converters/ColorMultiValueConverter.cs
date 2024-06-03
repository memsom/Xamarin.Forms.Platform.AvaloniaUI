using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Xaml;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public sealed class ColorMultiValueConverter : global::Avalonia.Data.Converters.IMultiValueConverter
{
    public object? Convert(object[]? values, Type targetType, object? parameter, CultureInfo culture)
    {
        Control framework = values[0] as Control;
        AvaloniaProperty dp = parameter as AvaloniaProperty;

        if (values.Count() > 1 && framework != null && values[1] is Color && dp != null)
        {
            return framework.UpdateDependencyColor(dp, (Color)values[1]);
        }
        return Color.Transparent.ToNativeBrush();
    }

    public object? Convert(IList<object?> values, Type targetType, object? parameter, CultureInfo culture) => Convert(values?.ToArray(), targetType, parameter, culture);

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture) => throw new NotImplementedException();
}