using System.Globalization;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Converters;

public sealed class ViewToRendererConverter : global::Avalonia.Data.Converters.IValueConverter
{
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is VisualElement visualElement)
        {
            var frameworkElement = Platform.GetOrCreateRenderer(visualElement)?.GetNativeElement();

            if(frameworkElement != null)
            {
                frameworkElement.Initialized += (_, _) =>
                {
                    visualElement.Layout(new Rectangle(0, 0, frameworkElement.Bounds.Width, frameworkElement.Bounds.Height));
                };

                frameworkElement.LayoutUpdated += (_, _) =>
                {
                    visualElement.Layout(new Rectangle(0, 0, frameworkElement.Bounds.Width, frameworkElement.Bounds.Height));
                };

                return frameworkElement;
            }
        }
        return null;
    }

    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}