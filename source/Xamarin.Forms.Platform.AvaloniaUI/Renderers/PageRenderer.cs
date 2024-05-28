using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(Page), typeof(PageRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class PageRenderer : VisualPageRenderer<Page, FormsContentPage>
{
    VisualElement? currentView;

    protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and subscribe control event
            {
                SetNativeControl(new FormsContentPage());
            }

            // Update control property
            UpdateContent();
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == ContentPage.ContentProperty.PropertyName)
        {
            UpdateContent();
        }
    }

    void UpdateContent()
    {
        try
        {
            if (Element is ContentPage page)
            {
                if (currentView != null)
                {
                    currentView.Cleanup(); // cleanup old view
                }

                currentView = page.Content;
                Control.Content = currentView != null ? Platform.GetOrCreateRenderer(currentView).GetNativeElement() : null;
            }
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}