using System.ComponentModel;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public interface IVisualNativeElementRenderer : IVisualElementRenderer
{
    event EventHandler<PropertyChangedEventArgs> ElementPropertyChanged;
    event EventHandler ControlChanging;
    event EventHandler ControlChanged;
}