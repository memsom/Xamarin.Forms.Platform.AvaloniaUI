using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;

public static class ControlExtensions
{
    public static TopLevel GetParentWindow(this Control control)
    {
        var topLevel = control;
        while(topLevel != null && !(topLevel is TopLevel))
        {
            topLevel = (TopLevel)topLevel.Parent;
        }
        return topLevel as TopLevel;
    }
}