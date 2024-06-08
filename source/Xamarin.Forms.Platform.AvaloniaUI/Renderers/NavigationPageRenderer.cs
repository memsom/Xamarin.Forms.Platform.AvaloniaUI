using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(NavigationPage), typeof(NavigationPageRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class NavigationPageRenderer : VisualPageRenderer<NavigationPage, FormsNavigationPage>
{
    protected override void OnElementChanged(ElementChangedEventArgs<NavigationPage> e)
    {
        if (e.OldElement != null) // Clear old element event
        {
            e.OldElement.PushRequested -= Element_PushRequested;
            e.OldElement.PopRequested -= Element_PopRequested;
            e.OldElement.PopToRootRequested -= Element_PopToRootRequested;
            e.OldElement.RemovePageRequested -= Element_RemovePageRequested;
            e.OldElement.InsertPageBeforeRequested -= Element_InsertPageBeforeRequested;
        }

        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsNavigationPage(Element));
            }

            // Update control property
            UpdateBarBackgroundColor();
            UpdateBarTextColor();

            // Suscribe element event
            Element.PushRequested += Element_PushRequested;
            Element.PopRequested += Element_PopRequested;
            Element.PopToRootRequested += Element_PopToRootRequested;
            Element.RemovePageRequested += Element_RemovePageRequested;
            Element.InsertPageBeforeRequested += Element_InsertPageBeforeRequested;
        }

        base.OnElementChanged(e);
    }

    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == NavigationPage.BarBackgroundColorProperty.PropertyName)
        {
            UpdateBarBackgroundColor();
        }
        else if (e.PropertyName == NavigationPage.BarTextColorProperty.PropertyName)
        {
            UpdateBarTextColor();
        }
    }

    protected override void Appearing()
    {
        base.Appearing();
        PushExistingNavigationStack();
    }

    void Element_InsertPageBeforeRequested(object? sender, Internals.NavigationRequestedEventArgs e) => Control?.InsertPageBefore(e.Page, e.BeforePage);

    void Element_RemovePageRequested(object? sender, Internals.NavigationRequestedEventArgs e) => Control?.RemovePage(e.Page);

    void Element_PopToRootRequested(object? sender, Internals.NavigationRequestedEventArgs e) => Control?.PopToRoot(e.Animated);

    void Element_PopRequested(object? sender, Internals.NavigationRequestedEventArgs e) => Control?.Pop(e.Animated);

    void Element_PushRequested(object? sender, Internals.NavigationRequestedEventArgs e) => Control?.Push(e.Page, e.Animated);

    void PushExistingNavigationStack()
    {
        foreach (var page in Element.Pages)
        {
            Control.InternalChildren.Add(page);
        }

        Control.CurrentPage = Control.InternalChildren.Last();
    }

    void UpdateBarBackgroundColor() => Control.UpdateDependencyColor(AvaloniaDynamicContentPage.TitleBarBackgroundColorProperty, Element.BarBackgroundColor);

    void UpdateBarTextColor() => Control.UpdateDependencyColor(AvaloniaDynamicContentPage.TitleBarTextColorProperty, Element.BarTextColor);

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
        {
            return;
        }

        if (disposing && Element != null)
        {
            Element.PushRequested -= Element_PushRequested;
            Element.PopRequested -= Element_PopRequested;
            Element.PopToRootRequested -= Element_PopToRootRequested;
            Element.RemovePageRequested -= Element_RemovePageRequested;
            Element.InsertPageBeforeRequested -= Element_InsertPageBeforeRequested;
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}