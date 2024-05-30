using System.ComponentModel;
using Avalonia.Animation;
using Avalonia.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(ScrollView), typeof(ScrollViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ScrollViewRenderer : ViewRenderer<ScrollView, FormsScrollViewer>
{
    VisualElement? currentView;
    Animatable? animatable;

    protected IScrollViewController Controller => Element;

    protected override void OnElementChanged(ElementChangedEventArgs<ScrollView> e)
    {
        if (e.OldElement != null) // Clear old element event
        {
            ((IScrollViewController)e.OldElement).ScrollToRequested -= OnScrollToRequested;
        }

        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsScrollViewer()
                {
                    HorizontalScrollBarVisibility = e.NewElement.HorizontalScrollBarVisibility.ToNativeScrollBarVisibility(),
                    VerticalScrollBarVisibility = e.NewElement.VerticalScrollBarVisibility.ToNativeScrollBarVisibility()
                });
                Control.LayoutUpdated += NativeLayoutUpdated;
            }

            // Update control property
            UpdateOrientation();
            LoadContent();

            // Suscribe element event
            Controller.ScrollToRequested += OnScrollToRequested;
        }

        base.OnElementChanged(e);
    }


    protected override void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == "Content")
        {
            LoadContent();
        }
        else if (e.PropertyName == Layout.PaddingProperty.PropertyName)
        {
            UpdateMargins();
        }
        else if (e.PropertyName == ScrollView.OrientationProperty.PropertyName)
        {
            UpdateOrientation();
        }
        else if (e.PropertyName == ScrollView.VerticalScrollBarVisibilityProperty.PropertyName)
        {
            UpdateVerticalScrollBarVisibility();
        }
        else if (e.PropertyName == ScrollView.HorizontalScrollBarVisibilityProperty.PropertyName)
        {
            UpdateHorizontalScrollBarVisibility();
        }
    }

    void NativeLayoutUpdated(object? sender, EventArgs e) => UpdateScrollPosition();

    static double GetDistance(double start, double position, double v) => start + (position - start) * v;

    void LoadContent()
    {
        if (currentView != null)
        {
            currentView.Cleanup(); // cleanup old view
        }

        currentView = Element.Content;

        if (currentView != null)
        {
            /*
             * Wrap Content in a DockPanel : The goal is to reduce ce Measure Cycle on scolling
             */
            DockPanel dockPanel = new DockPanel();
            dockPanel.Children.Add(Platform.GetOrCreateRenderer(currentView).GetNativeElement());
            Control.Content = dockPanel;
        }
        else
        {
            Control.Content = null;
        }

        UpdateMargins();
    }

    void OnScrollToRequested(object? sender, ScrollToRequestedEventArgs e)
    {
        if (animatable == null && e.ShouldAnimate)
        {
            animatable = new Animatable();
        }

        ScrollToPosition position = e.Position;
        double x = e.ScrollX;
        double y = e.ScrollY;

        if (e.Mode == ScrollToMode.Element)
        {
            Point itemPosition = Controller.GetScrollPositionForElement(e.Element as VisualElement, e.Position);
            x = itemPosition.X;
            y = itemPosition.Y;
        }

        if (Control.Offset.Y == y && Control.Offset.X == x)
        {
            return;
        }

        if (e.ShouldAnimate)
        {
            //var animation = new Animation(v => { UpdateScrollOffset(GetDistance(Control.ViewportWidth, x, v), GetDistance(Control.ViewportHeight, y, v)); });

            //animation.Commit(_animatable, "ScrollTo", length: 500, easing: Easing.CubicInOut, finished: (v, d) =>
            //{
            //	UpdateScrollOffset(x, y);
            //	Controller.SendScrollFinished();
            //});
        }
        else
        {
            UpdateScrollOffset(x, y);
            Controller.SendScrollFinished();
        }
    }

    void UpdateMargins()
    {
        if (Control.Content is Control element)
        {
            switch (Element.Orientation)
            {
                case ScrollOrientation.Horizontal:
                    // need to add left/right margins
                    element.Margin = new global::Avalonia.Thickness(Element.Padding.Left, 0, 10, 0);
                    break;
                case ScrollOrientation.Vertical:
                    // need to add top/bottom margins
                    element.Margin = new global::Avalonia.Thickness(0, Element.Padding.Top, 0, Element.Padding.Bottom);
                    break;
                case ScrollOrientation.Both:
                    // need to add all margins
                    element.Margin = new global::Avalonia.Thickness(Element.Padding.Left, Element.Padding.Top, Element.Padding.Right, Element.Padding.Bottom);
                    break;
            }
        }
    }

    void UpdateOrientation()
    {
        var orientation = Element.Orientation;
        if (orientation == ScrollOrientation.Horizontal || orientation == ScrollOrientation.Both)
        {
            Control.HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
        }
        else
        {
            Control.HorizontalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled;
        }

        if (orientation == ScrollOrientation.Vertical || orientation == ScrollOrientation.Both)
        {
            Control.VerticalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Auto;
        }
        else
        {
            Control.VerticalScrollBarVisibility = global::Avalonia.Controls.Primitives.ScrollBarVisibility.Disabled;
        }
    }

    void UpdateScrollOffset(double x, double y)
    {
        if (Element.Orientation == ScrollOrientation.Horizontal)
        {
            Control.Offset = Control.Offset.WithX(x);
        }
        else
        {
            Control.Offset = Control.Offset.WithY(y);
        }
    }

    void UpdateScrollPosition()
    {
        if (Element != null)
        {
            Controller.SetScrolledPosition(Control.Offset.X, Control.Offset.Y);
        }
    }

    void UpdateVerticalScrollBarVisibility() { Control.VerticalScrollBarVisibility = Element.VerticalScrollBarVisibility.ToNativeScrollBarVisibility(); }

    void UpdateHorizontalScrollBarVisibility()
    {
        var orientation = Element.Orientation;
        if (orientation == ScrollOrientation.Horizontal || orientation == ScrollOrientation.Both)
        {
            Control.HorizontalScrollBarVisibility = Element.HorizontalScrollBarVisibility.ToNativeScrollBarVisibility();
        }
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
                Control.LayoutUpdated -= NativeLayoutUpdated;
            }

            if (Element != null)
            {
                Controller.ScrollToRequested -= OnScrollToRequested;
            }
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}