namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaSelectionChangedEventArgs(object? oldElement, object? newElement) : EventArgs
{
    public object? NewElement { get; private set; } = newElement;

    public object? OldElement { get; private set; } = oldElement;
}