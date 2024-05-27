using System.ComponentModel;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;

[assembly: ExportRenderer(typeof(Page), typeof(PageRenderer))]
namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class PageRenderer : VisualPageRenderer<Page, FormsContentPage>
{
    VisualElement _currentView;

    protected override void OnElementChanged(ElementChangedEventArgs<Page> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
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

        if(e.PropertyName == ContentPage.ContentProperty.PropertyName)
            UpdateContent();
    }

    void UpdateContent()
    {
        try
        {
            ContentPage page = Element as ContentPage;
            if (page != null)
            {
                if (_currentView != null)
                {
                    _currentView.Cleanup(); // cleanup old view
                }

                _currentView = page.Content;
                Control.Content = _currentView != null ? Platform.GetOrCreateRenderer(_currentView).GetNativeElement() : null;
            }
        }
        catch(Exception ex)
        {
            Debug.WriteLine(ex);
        }
    }
}