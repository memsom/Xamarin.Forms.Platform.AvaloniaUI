using System.ComponentModel;
using Avalonia.Layout;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(Image), typeof(ImageRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ImageRenderer : ViewRenderer<Image, global::Avalonia.Controls.Image>
{
    protected override async void OnElementChanged(ElementChangedEventArgs<Image> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new global::Avalonia.Controls.Image());
            }

            // Update control property
            await TryUpdateSource();
            UpdateAspect();
        }

        base.OnElementChanged(e);
    }

    protected override async void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Image.SourceProperty.PropertyName)
            await TryUpdateSource();
        else if (e.PropertyName == Image.AspectProperty.PropertyName)
            UpdateAspect();
    }

    void UpdateAspect()
    {
        Control.Stretch = Element.Aspect.ToStretch();
        if (Element.Aspect == Aspect.AspectFill) // Then Center Crop
        {
            Control.HorizontalAlignment = HorizontalAlignment.Center;
            Control.VerticalAlignment = VerticalAlignment.Center;
        }
        else // Default
        {
            Control.HorizontalAlignment = HorizontalAlignment.Left;
            Control.VerticalAlignment = VerticalAlignment.Top;
        }
    }

    protected virtual async Task TryUpdateSource()
    {
        try
        {
            await UpdateSource().ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            Log.Warning(nameof(ImageRenderer), "Error loading image: {0}", ex);
        }
        finally
        {
            Element.SetIsLoading(false);
        }
    }

    protected async Task UpdateSource()
    {
        if (Element == null || Control == null)
        {
            return;
        }

        var source = Element.Source;

        Element.SetIsLoading(true);
        try
        {
            var imagesource = await source.ToNativeImageSourceAsync();

            // only set if we are still on the same image
            if (Control != null && Element.Source == source)
            {
                Control.Source = imagesource;
            }
        }
        finally
        {
            // only mark as finished if we are still on the same image
            if (Element.Source == source)
            {
                Element.SetIsLoading(false);
            }
        }

        ((IVisualElementController)Element)?.InvalidateMeasure(InvalidationTrigger.RendererReady);
    }
}