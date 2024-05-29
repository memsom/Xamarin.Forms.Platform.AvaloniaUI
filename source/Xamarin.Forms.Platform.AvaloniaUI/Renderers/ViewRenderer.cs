using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using AvaloniaSize = Avalonia.Size;

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ViewRenderer<TElement, TNativeElement> : IVisualElementRenderer, IEffectControlProvider
    where TElement : VisualElement where TNativeElement : Control
{
    readonly List<EventHandler<VisualElementChangedEventArgs>> elementChangedHandlers = new();

    VisualElementTracker? tracker;
    bool disposed;

    IElementController? ElementController => Element;

    public TNativeElement? Control { get; private set; }

    public TElement? Element { get; private set; }

    protected virtual bool AutoTrack { get; set; } = true;

    protected VisualElementTracker? Tracker
    {
        get => tracker;
        set
        {
            if (tracker == value)
                return;

            if (tracker != null)
            {
                tracker.Dispose();
                tracker.Updated -= HandleTrackerUpdated;
            }

            tracker = value;

            if (tracker != null)
            {
                tracker.Updated += HandleTrackerUpdated;
            }
        }
    }

    void IEffectControlProvider.RegisterEffect(Effect effect)
    {
        if (effect is PlatformEffect platformEffect)
        {
            OnRegisterEffect(platformEffect);
        }
    }

    VisualElement? IVisualElementRenderer.Element => Element;

    public Control? ContainerElement => Control;

    public Control? GetNativeElement() => Control;

    event EventHandler<VisualElementChangedEventArgs> IVisualElementRenderer.ElementChanged
    {
        add => elementChangedHandlers.Add(value);
        remove => elementChangedHandlers.Remove(value);
    }

    public virtual SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        if (Control == null)
        {
            return new SizeRequest();
        }

        var constraint = new AvaloniaSize(widthConstraint, heightConstraint);

        if (Element is {HeightRequest: -1})
        {
            Control.Height = double.NaN;
        }

        if (Element is {WidthRequest: -1})
        {
            Control.Width = double.NaN;
        }

        Control.Measure(constraint);

        return new SizeRequest(new Size(Math.Ceiling(Control.DesiredSize.Width), Math.Ceiling(Control.DesiredSize.Height)));
    }

    public void SetElement(VisualElement? element)
    {
        TElement? oldElement = Element;
        Element = (TElement?)element;

        if (oldElement != null)
        {
            oldElement.PropertyChanged -= OnElementPropertyChanged;
            oldElement.FocusChangeRequested -= OnModelFocusChangeRequested;
        }

        if (Element != null)
        {
            Element.PropertyChanged += OnElementPropertyChanged;
            Element.FocusChangeRequested += OnModelFocusChangeRequested;

            OnElementChanged(new ElementChangedEventArgs<TElement>(oldElement, Element));
        }

        IElementController? controller = oldElement;
        if (controller != null && controller.EffectControlProvider == this)
        {
            controller.EffectControlProvider = null;
        }

        controller = element;
        if (controller != null)
        {
            controller.EffectControlProvider = this;
        }
    }

    public event EventHandler<ElementChangedEventArgs<TElement>>? ElementChanged;

    protected virtual void OnElementChanged(ElementChangedEventArgs<TElement> e)
    {
        var args = new VisualElementChangedEventArgs(e.OldElement, e.NewElement);
        for (var i = 0; i < elementChangedHandlers.Count; i++)
        {
            elementChangedHandlers[i](this, args);
        }

        ElementChanged?.Invoke(this, e);
    }

    protected virtual void OnElementPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == VisualElement.IsEnabledProperty.PropertyName)
            UpdateEnabled();
        else if (e.PropertyName == VisualElement.HeightProperty.PropertyName)
            UpdateHeight();
        else if (e.PropertyName == VisualElement.WidthProperty.PropertyName)
            UpdateWidth();
        else if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            UpdateBackground();
        else if (e.PropertyName == View.HorizontalOptionsProperty.PropertyName || e.PropertyName == View.VerticalOptionsProperty.PropertyName)
            UpdateAlignment();
        else if (e.PropertyName == VisualElement.IsTabStopProperty.PropertyName)
            UpdateTabStop();
        else if (e.PropertyName == VisualElement.TabIndexProperty.PropertyName)
            UpdateTabIndex();
    }

    protected virtual void OnGotFocus(object? sender, RoutedEventArgs args)
    {
        if(Element is IElementController elementController)
        {
            elementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, true);
        }
    }

    protected virtual void OnLostFocus(object? sender, RoutedEventArgs args)
    {
        if (Element is IElementController elementController)
        {
            elementController.SetValueFromRenderer(VisualElement.IsFocusedPropertyKey, false);
        }
    }

    protected virtual void OnRegisterEffect(PlatformEffect effect)
    {
        if(Control is not null)
        {
            effect.SetControl(Control);
        }
    }

    protected void SetNativeControl(TNativeElement native)
    {
        Control = native;

        if (AutoTrack && Tracker == null)
        {
            Tracker = new VisualElementTracker<TElement, Control>
            {
                Element = Element,
                Control = Control,
            };
        }

        Element.IsNativeStateConsistent = false;

        Control.AttachedToVisualTree += Control_AttachedToVisualTree;
        Control.DetachedFromVisualTree += Control_DetachedFromVisualTree;

        Control.GotFocus += OnGotFocus;
        Control.LostFocus += OnLostFocus;

        UpdateBackground();
        UpdateAlignment();
        UpdateWidth();
        UpdateHeight();
    }

    private void Control_AttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        Control.AttachedToVisualTree -= Control_AttachedToVisualTree;
        Control_Loaded(sender, new RoutedEventArgs());
    }

    private void Control_Loaded(object? sender, RoutedEventArgs e)
    {
        Element.IsNativeStateConsistent = true;
        Appearing();
    }

    private void Control_DetachedFromVisualTree(object? sender, VisualTreeAttachmentEventArgs e)
    {
        Control.DetachedFromVisualTree -= Control_DetachedFromVisualTree;
        Control_Unloaded(sender, new RoutedEventArgs());
    }

    private void Control_Unloaded(object? sender, RoutedEventArgs e)
    {
        Disappearing();
    }

    protected virtual void Appearing()
    {

    }

    protected virtual void Disappearing()
    {

    }

    protected virtual void UpdateBackground()
    {
        if (Control is TemplatedControl templatedControl)
        {
            templatedControl?.UpdateDependencyColor(TemplatedControl.BackgroundProperty, Element.BackgroundColor);
        }
    }

    protected virtual void UpdateHeight()
    {
        if (Control == null || Element == null)
            return;

        Control.Height = Element.Height > 0 ? Element.Height : Element.HeightRequest;
    }

    protected virtual void UpdateWidth()
    {
        if (Control == null || Element == null)
            return;

        Control.Width = Element.Width > 0 ? Element.Width : Element.WidthRequest;
    }

    protected virtual void UpdateNativeWidget()
    {
        UpdateEnabled();
        UpdateTabStop();
        UpdateTabIndex();
    }

    protected virtual void OnModelFocusChangeRequested(object? sender, VisualElement.FocusRequestArgs args)
    {
        if (Control == null)
            return;

        if (args.Focus)
        {
            Control.Focus();
            args.Result = Control.IsFocused;
        }
        else
        {
            UnfocusControl(Control);
            args.Result = true;
        }
    }

    protected void UnfocusControl(Control? control)
    {
        if (control is not {IsEnabled: true})
            return;
        // TODO:
        //control.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
    }

    void HandleTrackerUpdated(object? sender, EventArgs e)
    {
        UpdateNativeWidget();
    }

    protected void UpdateTabStop()
    {
        //if (Control is AControl aControl)
        //{
        //    aControl.IsTabStop = Element.IsTabStop;

        //    // update TabStop of children for complex controls (like as DatePicker, TimePicker, SearchBar and Stepper)
        //    var children = FrameworkElementExtensions.GetChildren<AControl>(Control);
        //    foreach (var child in children)
        //    {
        //        child.IsTabStop = aControl.IsTabStop;
        //    }
        //}
    }

    protected void UpdateTabIndex()
    {
        //if (Control is AControl aControl)
        //{
        //    aControl.TabIndex = Element.TabIndex;
        //}
    }

    protected virtual void UpdateEnabled()
    {
        if (Control != null)
        {
            Control.IsEnabled = Element.IsEnabled;
        }
    }

    void UpdateAlignment()
    {
        View view = Element as View;
        if (view != null)
        {
            Control.HorizontalAlignment = view.HorizontalOptions.ToNativeHorizontalAlignment();
            Control.VerticalAlignment = view.VerticalOptions.ToNativeVerticalAlignment();
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!disposing || disposed)
            return;

        disposed = true;

        if (Control != null)
        {
            Control.GotFocus -= OnGotFocus;
            Control.LostFocus -= OnLostFocus;
        }

        if (Element != null)
        {
            Element.PropertyChanged -= OnElementPropertyChanged;
            Element.FocusChangeRequested -= OnModelFocusChangeRequested;
        }

        Tracker = null;
    }
}