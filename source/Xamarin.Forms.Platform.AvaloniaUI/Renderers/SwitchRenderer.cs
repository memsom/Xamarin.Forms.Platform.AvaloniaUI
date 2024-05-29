using System.ComponentModel;
using Avalonia.Interactivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(Switch), typeof(SwitchRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class SwitchRenderer : ViewRenderer<Switch, global::Avalonia.Controls.CheckBox>
{
    protected override void OnElementChanged(ElementChangedEventArgs<Switch> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new global::Avalonia.Controls.CheckBox());
                Control!.IsCheckedChanged += OnNativeToggled;
            }

            // Update control property
            UpdateIsToggled();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Switch.IsToggledProperty.PropertyName)
        {
            UpdateIsToggled();
        }
    }

    void UpdateIsToggled()
    {
        Control.IsChecked = Element.IsToggled;
    }

    void OnNativeToggled(object? sender, RoutedEventArgs e)
    {
        ((IElementController)Element).SetValueFromRenderer(Switch.IsToggledProperty, Control.IsChecked);
    }

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
            return;

        if (disposing && Control != null)
        {
            Control.IsCheckedChanged -= OnNativeToggled;
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}