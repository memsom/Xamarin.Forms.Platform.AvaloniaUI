using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Styling;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class DynamicContentControl : ContentControl, IStyleable
{
    public static readonly StyledProperty<object> SourceProperty = AvaloniaProperty.Register<DynamicContentControl, object>(nameof(Source));
    public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<DynamicContentControl, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

    static DynamicContentControl()
    {
        SourceProperty.Changed.AddClassHandler<DynamicContentControl>((x, e) => x.OnSourcePropertyChanged(e));
        ContentLoaderProperty.Changed.AddClassHandler<DynamicContentControl>((x, e) => x.OnContentLoaderPropertyChanged(e));
    }

    Type IStyleable.StyleKey => typeof(ContentControl);

    private CancellationTokenSource tokenSource;

    public object Source
    {
        get { return (object)GetValue(SourceProperty); }
        set { SetValue(SourceProperty, value); }
    }

    public IContentLoader ContentLoader
    {
        get { return (IContentLoader)GetValue(ContentLoaderProperty); }
        set { SetValue(ContentLoaderProperty, value); }
    }

    public DynamicContentControl() { LayoutUpdated += OnLayoutUpdated; }

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
        this.tokenSource = localTokenSource;

        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        var task = this.ContentLoader.LoadContentAsync(this, oldValue, newValue, this.tokenSource.Token);

        task.ContinueWith(t =>
        {
            try
            {
                if (t.IsFaulted || t.IsCanceled || localTokenSource.IsCancellationRequested)
                {
                    this.Content = null;
                }
                else
                {
                    if (t.Result is Control control)
                    {
                        if (control.Parent != null)
                        {
                            this.Content = control.Parent;
                        }
                        else
                        {
                            this.Content = control;
                        }
                    }
                    else
                    {
                        this.Content = t.Result;
                    }
                }
            }
            finally
            {
                if (this.tokenSource == localTokenSource)
                {
                    this.tokenSource = null;
                }

                localTokenSource.Dispose();
            }
        }, scheduler);
        return;
    }
}