using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(CarouselPage), typeof(CarouselPageRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class CarouselPageRenderer : VisualMultiPageRenderer<CarouselPage, ContentPage, FormsCarouselPage>
{
    protected override void OnElementChanged(ElementChangedEventArgs<CarouselPage> e)
    {
        if (e.NewElement != null && Control == null)
        { // construct and SetNativeControl and suscribe control event
            SetNativeControl(new FormsCarouselPage() {ContentLoader = new FormsContentLoader()});
        }

        base.OnElementChanged(e);
    }
}