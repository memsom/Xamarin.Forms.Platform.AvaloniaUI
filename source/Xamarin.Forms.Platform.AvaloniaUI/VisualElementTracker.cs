using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public abstract class VisualElementTracker : IDisposable
{
    public abstract void Dispose();

    public event EventHandler Updated;

    protected void OnUpdated()
    {
        Updated?.Invoke(this, EventArgs.Empty);
    }
}

public class VisualElementTracker<TElement, TNativeElement> : VisualElementTracker where TElement : VisualElement where TNativeElement : Control
{
    bool _disposed;
    Control _container;
    TNativeElement _control;
    TElement? _element;

    bool _invalidateArrangeNeeded;
    bool _isPanning;
#pragma warning disable 0414 // The private field 'field' is assigned but its value is never used
    bool _isPinching;
#pragma warning restore 0414

    bool _touchFrameReportedEventSet;
    int _touchPoints = 1;

    public TNativeElement Control
    {
        get { return _control; }
        set
        {
            if (_control == value) return;

            if (_control != null)
            {
                _control.PointerReleased -= OnControl_PointerReleased;
                _control.PointerPressed -= OnControl_PointerPressed;
                _control.PointerCaptureLost -= OnControl_PointerCaptureLost;
                _control.PointerMoved -= OnControl_PointerMoved;
            }

            _control = value;

            if (_control != null)
            {
                _control.PointerReleased += OnControl_PointerReleased;
                _control.PointerPressed += OnControl_PointerPressed;
                _control.PointerCaptureLost += OnControl_PointerCaptureLost;
                _control.PointerMoved += OnControl_PointerMoved;
            }

            UpdateNativeControl();
        }
    }

    private void OnControl_PointerReleased(object? sender, global::Avalonia.Input.PointerReleasedEventArgs e) { MouseLeftButtonUp(sender, new MouseButtonEventArgs(e)); }

    private void OnControl_PointerPressed(object? sender, global::Avalonia.Input.PointerPressedEventArgs e) { OnManipulationDelta(sender, e); }

    private void OnControl_PointerCaptureLost(object? sender, global::Avalonia.Input.PointerCaptureLostEventArgs e) { OnManipulationCompleted(sender, e); }

    private void OnControl_PointerMoved(object? sender, PointerEventArgs e) { Touch_FrameReported(sender, e); }

    public TElement? Element
    {
        get => _element;
        set
        {
            if (_element == value) return;

            if (_element != null)
            {
                _element.BatchCommitted -= OnRedrawNeeded;
                _element.PropertyChanged -= OnPropertyChanged;
            }

            _element = value;

            if (_element != null)
            {
                _element.BatchCommitted += OnRedrawNeeded;
                _element.PropertyChanged += OnPropertyChanged;
            }

            UpdateNativeControl();
        }
    }

    protected virtual void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (Element?.Batched ?? false)
        {
            if (e.PropertyName == VisualElement.XProperty.PropertyName ||
                e.PropertyName == VisualElement.YProperty.PropertyName ||
                e.PropertyName == VisualElement.WidthProperty.PropertyName ||
                e.PropertyName == VisualElement.HeightProperty.PropertyName)
            {
                _invalidateArrangeNeeded = true;
            }

            return;
        }

