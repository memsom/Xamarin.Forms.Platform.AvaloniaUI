namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaTimeChangedEventArgs(TimeSpan? oldTime, TimeSpan? newTime) : EventArgs
{
    public TimeSpan? OldTime => oldTime;

    public TimeSpan? NewTime => newTime;
}