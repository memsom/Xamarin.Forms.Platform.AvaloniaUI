using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Styling;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public partial class XFAvaloniaUI() : ResourceDictionary
{
    public static readonly XFAvaloniaUI Instance = new ();
}