using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Media.Imaging;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;
using AvaloniaBrush = Avalonia.Media.Brush;
using AvaloniaThickness = Avalonia.Thickness;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaPageControl : ContentControl, IToolbarProvider, ITitleViewRendererController
{
    public static readonly StyledProperty<bool> TitleVisibilityProperty = AvaloniaProperty.Register<AvaloniaPageControl, bool>(nameof(TitleVisibility));
    public static readonly StyledProperty<AvaloniaBrush> ToolbarBackgroundProperty = AvaloniaProperty.Register<AvaloniaPageControl, AvaloniaBrush>(nameof(ToolbarBackground));
    public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<AvaloniaPageControl, string>(nameof(BackButtonTitle));
    public static readonly StyledProperty<AvaloniaThickness> ContentMarginProperty = AvaloniaProperty.Register<AvaloniaPageControl, AvaloniaThickness>(nameof(ContentMargin));
    public static readonly StyledProperty<Bitmap> TitleIconProperty = AvaloniaProperty.Register<AvaloniaPageControl, Bitmap>(nameof(TitleIcon));
    public static readonly StyledProperty<object> TitleViewContentProperty = AvaloniaProperty.Register<AvaloniaPageControl, object>(nameof(TitleViewContent));
    public static readonly StyledProperty<bool> TitleViewVisibilityProperty = AvaloniaProperty.Register<AvaloniaPageControl, bool>(nameof(TitleViewVisibility));
    public static readonly StyledProperty<double> TitleInsetProperty = AvaloniaProperty.Register<AvaloniaPageControl, double>(nameof(TitleInset));
    public static readonly StyledProperty<AvaloniaBrush> TitleBrushProperty = AvaloniaProperty.Register<AvaloniaPageControl, AvaloniaBrush>(nameof(TitleBrush));

    static AvaloniaPageControl() { }

    protected override Type StyleKeyOverride => typeof(AvaloniaPageControl);

    protected FormsCommandBar? commandBar;
    public FormsCommandBar? CommandBar => commandBar;

    protected Control? titleViewPresenter;
    public Control? TitleViewPresenter => titleViewPresenter;

    ToolbarPlacement toolbarPlacement;
    bool toolbarDynamicOverflowEnabled = true;
    readonly ToolbarPlacementHelper toolbarPlacementHelper = new();

    public bool ShouldShowToolbar
    {
        get => toolbarPlacementHelper.ShouldShowToolBar;
        set => toolbarPlacementHelper.ShouldShowToolBar = value;
    }

    public Bitmap TitleIcon
    {
        get => GetValue(TitleIconProperty);
        set => SetValue(TitleIconProperty, value);
    }

    public object TitleViewContent
    {
        get => GetValue(TitleViewContentProperty);
        set => SetValue(TitleViewContentProperty, value);
    }

    TaskCompletionSource<FormsCommandBar>? commandBarTcs;
    ContentPresenter? presenter;
    TitleViewManager? titleViewManager;

    public AvaloniaPageControl() => LayoutUpdated += OnLayoutUpdated;

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

    protected virtual void OnLayoutUpdated(object? sender, EventArgs e) { OnSizeChanged(e); }

    #endregion

    public string BackButtonTitle
    {
        get => GetValue(BackButtonTitleProperty);
        set => SetValue(BackButtonTitleProperty, value);
    }

    public double ContentHeight => presenter != null ? presenter.Height : 0;

    public AvaloniaThickness ContentMargin
    {
        get => GetValue(ContentMarginProperty);
        set => SetValue(ContentMarginProperty, value);
    }

    public double ContentWidth => presenter != null ? presenter.Width : 0;

    public AvaloniaBrush ToolbarBackground
    {
        get => GetValue(ToolbarBackgroundProperty);
        set => SetValue(ToolbarBackgroundProperty, value);
    }

    public ToolbarPlacement ToolbarPlacement
    {
        get => toolbarPlacement;
        set
        {
            toolbarPlacement = value;
            toolbarPlacementHelper.UpdateToolbarPlacement();
        }
    }

    public bool ToolbarDynamicOverflowEnabled
    {
        get => toolbarDynamicOverflowEnabled;
        set
        {
            toolbarDynamicOverflowEnabled = value;
            UpdateToolbarDynamicOverflowEnabled();
        }
    }

    public bool TitleVisibility
    {
        get => GetValue(TitleVisibilityProperty);
        set => SetValue(TitleVisibilityProperty, value);
    }

    public bool TitleViewVisibility
    {
        get => GetValue(TitleViewVisibilityProperty);
        set => SetValue(TitleViewVisibilityProperty, value);
    }

    public AvaloniaBrush TitleBrush
    {
        get => GetValue(TitleBrushProperty);
        set => SetValue(TitleBrushProperty, value);
    }

    public double TitleInset
    {
        get => GetValue(TitleInsetProperty);
        set => SetValue(TitleInsetProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        presenter = e.NameScope.Find<ContentPresenter>("presenter");

        titleViewPresenter = e.NameScope.Find<Control>("TitleViewPresenter");

        commandBar = e.NameScope.Find<FormsCommandBar>("CommandBar");

        titleViewManager = new TitleViewManager(this);

        toolbarPlacementHelper.Initialize(commandBar, () => ToolbarPlacement, name => e.NameScope.Find(name) as AvaloniaObject);
        UpdateToolbarDynamicOverflowEnabled();

        TaskCompletionSource<FormsCommandBar> tcs = commandBarTcs;
        tcs?.SetResult(commandBar);
    }

    void UpdateToolbarDynamicOverflowEnabled()
    {
        if (commandBar != null)
        {
            commandBar.IsDynamicOverflowEnabled = ToolbarDynamicOverflowEnabled;
        }
    }

    public Task<FormsCommandBar> GetCommandBarAsync()
    {
        if (commandBar != null) return Task.FromResult(commandBar);

        commandBarTcs = new TaskCompletionSource<FormsCommandBar>();
        ApplyTemplate();
        return commandBarTcs.Task;
    }
}