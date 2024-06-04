using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(CollectionView), typeof(CollectionViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class CollectionViewRenderer : ViewRenderer<CollectionView, FormsCollectionView>
{
    protected override void OnElementChanged(ElementChangedEventArgs<CollectionView> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsCollectionView() { ContentLoader = new FormsContentLoader() });
            }

            // TODO:
            UpdateItemSource();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        // TODO:
    }

    void UpdateItemSource() => Control.ItemsSource = Element.ItemsSource; // this is a guess
}