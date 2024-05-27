namespace Xamarin.Forms.Platform.AvaloniaUI;

public class VisualElementChangedEventArgs(VisualElement? oldElement, VisualElement? newElement) : ElementChangedEventArgs<VisualElement>(oldElement, newElement);