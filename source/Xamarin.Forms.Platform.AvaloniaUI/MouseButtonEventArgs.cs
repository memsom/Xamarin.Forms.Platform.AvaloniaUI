using Avalonia.Controls;
using AvaloniaPoint = Avalonia.Point;
using AvaloniaPointerReleasedEventArgs = Avalonia.Input.PointerReleasedEventArgs;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public class MouseButtonEventArgs(AvaloniaPointerReleasedEventArgs e)
{
    public bool Handled
    {
        get => e.Handled;
        set => e.Handled = value;
    }

    public int ClickCount => 1; // TODO - this needs to match actual count

    public AvaloniaPoint GetPosition(Control fe)
    {
        return e.GetPosition(fe);
    }
}