using System.Reflection;
using Avalonia;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class BitmapIcon : ElementIcon
{
    public static readonly StyledProperty<Uri> UriSourceProperty = AvaloniaProperty.Register<BitmapIcon, Uri>(nameof(UriSource));

    static BitmapIcon()
    {
        UriSourceProperty.Changed.AddClassHandler<BitmapIcon>((x, e) => x.OnUriSourcePropertyChanged(e));
    }

    public Uri UriSource
    {
        get => GetValue(UriSourceProperty);
        set => SetValue(UriSourceProperty, value);
    }

    private void OnUriSourcePropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var newValue = e.NewValue;
        if (newValue is Uri uri && !uri.IsAbsoluteUri)
        {
            var name = Assembly.GetEntryAssembly().GetName().Name;
            UriSource = new Uri(string.Format("pack://application:,,,/{0};component/{1}", name, uri.OriginalString));
        }
    }
}