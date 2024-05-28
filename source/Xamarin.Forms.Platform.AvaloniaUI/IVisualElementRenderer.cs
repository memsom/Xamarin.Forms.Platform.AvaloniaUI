using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public interface IVisualElementRenderer : IRegisterable, IDisposable
{
    Control? ContainerElement { get; }

    Control? GetNativeElement();

    VisualElement? Element { get; }

    event EventHandler<VisualElementChangedEventArgs> ElementChanged;

    SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint);

    void SetElement(VisualElement element);
}