        if (e.PropertyName == VisualElement.XProperty.PropertyName ||
            e.PropertyName == VisualElement.YProperty.PropertyName ||
            e.PropertyName == VisualElement.WidthProperty.PropertyName ||
            e.PropertyName == VisualElement.HeightProperty.PropertyName)
        {
            MaybeInvalidate();
        }
        else if (
            e.PropertyName == VisualElement.AnchorXProperty.PropertyName ||
            e.PropertyName == VisualElement.AnchorYProperty.PropertyName ||
            e.PropertyName == VisualElement.ScaleProperty.PropertyName ||
            e.PropertyName == VisualElement.TranslationXProperty.PropertyName ||
            e.PropertyName == VisualElement.TranslationYProperty.PropertyName ||
            e.PropertyName == VisualElement.RotationProperty.PropertyName ||
            e.PropertyName == VisualElement.RotationXProperty.PropertyName ||
            e.PropertyName == VisualElement.RotationYProperty.PropertyName)
        {
            UpdateScaleAndTranslateAndRotation();
        }
        else if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName)
        {
            UpdateVisibility(Element, Container);
        }
        else if (e.PropertyName == VisualElement.OpacityProperty.PropertyName)
        {
            UpdateOpacity(Element, Container);
        }
        else if (e.PropertyName == VisualElement.InputTransparentProperty.PropertyName)
        {
            UpdateInputTransparent(Element, Container);
        }
        else if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
        {
            UpdateInputTransparent(Element, Container);
        }
    }

    protected virtual void UpdateNativeControl()
    {
        if (Element == null || Control == null) return;

        if (_container == null)
        {
            _container = Control;
        }

        UpdateVisibility(Element, Container);
        UpdateOpacity(Element, Container);
        UpdateScaleAndTranslateAndRotation();
        UpdateInputTransparent(Element, Container);

        if (_invalidateArrangeNeeded)
        {
            MaybeInvalidate();
        }

        _invalidateArrangeNeeded = false;

        UpdateTouchFrameReportedEvent();

        OnUpdated();
    }

    private void MouseLeftButtonUp(object? sender, MouseButtonEventArgs e)
    {
        var fe = (sender as Control);
        var vr = (sender as DefaultViewRenderer)?.Element;

        if ((fe != null && !fe.IsEnabled) || (vr != null && !vr.IsEnabled)) return;

        e.Handled = ElementOnTap(e.ClickCount, e.GetPosition(fe));
    }

    bool ElementOnTap(int numberOfTapsRequired, global::Avalonia.Point tapPosition)
    {
        var view = Element as View;
        if (view == null) return false;

        var handled = false;

        var children = (view as IGestureController)?.GetChildElements(new Point(tapPosition.X, tapPosition.Y));

        if (children != null)
        {
            foreach (var recognizer in children.GetChildGesturesFor<TapGestureRecognizer>().Where(g => g.NumberOfTapsRequired == numberOfTapsRequired))
            {
                recognizer.SendTapped(view);
                handled = true;
            }
        }

        if (handled) return handled;

        foreach (var gestureRecognizer in
            view.GestureRecognizers.OfType<TapGestureRecognizer>().Where(g => g.NumberOfTapsRequired == numberOfTapsRequired))
        {
            gestureRecognizer.SendTapped(view);
            handled = true;
        }

        return handled;
    }

    void HandlePan(global::Avalonia.Input.PointerPressedEventArgs e, View view)
    {
        foreach (PanGestureRecognizer recognizer in
            view.GestureRecognizers.GetGesturesFor<PanGestureRecognizer>().Where(g => g.TouchPoints == _touchPoints))
        {
            if (!_isPanning)
            {
                ((IPanGestureController)recognizer).SendPanStarted(view, Application.Current.PanGestureId);
            }

            double totalX = 0;
            double totalY = 0;

            ((IPanGestureController)recognizer).SendPan(view, totalX, totalY, Application.Current.PanGestureId);
            _isPanning = true;
        }
    }

    void OnManipulationCompleted(object? sender, PointerCaptureLostEventArgs e)
    {
        var view = Element as View;
        if (view == null) return;

        var pinchGestures = view.GestureRecognizers.GetGesturesFor<PinchGestureRecognizer>();
        foreach (var recognizer in pinchGestures)
        {
            ((IPinchGestureController)recognizer).SendPinchEnded(view);
        }

        _isPinching = false;

        IEnumerable<PanGestureRecognizer> panGestures = view.GestureRecognizers.GetGesturesFor<PanGestureRecognizer>().Where(g => g.TouchPoints == _touchPoints);
        foreach (PanGestureRecognizer recognizer in panGestures)
        {
            ((IPanGestureController)recognizer).SendPanCompleted(view, Application.Current.PanGestureId);
        }

        Application.Current.PanGestureId++;
        _isPanning = false;
    }

    void OnManipulationDelta(object? sender, global::Avalonia.Input.PointerPressedEventArgs e)
    {
        var view = Element as View;
        if (view == null) return;

        HandlePan(e, view);
    }

    void Touch_FrameReported(object? sender, PointerEventArgs e) { _touchPoints = e.Pointer.Type == PointerType.Touch ? 1 : 0; }

    void OnRedrawNeeded(object? sender, EventArgs e) { UpdateNativeControl(); }

    void MaybeInvalidate()
    {
        if (Element is {IsInNativeLayout: true}) return;
        var parent = (Control?)Control.Parent;
        parent?.InvalidateMeasure();
        Control.InvalidateMeasure();
    }

    void UpdateScaleAndTranslateAndRotation()
    {
        // TODO : Implement plane projection - Don't exist in WPF framework :(
        if (Element != null)
        {
            double anchorX = Element.AnchorX;
            double anchorY = Element.AnchorY;
            double rotationX = Element.RotationX;
            double rotationY = Element.RotationY;
            double rotation = Element.Rotation;
            double translationX = Element.TranslationX;
            double translationY = Element.TranslationY;
            double scale = Element.Scale;

            double offsetX = scale == 0 ? 0 : translationX / scale;
            double offsetY = scale == 0 ? 0 : translationY / scale;

            Control.RenderTransformOrigin = new RelativePoint(anchorX, anchorY, RelativeUnit.Relative);
            Control.RenderTransform = new TransformGroup()
            {
                Children = new Transforms()
                {
                    new RotateTransform()
                    {
                        Angle = Element.Rotation
                    },
                    new TranslateTransform()
                    {
                        X = offsetX,
                        Y = offsetY
                    },
                    new ScaleTransform
                    {
                        ScaleX = scale,
                        ScaleY = scale
                    }
                }
            };
        }
    }

    void UpdateTouchFrameReportedEvent()
    {
        if (_touchFrameReportedEventSet) return;

        _touchFrameReportedEventSet = false;

        var view = Element as View;
        if (view == null) return;

        if (!view.GestureRecognizers.GetGesturesFor<PanGestureRecognizer>().Any(g => g.TouchPoints > 1)) return;

        _touchFrameReportedEventSet = true;
    }

    static void UpdateInputTransparent(VisualElement? view, Control frameworkElement)
    {
        if (view is Layout)
        {
            // Let VisualElementRenderer handle this
        }

        if (view != null) frameworkElement.IsHitTestVisible = view is {IsEnabled: true, InputTransparent: false};
    }

    static void UpdateOpacity(VisualElement view, Control frameworkElement) { frameworkElement.Opacity = view.Opacity; }

    static void UpdateVisibility(VisualElement view, Control frameworkElement) { frameworkElement.IsVisible = view.IsVisible; }

    public Control Container
    {
        get => _container;
        set
        {
            if (_container == value) return;

            ClearContainerEventHandlers();

            _container = value;

            UpdateNativeControl();
        }
    }

    void ClearContainerEventHandlers()
    {
        if (_container != null)
        {
            // TODO:
            //_container.PointerReleased -= OnControl_PointerReleased;
            //_container.PointerPressed -= OnControl_PointerPressed;
            //_container.PointerCaptureLost -= OnControl_PointerCaptureLost;
            //_container.PointerMoved -= OnControl_PointerMoved;
        }
    }

    public override void Dispose()
    {
        if (_disposed) return;
        _disposed = true;

        if (_control != null)
        {
            //_control.MouseLeftButtonUp -= MouseLeftButtonUp;
            _control.PointerReleased += OnControl_PointerReleased;
            //_control.ManipulationDelta -= OnManipulationDelta;
            //_control.ManipulationCompleted -= OnManipulationCompleted;
        }

        if (_element != null)
        {
            _element.BatchCommitted -= OnRedrawNeeded;
            _element.PropertyChanged -= OnPropertyChanged;
        }

        Element = null;
        Control = null;
    }
}