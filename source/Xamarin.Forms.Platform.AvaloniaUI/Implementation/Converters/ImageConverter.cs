using System.Globalization;
using Avalonia.Media.Imaging;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public sealed class ImageConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        var task = (value as ImageSource)?.ToNativeImageSourceAsync();
        return task?.AsAsyncValue() ?? AsyncValue<Bitmap>.Null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}