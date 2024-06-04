using System.Reflection;
using Avalonia;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaBitmapIcon : AvaloniaElementIcon
{
    public static readonly StyledProperty<Uri> UriSourceProperty = AvaloniaProperty.Register<AvaloniaBitmapIcon, Uri>(nameof(UriSource));

    static AvaloniaBitmapIcon()
    {
        UriSourceProperty.Changed.AddClassHandler<AvaloniaBitmapIcon>((x, e) => x.OnUriSourcePropertyChanged(e));
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