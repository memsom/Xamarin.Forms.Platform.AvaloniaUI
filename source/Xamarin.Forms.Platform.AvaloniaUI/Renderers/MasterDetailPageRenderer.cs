using System.ComponentModel;
using Avalonia;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(MasterDetailPage), typeof(MasterDetailPageRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class MasterDetailPageRenderer : VisualPageRenderer<MasterDetailPage, FormsMasterDetailPage>
{
    protected override void OnElementChanged(ElementChangedEventArgs<MasterDetailPage> e)
    {
        if (e.NewElement != null && Control == null)
        { // construct and SetNativeControl and suscribe control event
            SetNativeControl(new FormsMasterDetailPage() { ContentLoader = new FormsContentLoader() });

            Control.PropertyChanged += Control_PropertyChanged;
        }

        base.OnElementChanged(e);
    }

    private void Control_PropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if(e.Property == AvaloniaMasterDetailPage.IsPresentedProperty)
        {
            OnIsPresentedChanged(e);
        }
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == FlyoutPage.IsPresentedProperty.PropertyName) // || e.PropertyName == MasterDetailPage.MasterBehaviorProperty.PropertyName)
        {
            UpdateIsPresented();
        }
        else if (e.PropertyName == "Master")
        {
            UpdateMasterPage();
        }
        else if (e.PropertyName == "Detail")
        {
            UpdateDetailPage();
        }
    }

    protected override void Appearing()
    {
        base.Appearing();
        UpdateIsPresented();
        UpdateMasterPage();
        UpdateDetailPage();
    }

    void UpdateIsPresented()
    {
        Control.IsPresented = Element.IsPresented;
    }

    void UpdateMasterPage()
    {
        Control.MasterPage = Element.Master;
    }

    void UpdateDetailPage()
    {
        Control.DetailPage = Element.Detail;
    }

    private void OnIsPresentedChanged(AvaloniaPropertyChangedEventArgs e)
    {
        ((IElementController)Element).SetValueFromRenderer(FlyoutPage.IsPresentedProperty, Control.IsPresented);
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
                Control.PropertyChanged -= Control_PropertyChanged;
            }
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}