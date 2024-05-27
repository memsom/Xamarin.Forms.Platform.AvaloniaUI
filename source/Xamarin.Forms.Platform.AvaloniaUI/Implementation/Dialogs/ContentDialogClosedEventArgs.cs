namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Dialogs;

public sealed class ContentDialogClosedEventArgs
{
    public ContentDialogResult Result { get; }

    public ContentDialogClosedEventArgs(ContentDialogResult result)
    {
        Result = result;
    }
}