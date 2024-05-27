using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using AvaloniaRect = Avalonia.Rect;
using AvaloniaSolidColorBrush = Avalonia.Media.SolidColorBrush;

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class VisualElementRenderer<TElement, TNativeElement> : Panel, IVisualNativeElementRenderer, IDisposable, IEffectControlProvider
    where TElement : VisualElement
    where TNativeElement : Control
{
    bool _disposed;
    EventHandler<VisualElementChangedEventArgs> _elementChangedHandlers;
    event EventHandler<PropertyChangedEventArgs> _elementPropertyChanged;
    event EventHandler _controlChanging;
    event EventHandler _controlChanged;
    VisualElementTracker<TElement, TNativeElement> _tracker;
    Control _containingPage; // Cache of containing page used for unfocusing
    Control _control => Control;

    Canvas _backgroundLayer;

    public TNativeElement Control { get; private set; }

    public TElement Element { get; private set; }

    protected bool AutoPackage { get; set; } = true;

    protected bool AutoTrack { get; set; } = true;

    protected bool ArrangeNativeChildren { get; set; }

    protected virtual bool PreventGestureBubbling { get; set; } = false;

    IElementController ElementController => Element;

    protected VisualElementTracker<TElement, TNativeElement> Tracker
    {
        get { return _tracker; }
        set
        {
            if (_tracker == value) return;

            if (_tracker != null)
            {
                _tracker.Dispose();
                _tracker.Updated -= OnTrackerUpdated;
            }

            _tracker = value;

            if (_tracker != null)
            {
                _tracker.Updated += OnTrackerUpdated;
                UpdateTracker();
            }
        }
    }

    VisualElementPackager Packager { get; set; }

    void IEffectControlProvider.RegisterEffect(Effect effect)
    {
        var platformEffect = effect as PlatformEffect;
        if (platformEffect != null) OnRegisterEffect(platformEffect);
    }

    public Control ContainerElement
    {
        get { return this; }
    }

    VisualElement IVisualElementRenderer.Element
    {
        get { return Element; }
    }

    event EventHandler<VisualElementChangedEventArgs> IVisualElementRenderer.ElementChanged
    {
        add
        {
            if (_elementChangedHandlers == null)
                _elementChangedHandlers = value;
            else
                _elementChangedHandlers = (EventHandler<VisualElementChangedEventArgs>)Delegate.Combine(_elementChangedHandlers, value);
        }

        remove { _elementChangedHandlers = (EventHandler<VisualElementChangedEventArgs>)Delegate.Remove(_elementChangedHandlers, value); }
    }

    public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        if (Children.Count == 0 || Control == null) return new SizeRequest();

        var constraint = new Avalonia.Size(widthConstraint, heightConstraint);
        TNativeElement child = Control;

        child.Measure(constraint);
        var result = new Size(Math.Ceiling(child.DesiredSize.Width), Math.Ceiling(child.DesiredSize.Height));

        return new SizeRequest(result);
    }

    public Control GetNativeElement() { return Control; }

    public void SetElement(VisualElement element)
    {
        TElement oldElement = Element;
        Element = (TElement)element;

        if (oldElement != null)
        {
            oldElement.PropertyChanged -= OnElementPropertyChanged;
        }

        if (element != null)
        {
            Element.PropertyChanged += OnElementPropertyChanged;

            if (AutoPackage && Packager == null) Packager = new VisualElementPackager(this);

            if (AutoTrack && Tracker == null)
            {
                Tracker = new VisualElementTracker<TElement, TNativeElement>();
            }

            // Disabled until reason for crashes with unhandled exceptions is discovered
            // Without this some layouts may end up with improper sizes, however their children
            // will position correctly
            //Loaded += (sender, args) => {
            if (Packager != null) Packager.Load();
            //};
        }

        OnElementChanged(new ElementChangedEventArgs<TElement>(oldElement, Element));

        var controller = (IElementController)oldElement;
        if (controller != null && controller.EffectControlProvider == this)
        {
            controller.EffectControlProvider = null;
        }

        controller = element;
        if (controller != null) controller.EffectControlProvider = this;
    }

    public event EventHandler<ElementChangedEventArgs<TElement>> ElementChanged;

    event EventHandler<PropertyChangedEventArgs> IVisualNativeElementRenderer.ElementPropertyChanged
    {
        add => _elementPropertyChanged += value;
        remove => _elementPropertyChanged -= value;
    }

    event EventHandler IVisualNativeElementRenderer.ControlChanging
    {
        add { _controlChanging += value; }
        remove { _controlChanging -= value; }
    }

    event EventHandler IVisualNativeElementRenderer.ControlChanged
    {
        add { _controlChanged += value; }
        remove { _controlChanged -= value; }
    }

    protected override Avalonia.Size ArrangeOverride(Avalonia.Size finalSize)
    {
        if (Element == null || finalSize.Width * finalSize.Height == 0) return finalSize;

        Element.IsInNativeLayout = true;

        var myRect = new AvaloniaRect(0, 0, finalSize.Width, finalSize.Height);

        if (Control != null)
        {
            Control.Arrange(myRect);
        }

        List<Control> arrangedChildren = null;
        for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
        {
            var child = ElementController.LogicalChildren[i] as VisualElement;
            if (child == null) continue;
            IVisualElementRenderer renderer = Platform.GetRenderer(child);
            if (renderer == null) continue;
            Rectangle bounds = child.Bounds;

            renderer.ContainerElement.Arrange(new AvaloniaRect(bounds.X, bounds.Y, Math.Max(0, bounds.Width), Math.Max(0, bounds.Height)));

            if (ArrangeNativeChildren)
            {
                if (arrangedChildren == null) arrangedChildren = new List<Control>();
                arrangedChildren.Add(renderer.ContainerElement);
            }
        }

        if (ArrangeNativeChildren)
        {
            // in the event that a custom renderer has added native controls,
            // we need to be sure to arrange them so that they are laid out.
            var nativeChildren = Children;
            for (int i = 0; i < nativeChildren.Count; i++)
            {
                var nativeChild = nativeChildren[i];
                if (arrangedChildren?.Contains(nativeChild) == true)
                    // don't try to rearrange renderers that were just arranged,
                    // lest you suffer a layout cycle
                    continue;
                nativeChild.Arrange(myRect);
            }
        }

        _backgroundLayer?.Arrange(myRect);

        Element.IsInNativeLayout = false;

        return finalSize;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || _disposed) return;

        _disposed = true;

        Tracker?.Dispose();
        Tracker = null;

        Packager?.Dispose();
        Packager = null;

        SetNativeControl(null);
        SetElement(null);
    }

    protected override Avalonia.Size MeasureOverride(Avalonia.Size availableSize)
    {
        if (Element == null || availableSize.Width * availableSize.Height == 0) return new Avalonia.Size(0, 0);

        Element.IsInNativeLayout = true;

        for (var i = 0; i < ElementController.LogicalChildren.Count; i++)
        {
            var child = ElementController.LogicalChildren[i] as VisualElement;
            if (child == null) continue;
            IVisualElementRenderer renderer = Platform.GetRenderer(child);
            if (renderer == null) continue;

            renderer.ContainerElement.Measure(availableSize);
        }

        double width = Math.Max(0, Element.Width);
        double height = Math.Max(0, Element.Height);
        var result = new Avalonia.Size(width, height);
        if (Control != null)
        {
            double w = Element.Width;
            double h = Element.Height;
            if (w == -1)
            {
                w = availableSize.Width;
            }

            if (h == -1)
            {
                h = availableSize.Height;
            }

            w = Math.Max(0, w);
            h = Math.Max(0, h);
            Control.Measure(new Avalonia.Size(w, h));
        }

        Element.IsInNativeLayout = false;

        return result;
    }

    protected virtual void OnElementChanged(ElementChangedEventArgs<TElement> e)
    {
        var args = new VisualElementChangedEventArgs(e.OldElement, e.NewElement);
        if (_elementChangedHandlers != null) _elementChangedHandlers(this, args);

        EventHandler<ElementChangedEventArgs<TElement>> changed = ElementChanged;
        if (changed != null) changed(this, e);
    }

    protected void UpdateTabStop() { }

    protected void UpdateTabIndex() { }

    protected virtual void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            UpdateEnabled();
        else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            UpdateBackgroundColor();
        else if (e.PropertyName == VisualElement.InputTransparentProperty.PropertyName ||
                 e.PropertyName == Layout.CascadeInputTransparentProperty.PropertyName)
            UpdateInputTransparent();
        else if (e.PropertyName == VisualElement.IsTabStopProperty.PropertyName)
            UpdateTabStop();
        else if (e.PropertyName == VisualElement.TabIndexProperty.PropertyName) UpdateTabIndex();

        _elementPropertyChanged?.Invoke(this, e);
    }

    protected virtual void OnRegisterEffect(PlatformEffect effect)
    {
        effect.SetContainer(this);
        effect.SetControl(Control);
    }

    protected void SetNativeControl(TNativeElement control)
    {
        _controlChanging?.Invoke(this, EventArgs.Empty);
        TNativeElement oldControl = Control;
        Control = control;

        if (oldControl != null)
        {
            Children.Remove(oldControl);

            // TODO:
            //Control.AttachedToVisualTree -= Control_AttachedToVisualTree;
            //Control.DetachedFromVisualTree -= Control_DetachedFromVisualTree;
        }

        UpdateTracker();

        if (control == null)
        {
            _controlChanged?.Invoke(this, EventArgs.Empty);
            return;
        }

        Control.HorizontalAlignment = HorizontalAlignment.Stretch;
        Control.VerticalAlignment = VerticalAlignment.Stretch;

        if (Element == null)
            throw new InvalidOperationException(
                "Cannot assign a native control without an Element; Renderer unbound and/or disposed. " +
                "Please consult Xamarin.Forms renderers for reference implementation of OnElementChanged.");

        Element.IsNativeStateConsistent = false;

        Control.AttachedToVisualTree += OnAttachedToVisualTree;
        Control.DetachedFromVisualTree += OnDetachedFromVisualTree;

        Children.Add(control);
        UpdateBackgroundColor();

        _controlChanged?.Invoke(this, EventArgs.Empty);
    }


    private void OnAttachedToVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        Control.AttachedToVisualTree -= OnAttachedToVisualTree;
        Control_Loaded(sender, new RoutedEventArgs());
    }

    private void Control_Loaded(object sender, RoutedEventArgs e)
    {
        OnControlLoaded(sender, e);
        Appearing();
    }

    private void OnDetachedFromVisualTree(object sender, VisualTreeAttachmentEventArgs e)
    {
        Control.DetachedFromVisualTree -= OnDetachedFromVisualTree;
        Control_Unloaded(sender, new RoutedEventArgs());
    }

    private void Control_Unloaded(object sender, RoutedEventArgs e) { Disappearing(); }

    protected virtual void Appearing() { }

    protected virtual void Disappearing() { }

    protected virtual void UpdateBackgroundColor()
    {
        Color backgroundColor = Element.BackgroundColor;

        var backgroundLayer = (Panel)this;
        if (_backgroundLayer != null)
        {
            backgroundLayer = _backgroundLayer;
            Background = null; // Make the container effectively hit test invisible
        }

        if (_control != null)
        {
            //if (!backgroundColor.IsDefault)
            //{
            //	_control.Background = backgroundColor.ToBrush();
            //}
            //else
            //{
            //	_control.ClearValue(Control.BackgroundProperty);
            //}
        }
        else
        {
            if (!backgroundColor.IsDefault)
            {
                backgroundLayer.Background = backgroundColor.ToBrush();
            }
            else
            {
                backgroundLayer.ClearValue(BackgroundProperty);
            }
        }
    }

    protected virtual void UpdateNativeControl()
    {
        UpdateEnabled();
        UpdateInputTransparent();
        UpdateTabStop();
        UpdateTabIndex();
    }

    void OnControlLoaded(object sender, RoutedEventArgs args) { Element.IsNativeStateConsistent = true; }

    void OnTrackerUpdated(object sender, EventArgs e) { UpdateNativeControl(); }

    void UpdateEnabled()
    {
        if (_control != null)
            _control.IsEnabled = Element.IsEnabled;
        else
            IsHitTestVisible = Element.IsEnabled && !Element.InputTransparent;
    }

    void UpdateInputTransparent()
    {
        if (NeedsBackgroundLayer(Element))
        {
            IsHitTestVisible = true;
            AddBackgroundLayer();
        }
        else
        {
            RemoveBackgroundLayer();
            IsHitTestVisible = Element.IsEnabled && !Element.InputTransparent;

            if (!IsHitTestVisible)
            {
                return;
            }

            // If this Panel's background brush is null, the UWP considers it transparent to hit testing (even
            // when IsHitTestVisible is true). So we have to explicitly set a background brush to make it show up
            // in hit testing.
            if (Element is Layout && Background == null)
            {
                Background = new AvaloniaSolidColorBrush(Colors.Transparent);
            }
        }
    }

    void AddBackgroundLayer()
    {
        if (_backgroundLayer != null)
        {
            return;
        }

        // In UWP, once a control has hit testing disabled, all of its child controls
        // also have hit testing disabled. The exception is a Panel with its
        // Background Brush set to `null`; the Panel will be invisible to hit testing, but its
        // children will work just fine.

        // In order to handle the situation where we need the layout to be invisible to hit testing,
        // the child controls to be visible to hit testing, *and* we need to support non-null
        // background brushes, we insert another empty Panel which is invisible to hit testing; that
        // Panel will be our Background color

        _backgroundLayer = new Canvas {IsHitTestVisible = false};
        Children.Insert(0, _backgroundLayer);
        UpdateBackgroundColor();
    }

    void RemoveBackgroundLayer()
    {
        if (_backgroundLayer == null)
        {
            return;
        }

        Children.Remove(_backgroundLayer);
        _backgroundLayer = null;
        UpdateBackgroundColor();
    }

    internal static bool NeedsBackgroundLayer(VisualElement element)
    {
        if (!(element is Layout layout))
        {
            return false;
        }

        if (layout.IsEnabled && layout.InputTransparent && !layout.CascadeInputTransparent)
        {
            return true;
        }

        return false;
    }


    void UpdateTracker()
    {
        if (_tracker == null) return;

        _tracker.Control = Control;
        _tracker.Element = Element;
        _tracker.Container = ContainerElement;
    }

    Control IVisualElementRenderer.GetNativeElement() { return Control; }
}