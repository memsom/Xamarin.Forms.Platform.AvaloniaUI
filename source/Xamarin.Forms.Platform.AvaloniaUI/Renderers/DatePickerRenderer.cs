using System.ComponentModel;
using Avalonia.Controls.Primitives;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using AvaloniaSelectionChangedEventArgs = Avalonia.Controls.SelectionChangedEventArgs;

[assembly: ExportRenderer(typeof(DatePicker), typeof(DatePickerRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class DatePickerRenderer : ViewRenderer<DatePicker, FormsDatePicker>
{
    protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsDatePicker());
                Control.SelectedDateChanged += OnNativeSelectedDateChanged;
            }

            // Update control property
            UpdateDate();
            UpdateMinimumDate();
            UpdateMaximumDate();
            UpdateTextColor();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == DatePicker.DateProperty.PropertyName)
        {
            UpdateDate();
        }
        else if (e.PropertyName == DatePicker.MaximumDateProperty.PropertyName)
        {
            UpdateMaximumDate();
        }
        else if (e.PropertyName == DatePicker.MinimumDateProperty.PropertyName)
        {
            UpdateMinimumDate();
        }
        else if (e.PropertyName == DatePicker.TextColorProperty.PropertyName)
        {
            UpdateTextColor();
        }
    }

    void UpdateDate()
    {
        Control.SelectedDate = Element.Date;
    }

    void UpdateMaximumDate()
    {
        Control.DisplayDateEnd = Element.MaximumDate;
    }

    void UpdateMinimumDate()
    {
        Control.DisplayDateStart = Element.MinimumDate;
    }

    void UpdateTextColor()
    {
        Control.UpdateDependencyColor(TemplatedControl.ForegroundProperty, Element.TextColor);
    }

    void OnNativeSelectedDateChanged(object? sender, AvaloniaSelectionChangedEventArgs e)
    {
        if (Control?.SelectedDate is {} date && Element is IElementController element)
        {
            element.SetValueFromRenderer(DatePicker.DateProperty, date);
        }
    }

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing && Control != null)
        {
            Control.SelectedDateChanged -= OnNativeSelectedDateChanged;
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}