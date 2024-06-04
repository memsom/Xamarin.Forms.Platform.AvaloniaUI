using System.ComponentModel;
using Avalonia;
using Avalonia.Controls.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(ProgressBar), typeof(ProgressBarRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ProgressBarRenderer : ViewRenderer<ProgressBar, FormsProgressBar>
{
    protected override void OnElementChanged(ElementChangedEventArgs<ProgressBar> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsProgressBar { Minimum = 0, Maximum = 1 });
                Control!.PropertyChanged += Control_PropertyChanged;
            }

            // Update control property
            UpdateProgress();
            UpdateProgressColor();
        }

        base.OnElementChanged(e);
    }

    private void Control_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if(e.Property == RangeBase.ValueProperty)
        {
            HandleValueChanged(sender, e);
        }
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == ProgressBar.ProgressProperty.PropertyName)
        {
            UpdateProgress();
        }
        else if (e.PropertyName == ProgressBar.ProgressColorProperty.PropertyName)
        {
            UpdateProgressColor();
        }
    }

    void UpdateProgressColor()
    {
        Control.UpdateDependencyColor(TemplatedControl.ForegroundProperty, Element.ProgressColor.IsDefault ? Color.DeepSkyBlue : Element.ProgressColor);
    }

    void UpdateProgress()
    {
        Control.Value = Element.Progress;
    }

    void HandleValueChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.MeasureChanged);
    }

    bool _isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            if (Control != null)
            {
                Control.PropertyChanged -= Control_PropertyChanged;
            }
        }

        _isDisposed = true;
        base.Dispose(disposing);
    }
}