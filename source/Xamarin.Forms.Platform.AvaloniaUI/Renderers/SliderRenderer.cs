using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using AvaloniaSlider = Avalonia.Controls.Slider;

[assembly: ExportRenderer(typeof(Slider), typeof(SliderRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class SliderRenderer : ViewRenderer<Slider, AvaloniaSlider>
{
    protected override void OnElementChanged(ElementChangedEventArgs<Slider> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new AvaloniaSlider());
                Control.PropertyChanged += Control_PropertyChanged;
            }

            // Update control property
            UpdateMinimum();
            UpdateMaximum();
            UpdateValue();
        }

        base.OnElementChanged(e);
    }

    private void Control_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == RangeBase.ValueProperty)
        {
            HandleValueChanged(sender, e);
        }
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Slider.MinimumProperty.PropertyName)
        {
            UpdateMinimum();
        }
        else if (e.PropertyName == Slider.MaximumProperty.PropertyName)
        {
            UpdateMaximum();
        }
        else if (e.PropertyName == Slider.ValueProperty.PropertyName)
        {
            UpdateValue();
        }
    }

    void UpdateMinimum() => Control.Minimum = Element.Minimum;

    void UpdateMaximum() => Control.Maximum = Element.Maximum;

    void UpdateValue()
    {
        if (Control.Value != Element.Value)
        {
            Control.Value = Element.Value;
        }
    }

    void HandleValueChanged(object? sender, AvaloniaPropertyChangedEventArgs e) =>
        ((IElementController)Element).SetValueFromRenderer(Slider.ValueProperty, Control.Value);

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing && Control != null)
        {
            Control.PropertyChanged -= Control_PropertyChanged;
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}