using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaAppBar : ContentControl
{
    public event EventHandler<object>? Closing;
    public event EventHandler<object>? Closed;
    public event EventHandler<object>? Opened;
    public event EventHandler<object>? Opening;

    public AvaloniaAppBar() => LayoutUpdated += OnLayoutUpdated;

    #region Loaded & Unloaded
    public event EventHandler<RoutedEventArgs>? Loaded;
    public event EventHandler<RoutedEventArgs>? Unloaded;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OnLoaded(new RoutedEventArgs());
        Appearing();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        OnUnloaded(new RoutedEventArgs());
        Disappearing();
    }

    protected virtual void OnLoaded(RoutedEventArgs e) => Loaded?.Invoke(this, e);
    protected virtual void OnUnloaded(RoutedEventArgs e) => Unloaded?.Invoke(this, e);

    #endregion

    #region Appearing & Disappearing
    protected virtual void Appearing()
    {
    }

    protected virtual void Disappearing()
    {
    }
    #endregion

    #region LayoutUpdated & SizeChanged
    public event EventHandler<EventArgs>? SizeChanged;
    protected virtual void OnSizeChanged(EventArgs e) => SizeChanged?.Invoke(this, e);

    protected virtual void OnLayoutUpdated(object? sender, EventArgs e) => OnSizeChanged(e);

    #endregion

    protected virtual void OnClosed(object e) => Closed?.Invoke(this, e);
    protected virtual void OnClosing(object e) => Closing?.Invoke(this, e);
    protected virtual void OnOpened(object e) => Opened?.Invoke(this, e);
    protected virtual void OnOpening(object e) => Opening?.Invoke(this, e);
}