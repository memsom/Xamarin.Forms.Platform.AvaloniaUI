using Avalonia.Controls;
using Xamarin.Forms.Internals;
using IResourceDictionary = Xamarin.Forms.Internals.IResourceDictionary;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

internal class AvaloniaResourcesProvider : ISystemResourcesProvider
{
    ResourceDictionary? dictionary;

    public IResourceDictionary GetSystemResources()
    {
        dictionary ??= new ResourceDictionary();

        UpdateStyles();

        return dictionary;
    }

    Style GetStyle(Style style, TextBlock hackbox)
    {
        //hackbox.Style = style;

        var result = new Style(typeof(Label));
        result.Setters.Add(new Setter { Property = Label.FontFamilyProperty, Value = hackbox.FontFamily });
        result.Setters.Add(new Setter { Property = Label.FontSizeProperty, Value = hackbox.FontSize });

        return result;
    }

    void UpdateStyles()
    {
        //var textBlock = new TextBlock();
        //_dictionary[Device.Styles.TitleStyleKey] = GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["HeaderTextBlockStyle"], textBlock);
        //_dictionary[Device.Styles.SubtitleStyleKey] = GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["SubheaderTextBlockStyle"], textBlock);
        //_dictionary[Device.Styles.BodyStyleKey] = GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["BodyTextBlockStyle"], textBlock);
        //_dictionary[Device.Styles.CaptionStyleKey] = GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["CaptionTextBlockStyle"], textBlock);
        //_dictionary[Device.Styles.ListItemTextStyleKey] = GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["BaseTextBlockStyle"], textBlock);
        //_dictionary[Device.Styles.ListItemDetailTextStyleKey] = GetStyle((System.Windows.Style)System.Windows.Application.Current.Resources["BodyTextBlockStyle"], textBlock);
    }
}