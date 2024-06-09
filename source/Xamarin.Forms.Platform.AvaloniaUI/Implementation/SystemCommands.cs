using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public static class SystemCommands
{
    internal static void MinimizeWindow(Window parentWindow)
    {
        parentWindow.WindowState = WindowState.Minimized;
    }

    internal static void RestoreWindow(Window parentWindow)
    {
        parentWindow.WindowState = WindowState.Normal;
    }

    internal static void MaximizeWindow(Window parentWindow)
    {
        parentWindow.WindowState = WindowState.Maximized;
    }
}