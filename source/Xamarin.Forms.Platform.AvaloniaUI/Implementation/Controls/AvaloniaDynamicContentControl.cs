using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaDynamicContentControl : ContentControl
{
    public static readonly StyledProperty<object> SourceProperty = AvaloniaProperty.Register<AvaloniaDynamicContentControl, object>(nameof(Source));
    public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<AvaloniaDynamicContentControl, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

    static AvaloniaDynamicContentControl()
    {
        SourceProperty.Changed.AddClassHandler<AvaloniaDynamicContentControl>((x, e) => x.OnSourcePropertyChanged(e));
        ContentLoaderProperty.Changed.AddClassHandler<AvaloniaDynamicContentControl>((x, e) => x.OnContentLoaderPropertyChanged(e));
    }

    protected override Type StyleKeyOverride => typeof(ContentControl);

    private CancellationTokenSource tokenSource;

    public object Source
    {
        get { return GetValue(SourceProperty); }
        set { SetValue(SourceProperty, value); }
    }

    public IContentLoader ContentLoader
    {
        get { return GetValue(ContentLoaderProperty); }
        set { SetValue(ContentLoaderProperty, value); }
    }

    public AvaloniaDynamicContentControl() { LayoutUpdated += OnLayoutUpdated; }

    #region Loaded & Unloaded

    public event EventHandler<RoutedEventArgs> Loaded;
    public event EventHandler<RoutedEventArgs> Unloaded;

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

    protected virtual void OnLoaded(RoutedEventArgs e) { Loaded?.Invoke(this, e); }
    protected virtual void OnUnloaded(RoutedEventArgs e) { Unloaded?.Invoke(this, e); }

    #endregion

    #region Appearing & Disappearing

    protected virtual void Appearing() { }

    protected virtual void Disappearing() { }

    #endregion

    #region LayoutUpdated & SizeChanged

    public event EventHandler<EventArgs> SizeChanged;
    protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

    protected virtual void OnLayoutUpdated(object sender, EventArgs e)
    {
        OnSizeChanged(e);

        ContentLoader.OnSizeContentChanged(this, Source);
    }

    #endregion

    private void OnSourcePropertyChanged(AvaloniaPropertyChangedEventArgs e) { OnSourceChanged(e.OldValue, e.NewValue); }

    private void OnContentLoaderPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue == null)
        {
            throw new ArgumentNullException("ContentLoader");
        }
    }

    protected virtual void OnSourceChanged(object? oldValue, object? newValue)
    {
        if (newValue != null && newValue.Equals(oldValue)) return;

        var localTokenSource = new CancellationTokenSource();
        tokenSource = localTokenSource;

        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        var task = ContentLoader.LoadContentAsync(this, oldValue, newValue, tokenSource.Token);

        task.ContinueWith(t =>
        {
            try
            {
                if (t.IsFaulted || t.IsCanceled || localTokenSource.IsCancellationRequested)
                {
                    Content = null;
                }
                else
                {
                    if (t.Result is Control control)
                    {
                        if (control.Parent != null)
                        {
                            Content = control.Parent;
                        }
                        else
                        {
                            Content = control;
                        }
                    }
                    else
                    {
                        Content = t.Result;
                    }
                }
            }
            finally
            {
                if (tokenSource == localTokenSource)
                {
                    tokenSource = null;
                }

                localTokenSource.Dispose();
            }
        }, scheduler);
    }
}