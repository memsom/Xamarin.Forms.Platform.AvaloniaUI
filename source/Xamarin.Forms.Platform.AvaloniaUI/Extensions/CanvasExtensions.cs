using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Extensions;

public static class CanvasExtensions
{
    public static int GetZIndex(Control child)
    {
        return child.ZIndex;
    }

    public static void SetZIndex(Control child, int zIndex)
    {
        child.ZIndex = zIndex;
    }

    public static Size ToSize(this global::Avalonia.Size size)
    {
        return new Size(size.Width, size.Height);
    }
}