using System.ComponentModel;
using Avalonia.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportRenderer(typeof(Layout), typeof(LayoutRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class LayoutRenderer : ViewRenderer<Layout, FormsPanel>
{
    IElementController? ElementController => Element;
    bool isZChanged;

    protected override void OnElementChanged(ElementChangedEventArgs<Layout> e)
    {
        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                SetNativeControl(new FormsPanel(Element));
            }

            // Update control property
            UpdateClipToBounds();
            foreach (Element child in ElementController.LogicalChildren)
            {
                HandleChildAdded(Element, new ElementEventArgs(child));
            }

            // Suscribe element event
            Element.ChildAdded += HandleChildAdded;
            Element.ChildRemoved += HandleChildRemoved;
            Element.ChildrenReordered += HandleChildrenReordered;
        }

        base.OnElementChanged(e);
    }

    void HandleChildAdded(object? sender, ElementEventArgs e)
    {
        UiHelper.ExecuteInUiThread(() =>
        {
            var view = e.Element as VisualElement;

            if (view == null || Control == null)
            {
                return;
            }

            if (Platform.CreateRenderer(view) is { } renderer)
            {
                Platform.SetRenderer(view, renderer);
                if (renderer.GetNativeElement() is { } nativeElement)
                {
                    Control.Children.Add(nativeElement);
                }
                if (isZChanged)
                {
                    EnsureZIndex();
                }
            }
        });
    }

    void HandleChildRemoved(object? sender, ElementEventArgs e)
    {
        UiHelper.ExecuteInUiThread(() =>
        {
            var view = e.Element as VisualElement;

            if (view == null)
                return;

            Control native = Platform.GetRenderer(view)?.GetNativeElement();
            if (native != null)
            {
                Control.Children.Remove(native);
                view.Cleanup();
                if (isZChanged)
                {
                    EnsureZIndex();
                }
            }
        });
    }

    void HandleChildrenReordered(object? sender, EventArgs e)
    {
        EnsureZIndex();
    }

    void EnsureZIndex()
    {
        if (ElementController.LogicalChildren.Count == 0)
        {
            return;
        }

        for (var z = 0; z < ElementController.LogicalChildren.Count; z++)
        {
            var child = ElementController.LogicalChildren[z] as VisualElement;
            if (child == null)
            {
                continue;
            }

            IVisualElementRenderer childRenderer = Platform.GetRenderer(child);

            if (childRenderer == null)
            {
                continue;
            }

            if (CanvasExtensions.GetZIndex(childRenderer.GetNativeElement()) != (z + 1))
            {
                if (!isZChanged)
                {
                    isZChanged = true;
                }

                CanvasExtensions.SetZIndex(childRenderer.GetNativeElement(), z + 1);
            }
        }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        base.OnElementPropertyChanged(sender, e);

        if (e.PropertyName == Layout.IsClippedToBoundsProperty.PropertyName)
        {
            UpdateClipToBounds();
        }
    }

    protected override void UpdateBackground()
    {
        Control.UpdateDependencyColor(FormsPanel.BackgroundProperty, Element.BackgroundColor);
    }

    void UpdateClipToBounds()
    {
        Control.ClipToBounds = Element.IsClippedToBounds;
    }

    bool _isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (_isDisposed)
            return;

        if (disposing)
        {
            if (Element != null)
            {
                Element.ChildAdded -= HandleChildAdded;
                Element.ChildRemoved -= HandleChildRemoved;
                Element.ChildrenReordered -= HandleChildrenReordered;
            }
        }

        _isDisposed = true;
        base.Dispose(disposing);
    }
}