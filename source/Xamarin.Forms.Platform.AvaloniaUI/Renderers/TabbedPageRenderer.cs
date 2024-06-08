using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(TabbedPage), typeof(TabbedPageRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class TabbedPageRenderer : VisualMultiPageRenderer<TabbedPage, Page, FormsTabbedPage>
{
    protected override void OnElementChanged(ElementChangedEventArgs<TabbedPage> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsTabbedPage
                {
                    ContentLoader = new FormsContentLoader()
                });
            }

            UpdateBarBackgroundColor();
            UpdateBarTextColor();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == TabbedPage.BarBackgroundColorProperty.PropertyName)
        {
            UpdateBarBackgroundColor();
        }
        else if (e.PropertyName == TabbedPage.BarTextColorProperty.PropertyName)
        {
            UpdateBarTextColor();
        }
    }

    void UpdateBarBackgroundColor() => Control.UpdateDependencyColor(FormsTabbedPage.BarBackgroundColorProperty, Element.BarBackgroundColor);

    void UpdateBarTextColor() => Control.UpdateDependencyColor(FormsTabbedPage.BarTextColorProperty, Element.BarTextColor);
}