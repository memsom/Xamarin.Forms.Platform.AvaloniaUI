using System.ComponentModel;
using Avalonia.Controls;
using Avalonia.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(BoxView), typeof(BoxViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class BoxViewRenderer : ViewRenderer<BoxView, FormsRectangle>
{
    Border? border;

    protected override void OnElementChanged(ElementChangedEventArgs<BoxView> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // Construct and SetNativeControl and suscribe control event
            {
                var rectangle = new FormsRectangle();

                border = new Border();

                VisualBrush visualBrush = new VisualBrush
                {
                    Visual = border
                };

                rectangle.Fill = visualBrush;

                SetNativeControl(rectangle);
            }

            UpdateBackground();
            UpdateCornerRadius();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == BoxView.ColorProperty.PropertyName)
        {
            UpdateBackground();
        }
        else if (e.PropertyName == BoxView.CornerRadiusProperty.PropertyName)
        {
            UpdateCornerRadius();
        }
    }

    protected override void UpdateNativeWidget()
    {
        base.UpdateNativeWidget();

        UpdateSize();
    }

    protected override void UpdateBackground()
    {
        Color color = Element.Color != Color.Default ? Element.Color : Element.BackgroundColor;
        border.UpdateDependencyColor(Border.BackgroundProperty, color);
        Control.InvalidateMeasure();
    }

    void UpdateCornerRadius()
    {
        var cornerRadius = Element.CornerRadius;
        border.CornerRadius = new Avalonia.CornerRadius(cornerRadius.TopLeft, cornerRadius.TopRight, cornerRadius.BottomRight, cornerRadius.BottomLeft);
    }

    void UpdateSize()
    {
        border.Height = Element.Height > 0 ? Element.Height : Double.NaN;
        border.Width = Element.Width > 0 ? Element.Width : Double.NaN;
    }
}