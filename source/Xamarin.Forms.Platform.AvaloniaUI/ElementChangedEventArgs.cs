namespace Xamarin.Forms.Platform.AvaloniaUI;

public class ElementChangedEventArgs<TElement>(TElement? oldElement, TElement? newElement) : EventArgs
    where TElement : Element
{
    public TElement? NewElement { get; private set; } = newElement;

    public TElement? OldElement { get; private set; } = oldElement;
}