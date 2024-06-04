using System.ComponentModel;
using Avalonia.Interactivity;
using Avalonia.Media;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using AvaloniaBrush = Avalonia.Media.Brush;

[assembly: ExportRenderer(typeof(TimePicker), typeof(TimePickerRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class TimePickerRenderer : ViewRenderer<TimePicker, FormsTimePicker>
{
    AvaloniaBrush? defaultBrush;
    bool fontApplied;
    FontFamily defaultFontFamily;

    protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
    {
        base.OnElementChanged(e);

        if (e.NewElement != null)
        {
            if (Control == null)
            {
                var picker = new FormsTimePicker();
                SetNativeControl(picker);

                Control.TimeChanged += OnControlTimeChanged;
                Control.AttachedToVisualTree += Control_AttachedToVisualTree;
            }

            UpdateTime();
            UpdateFlowDirection();
            UpdateTimeFormat();
        }
    }

    private void Control_AttachedToVisualTree(object? sender, Avalonia.VisualTreeAttachmentEventArgs e)
    {
        ControlOnLoaded(sender, new RoutedEventArgs());
    }

    void ControlOnLoaded(object? sender, RoutedEventArgs routedEventArgs)
    {
        // The defaults from the control template won't be available
        // right away; we have to wait until after the template has been applied
        defaultBrush = Control.Foreground as AvaloniaBrush;
        defaultFontFamily = Control.FontFamily;
        UpdateFont();
        UpdateTextColor();
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == TimePicker.TimeProperty.PropertyName)
            UpdateTime();
        else if (e.PropertyName == TimePicker.TextColorProperty.PropertyName)
            UpdateTextColor();
        else if (e.PropertyName == TimePicker.FontAttributesProperty.PropertyName || e.PropertyName == TimePicker.FontFamilyProperty.PropertyName || e.PropertyName == TimePicker.FontSizeProperty.PropertyName)
            UpdateFont();

        if (e.PropertyName == TimePicker.FormatProperty.PropertyName)
            UpdateTimeFormat();

        if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
            UpdateFlowDirection();
    }

    void OnControlTimeChanged(object sender, AvaloniaTimeChangedEventArgs e)
    {
        Element.Time = e.NewTime.HasValue ? e.NewTime.Value : (TimeSpan)TimePicker.TimeProperty.DefaultValue;
        ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.SizeRequestChanged);
    }

    void UpdateTimeFormat() => Control.TimeFormat = Element.Format;

    void UpdateFlowDirection()
    {
        //Control.FlowDirection = Element.FlowDirection == Xamarin.Forms.FlowDirection.RightToLeft ? System.Windows.FlowDirection.RightToLeft : System.Windows.FlowDirection.LeftToRight;
    }

    void PickerOnForceInvalidate(object sender, EventArgs eventArgs) => ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.SizeRequestChanged);

    void UpdateFont()
    {
        if (Control == null)
            return;

        TimePicker timePicker = Element;

        if (timePicker == null)
            return;

        bool timePickerIsDefault = timePicker.FontFamily == null && timePicker.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(TimePicker), true) && timePicker.FontAttributes == FontAttributes.None;

        if (timePickerIsDefault && !fontApplied)
            return;

        if (timePickerIsDefault)
        {
            Control.ClearValue(Avalonia.Controls.Primitives.TemplatedControl.FontStyleProperty);
            Control.ClearValue(Avalonia.Controls.Primitives.TemplatedControl.FontSizeProperty);
            Control.ClearValue(Avalonia.Controls.Primitives.TemplatedControl.FontFamilyProperty);
            Control.ClearValue(Avalonia.Controls.Primitives.TemplatedControl.FontWeightProperty);
        }
        else
        {
            Control.ApplyFont(timePicker);
        }

        fontApplied = true;
    }

    void UpdateTime() => Control.Time = Element.Time;

    void UpdateTextColor()
    {
        Color color = Element.TextColor;
        Control.Foreground = color.IsDefault ? (defaultBrush ?? color.ToNativeBrush()) : color.ToNativeBrush();
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing && Control != null)
        {
            Control.TimeChanged -= OnControlTimeChanged;
            Control.AttachedToVisualTree -= Control_AttachedToVisualTree;
        }

        base.Dispose(disposing);
    }
}