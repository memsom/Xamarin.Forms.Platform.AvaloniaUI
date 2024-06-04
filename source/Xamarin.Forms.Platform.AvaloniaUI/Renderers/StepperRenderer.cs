using System.ComponentModel;
using Avalonia.Interactivity;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(Stepper), typeof(StepperRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class StepperRenderer : ViewRenderer<Stepper, FormsStepper>
{
    protected override void OnElementChanged(ElementChangedEventArgs<Stepper> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(CreateControl());
            }

            // Update control property
            UpdateButtons();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Stepper.MinimumProperty.PropertyName || e.PropertyName == Stepper.MaximumProperty.PropertyName ||
            e.PropertyName == Stepper.ValueProperty.PropertyName)
        {
            UpdateButtons();
        }
    }

    FormsStepper CreateControl()
    {
        var stepper = new FormsStepper();
        stepper.UpClicked += UpButtonOnClick;
        stepper.DownClicked += DownButtonOnClick;
        return stepper;
    }

    void DownButtonOnClick(object? sender, RoutedEventArgs routedEventArgs) => ((IElementController)Element).SetValueFromRenderer(Stepper.ValueProperty, Math.Max(Element.Minimum, Element.Value - Element.Increment));

    void UpButtonOnClick(object? sender, RoutedEventArgs routedEventArgs) => ((IElementController)Element).SetValueFromRenderer(Stepper.ValueProperty, Math.Min(Element.Maximum, Element.Value + Element.Increment));

    void UpdateButtons()
    {
        Control.IsUpEnabled = Element.Value < Element.Maximum;
        Control.IsDownEnabled = Element.Value > Element.Minimum;
    }

    protected override void UpdateEnabled() => Control.IsEnabled = Element.IsEnabled;

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing && Control != null)
        {
            Control.Dispose();
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}