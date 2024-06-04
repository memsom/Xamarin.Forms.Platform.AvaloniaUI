using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(SwipeView), typeof(SwipeViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class SwipeViewRenderer : ViewRenderer<SwipeView, FormsSwipeView>
{
    protected override void OnElementChanged(ElementChangedEventArgs<SwipeView> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsSwipeView() { ContentLoader = new FormsContentLoader() });
            }

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