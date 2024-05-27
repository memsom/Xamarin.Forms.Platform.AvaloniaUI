namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Dialogs;

public sealed class ContentDialogClosingEventArgs
{
    public bool Cancel { get; set; }
    public ContentDialogResult Result { get; }

    public ContentDialogClosingEventArgs(ContentDialogResult result)
    {
        Result = result;
    }
}