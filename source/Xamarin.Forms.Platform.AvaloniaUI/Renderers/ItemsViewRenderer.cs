using System.Collections.Specialized;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public abstract class ItemsViewRenderer<TItemsView, TNativeElement> : ViewRenderer<TItemsView, TNativeElement>
    where TItemsView : ItemsView where TNativeElement : FormsMultiView
{
    protected override void OnElementChanged(ElementChangedEventArgs<TItemsView> e)
    {
        if (e.OldElement != null) // Clear old element event
        {
            ((INotifyCollectionChanged)e.OldElement.ItemsSource).CollectionChanged -= OnPagesChanged;
        }

        if (e.NewElement != null)
        {
            // Subscribe control event
            Control.SelectionChanged += Control_SelectionChanged;

            // Subscribe element event
            ((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged += OnPagesChanged;
        }

        base.OnElementChanged(e);
    }

    //protected override void Appearing()
    //{
    //    base.Appearing();

    //    OnPagesChanged(Element.ItemsSource, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    //}

    void OnPagesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (Element.ItemsSource != null)
        {
            Control.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<object>(Element.ItemsSource.Cast<object>());
        }
        else
        {
            Control.ItemsSource = new System.Collections.ObjectModel.ObservableCollection<object>();
        }
    }

    private void Control_SelectionChanged(object? sender, AvaloniaSelectionChangedEventArgs e)
    {
    }

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed)
            return;

        if (disposing)
        {
            if (Control != null)
            {
                Control.SelectionChanged -= Control_SelectionChanged;
            }

            if (Element != null)
            {
                ((INotifyCollectionChanged)Element.ItemsSource).CollectionChanged -= OnPagesChanged;
            }
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}