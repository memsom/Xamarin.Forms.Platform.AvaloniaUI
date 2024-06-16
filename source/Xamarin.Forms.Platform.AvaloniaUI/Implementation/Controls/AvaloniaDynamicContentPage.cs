using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Navigation;
using IAvaloniaNavigation = Xamarin.Forms.Platform.AvaloniaUI.Implementation.Navigation.INavigation;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaDynamicContentPage : UserControl
{
    public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());

    public static readonly StyledProperty<string> TitleProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, string>(nameof(Title));
    public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, string>(nameof(BackButtonTitle));
    public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, bool>(nameof(HasNavigationBar), true);
    public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, bool>(nameof(HasBackButton), true);

    public static readonly StyledProperty<ObservableCollection<Control>> PrimaryTopBarCommandsProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, ObservableCollection<Control>>(nameof(PrimaryTopBarCommands));
    public static readonly StyledProperty<ObservableCollection<Control>> SecondaryTopBarCommandsProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, ObservableCollection<Control>>(nameof(SecondaryTopBarCommands));
    public static readonly StyledProperty<ObservableCollection<Control>> PrimaryBottomBarCommandsProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, ObservableCollection<Control>>(nameof(PrimaryBottomBarCommands));
    public static readonly StyledProperty<ObservableCollection<Control>> SecondaryBottomBarCommandsProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, ObservableCollection<Control>>(nameof(SecondaryBottomBarCommands));
    public static readonly StyledProperty<ObservableCollection<Control>> ContentBottomBarProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, ObservableCollection<Control>>(nameof(ContentBottomBar));

    public static readonly StyledProperty<Avalonia.Media.Brush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, Avalonia.Media.Brush>(nameof(TitleBarBackgroundColor));
    public static readonly StyledProperty<Avalonia.Media.Brush> TitleBarTextColorProperty = AvaloniaProperty.Register<AvaloniaDynamicContentPage, Avalonia.Media.Brush>(nameof(TitleBarTextColor));

    static AvaloniaDynamicContentPage() { }

    public IContentLoader ContentLoader
    {
        get => GetValue(ContentLoaderProperty);
        set => SetValue(ContentLoaderProperty, value);
    }

    public Avalonia.Media.Brush TitleBarBackgroundColor
    {
        get => GetValue(TitleBarBackgroundColorProperty);
        set => SetValue(TitleBarBackgroundColorProperty, value);
    }

    public Avalonia.Media.Brush TitleBarTextColor
    {
        get => GetValue(TitleBarTextColorProperty);
        set => SetValue(TitleBarTextColorProperty, value);
    }

    public string Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string BackButtonTitle
    {
        get => GetValue(BackButtonTitleProperty);
        set => SetValue(BackButtonTitleProperty, value);
    }

    public bool HasNavigationBar
    {
        get => GetValue(HasNavigationBarProperty);
        set => SetValue(HasNavigationBarProperty, value);
    }

    public bool HasBackButton
    {
        get => GetValue(HasBackButtonProperty);
        set => SetValue(HasBackButtonProperty, value);
    }

    public ObservableCollection<Control> PrimaryTopBarCommands
    {
        get => GetValue(PrimaryTopBarCommandsProperty);
        set => SetValue(PrimaryTopBarCommandsProperty, value);
    }

    public ObservableCollection<Control> SecondaryTopBarCommands
    {
        get => GetValue(SecondaryTopBarCommandsProperty);
        set => SetValue(SecondaryTopBarCommandsProperty, value);
    }

    public ObservableCollection<Control> PrimaryBottomBarCommands
    {
        get => GetValue(PrimaryBottomBarCommandsProperty);
        set => SetValue(PrimaryBottomBarCommandsProperty, value);
    }

    public ObservableCollection<Control> SecondaryBottomBarCommands
    {
        get => GetValue(SecondaryBottomBarCommandsProperty);
        set => SetValue(SecondaryBottomBarCommandsProperty, value);
    }

    public object ContentBottomBar
    {
        get => GetValue(ContentBottomBarProperty);
        set => SetValue(ContentBottomBarProperty, value);
    }

    public Navigation.INavigation Navigation
    {
        get
        {
            var nav = this.TryFindParent<AvaloniaNavigationPage>() as IAvaloniaNavigation;
            return nav ?? new DefaultNavigation();
        }
    }

    public ApplicationWindow? ParentWindow => this.GetUiHost() as ApplicationWindow;

    public AvaloniaDynamicContentPage()
    {
        SetValue(PrimaryTopBarCommandsProperty, new ObservableCollection<Control>());
        SetValue(SecondaryTopBarCommandsProperty, new ObservableCollection<Control>());
        SetValue(PrimaryBottomBarCommandsProperty, new ObservableCollection<Control>());
        SetValue(SecondaryBottomBarCommandsProperty, new ObservableCollection<Control>());

        LayoutUpdated += OnLayoutUpdated;
    }

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
        PrimaryTopBarCommands.CollectionChanged += Commands_CollectionChanged;
        SecondaryTopBarCommands.CollectionChanged += Commands_CollectionChanged;
        PrimaryBottomBarCommands.CollectionChanged += Commands_CollectionChanged;
        SecondaryBottomBarCommands.CollectionChanged += Commands_CollectionChanged;
        ParentWindow?.SynchronizeToolbarCommands();
        ParentWindow?.SynchronizeAppBar();
    }

    protected virtual void Disappearing()
    {
        PrimaryTopBarCommands.CollectionChanged -= Commands_CollectionChanged;
        SecondaryTopBarCommands.CollectionChanged -= Commands_CollectionChanged;
        PrimaryBottomBarCommands.CollectionChanged -= Commands_CollectionChanged;
        SecondaryBottomBarCommands.CollectionChanged -= Commands_CollectionChanged;
    }

    #endregion

    #region LayoutUpdated & SizeChanged

    public event EventHandler<EventArgs>? SizeChanged;
    protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

    protected virtual void OnLayoutUpdated(object? sender, EventArgs e) { OnSizeChanged(e); }

    #endregion

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);
        if (change.Property == TitleProperty ||
            change.Property == HasBackButtonProperty ||
            change.Property == HasNavigationBarProperty ||
            change.Property == TitleBarBackgroundColorProperty ||
            change.Property == TitleBarTextColorProperty)
        {
            ParentWindow?.SynchronizeAppBar();
        }
    }


    private void Commands_CollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        ParentWindow?.SynchronizeToolbarCommands();
    }

    public virtual string GetTitle() { return Title; }

    public virtual bool GetHasNavigationBar() => HasNavigationBar;

    public virtual Avalonia.Media.Brush GetTitleBarBackgroundColor() => TitleBarBackgroundColor;

    public virtual Avalonia.Media.Brush GetTitleBarTextColor() => TitleBarTextColor;

    public virtual IEnumerable<Control> GetPrimaryTopBarCommands() => PrimaryTopBarCommands;

    public virtual IEnumerable<Control> GetSecondaryTopBarCommands() => SecondaryTopBarCommands;

    public virtual IEnumerable<Control> GetPrimaryBottomBarCommands() => PrimaryBottomBarCommands;

    public virtual IEnumerable<Control> GetSecondaryBottomBarCommands() => SecondaryBottomBarCommands;
}