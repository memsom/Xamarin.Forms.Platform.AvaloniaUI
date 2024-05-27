using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;

public static class CommandsExtensions
{
    public static IEnumerable<Control> Merge(this IEnumerable<Control> originalCommands, ContentControl contentControl, Func<DynamicContentPage, IEnumerable<Control>> callback)
    {
        List<Control> frameworkElements = new List<Control>();
        frameworkElements.AddRange(originalCommands);

        if (contentControl?.Content is DynamicContentPage page)
        {
            frameworkElements.AddRange(callback(page));
        }

        return frameworkElements;
    }
}