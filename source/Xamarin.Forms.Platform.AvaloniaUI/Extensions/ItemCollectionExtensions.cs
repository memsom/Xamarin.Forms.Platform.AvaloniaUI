using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Extensions;

public static class ItemCollectionExtensions
{
    public static void AddRange<T>(this ItemCollection instance, IEnumerable<T> items)
    {
        foreach (var item in items)
        {
            instance.Add(item);
        }
    }
    
    public static void ReplaceRange<T>(this ItemCollection instance, IEnumerable<T> items)
    {
        instance.Clear();
        instance.AddRange(items);
    }
}