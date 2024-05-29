using System.ComponentModel;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using AvaloniaBrush = Avalonia.Media.Brush;

[assembly: ExportRenderer(typeof(Editor), typeof(EditorRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class EditorRenderer : ViewRenderer<Editor, FormsTextBox>
{
    AvaloniaBrush? placeholderDefaultBrush;
    bool fontApplied;

    protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsTextBox
                {
                    TextWrapping = TextWrapping.Wrap,
                    AcceptsReturn = true
                });
                Control.LostFocus += NativeOnLostFocus;
                Control.TextChanged += NativeOnTextChanged;
            }

            // Update control property
            UpdateText();
            UpdatePlaceholder();
            UpdateInputScope();
            UpdateTextColor();
            UpdatePlaceholderColor();
            UpdateFont();
            UpdateMaxLength();
            UpdateIsReadOnly();
        }

        base.OnElementChanged(e);
    }


    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Editor.TextProperty.PropertyName)
        {
            UpdateText();
        }
        else if (e.PropertyName == InputView.KeyboardProperty.PropertyName)
        {
            UpdateInputScope();
        }
        else if (e.PropertyName == Editor.TextColorProperty.PropertyName)
        {
            UpdateTextColor();
        }
        else if (e.PropertyName == Editor.FontAttributesProperty.PropertyName)
        {
            UpdateFont();
        }
        else if (e.PropertyName == Editor.FontFamilyProperty.PropertyName)
        {
            UpdateFont();
        }
        else if (e.PropertyName == Editor.FontSizeProperty.PropertyName)
        {
            UpdateFont();
        }
        else if (e.PropertyName == InputView.MaxLengthProperty.PropertyName)
        {
            UpdateMaxLength();
        }
        else if (e.PropertyName == InputView.IsReadOnlyProperty.PropertyName)
        {
            UpdateIsReadOnly();
        }
        else if (e.PropertyName == Editor.PlaceholderProperty.PropertyName)
        {
            UpdatePlaceholder();
        }
        else if (e.PropertyName == Editor.PlaceholderColorProperty.PropertyName)
        {
            UpdatePlaceholderColor();
        }
    }

    void UpdatePlaceholder() => Control.PlaceholderText = Element.Placeholder ?? string.Empty;

    void UpdatePlaceholderColor()
    {
        Color placeholderColor = Element.PlaceholderColor;

        if (placeholderColor.IsDefault)
        {
            if (placeholderDefaultBrush == null)
            {
                placeholderDefaultBrush = (AvaloniaBrush)TemplatedControl.ForegroundProperty.GetMetadata(typeof(FormsTextBox)).GetDefaultValue();
            }

            // Use the cached default brush
            Control.PlaceholderForegroundBrush = placeholderDefaultBrush;
            return;
        }

        if (placeholderDefaultBrush == null)
        {
            // Cache the default brush in case we need to set the color back to default
            placeholderDefaultBrush = Control.PlaceholderForegroundBrush;
        }

        Control.PlaceholderForegroundBrush = placeholderColor.ToNativeBrush();
    }

    void NativeOnTextChanged(object? sender, RoutedEventArgs e) => ((IElementController)Element).SetValueFromRenderer(Editor.TextProperty, Control.Text);

    void NativeOnLostFocus(object? sender, RoutedEventArgs e) => Element.SendCompleted();

    void UpdateFont()
    {
        if (Control == null)
        {
            return;
        }

        var editor = Element;

        bool editorIsDefault = editor?.FontFamily == null && editor?.FontSize == Device.GetNamedSize(NamedSize.Default, typeof(Editor), true) && editor?.FontAttributes == FontAttributes.None;
        if (editor == null || (editorIsDefault && !fontApplied))
        {
            return;
        }

        if (editorIsDefault)
        {
            Control.ClearValue(TemplatedControl.FontStyleProperty);
            Control.ClearValue(TemplatedControl.FontSizeProperty);
            Control.ClearValue(TemplatedControl.FontFamilyProperty);
            Control.ClearValue(TemplatedControl.FontWeightProperty);
        }
        else
        {
            Control.ApplyFont(editor);
        }

        fontApplied = true;
    }

    void UpdateInputScope() => Control.InputScope = Element.Keyboard.ToInputScope();

    void UpdateText()
    {
        string newText = Element?.Text ?? "";

        if (Control.Text == newText)
        {
            return;
        }

        Control.Text = newText;
        Control.SelectionStart = Control.Text.Length;
    }

    void UpdateTextColor() => Control.UpdateDependencyColor(TemplatedControl.ForegroundProperty, Element.TextColor);

    void UpdateMaxLength()
    {
        Control.MaxLength = Element.MaxLength;

        var currentControlText = Control.Text;

        if (currentControlText.Length > Element.MaxLength)
        {
            Control.Text = currentControlText.Substring(0, Element.MaxLength);
        }
    }

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing)
        {
            if (Control != null)
            {
                Control.LostFocus -= NativeOnLostFocus;
                Control.TextChanged -= NativeOnTextChanged;
            }
        }

        isDisposed = true;
        base.Dispose(disposing);
    }

    void UpdateIsReadOnly() => Control.IsReadOnly = Element.IsReadOnly;
}