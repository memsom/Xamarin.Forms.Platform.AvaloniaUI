using AvaloniaScrollViewer = Avalonia.Controls.ScrollViewer;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsScrollViewer : AvaloniaScrollViewer
{
    protected override Type StyleKeyOverride => typeof(AvaloniaScrollViewer);
}