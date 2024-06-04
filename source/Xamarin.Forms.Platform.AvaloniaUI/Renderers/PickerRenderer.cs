using System.ComponentModel;
using Avalonia.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(Picker), typeof(PickerRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class PickerRenderer : ViewRenderer<Picker, ComboBox>
{
    protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new ComboBox());
                Control.SelectionChanged += OnControlSelectionChanged;
            }

            // Update control property
            UpdateTitle();
            UpdateSelectedIndex();
            UpdateTextColor();
            Control.Items.ReplaceRange(Element.Items);
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Picker.TitleProperty.PropertyName)
        {
            UpdateTitle();
        }
        else if (e.PropertyName == Picker.SelectedIndexProperty.PropertyName)
        {
            UpdateSelectedIndex();
        }
        else if (e.PropertyName == Picker.TextColorProperty.PropertyName)
        {
            UpdateTextColor();
        }
    }

    void UpdateTitle()
    {
        //TODO: Create full size combobox
    }

    void UpdateTextColor()
    {
        Control.UpdateDependencyColor(ComboBox.ForegroundProperty, Element.TextColor);
    }

    void UpdateSelectedIndex()
    {
        Control.SelectedIndex = Element.SelectedIndex;
    }

    private void OnControlSelectionChanged(object? sender, global::Avalonia.Controls.SelectionChangedEventArgs e)
    {
        if (Element != null)
        {
            Element.SelectedIndex = Control.SelectedIndex;
        }

        // TODO: The control is not refresh.
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
                Control.SelectionChanged -= OnControlSelectionChanged;
                Control.Items.Clear();
            }

            if (Element != null)
            {

            }
        }

        _isDisposed = true;
        base.Dispose(disposing);
    }
}