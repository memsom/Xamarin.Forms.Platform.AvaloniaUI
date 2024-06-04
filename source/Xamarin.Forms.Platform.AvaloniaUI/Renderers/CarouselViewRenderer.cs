using System.ComponentModel;
using Avalonia.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(CarouselView), typeof(CarouselViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class CarouselViewRenderer : ItemsViewRenderer<CarouselView, FormsCarouselView>
{
    protected override void OnElementChanged(ElementChangedEventArgs<CarouselView> e)
    {
        if (e.NewElement != null && Control == null)
        {
            SetNativeControl(new FormsCarouselView() { ContentLoader = new FormsContentLoader() });
            // TODO:
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        // TODO:
    }
}