using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Dialogs;
using AvaloniaButton = Avalonia.Controls.Button;
using AvaloniaBrush = Avalonia.Media.Brush;
using AvaloniaContentPresenter = Avalonia.Controls.Presenters.ContentPresenter;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public class ApplicationControl : UserControl, IUiHost
{
    public static readonly StyledProperty<object> StartupPageProperty = AvaloniaProperty.Register<ApplicationWindow, object>(nameof(StartupPage));

    public static readonly StyledProperty<object> CurrentModalPageProperty = AvaloniaProperty.Register<ApplicationWindow, object>(nameof(CurrentModalPage));
    public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<ApplicationWindow, IContentLoader>(nameof(ContentLoader));
    public static readonly StyledProperty<string> CurrentTitleProperty = AvaloniaProperty.Register<ApplicationWindow, string>(nameof(CurrentTitle));
    public static readonly StyledProperty<bool> HasBackButtonProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBackButton));
    public static readonly StyledProperty<bool> HasBackButtonModalProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBackButtonModal));
    public static readonly StyledProperty<bool> HasNavigationBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasNavigationBar));
    public static readonly StyledProperty<string> BackButtonTitleProperty = AvaloniaProperty.Register<ApplicationWindow, string>(nameof(BackButtonTitle));

    public static readonly StyledProperty<AvaloniaBrush> TitleBarBackgroundColorProperty = AvaloniaProperty.Register<ApplicationWindow, AvaloniaBrush>(nameof(TitleBarBackgroundColor));
    public static readonly StyledProperty<AvaloniaBrush> TitleBarTextColorProperty = AvaloniaProperty.Register<ApplicationWindow, AvaloniaBrush>(nameof(TitleBarTextColor));

    public static readonly StyledProperty<bool> HasContentDialogProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasContentDialog));
    public static readonly StyledProperty<bool> HasModalPageProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasModalPage));

    public static readonly StyledProperty<bool> HasTopAppBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasTopAppBar));
    public static readonly StyledProperty<bool> HasBottomAppBarProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasBottomAppBar));

    public static readonly StyledProperty<AvaloniaNavigationPage?> CurrentNavigationPageProperty = AvaloniaProperty.Register<ApplicationWindow, AvaloniaNavigationPage?>(nameof(CurrentNavigationPage));
    public static readonly StyledProperty<AvaloniaMasterDetailPage?> CurrentMasterDetailPageProperty = AvaloniaProperty.Register<ApplicationWindow, AvaloniaMasterDetailPage?>(nameof(CurrentMasterDetailPage));
    public static readonly StyledProperty<ContentDialog?> CurrentContentDialogProperty = AvaloniaProperty.Register<ApplicationWindow, ContentDialog?>(nameof(CurrentContentDialog));

    /// <summary>
    /// Defines the <see cref="Title"/> property.
    /// </summary>
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Window, string?>(nameof(Title), "Window");

    static ApplicationControl() { }

    protected override Type StyleKeyOverride => typeof(ApplicationWindow);

    FormsCommandBar? topAppBar;
    FormsCommandBar? bottomAppBar;

    AvaloniaButton? previousButton;
    AvaloniaButton? previousModalButton;
    AvaloniaButton? hamburgerButton;

    public AvaloniaContentPresenter? ContentDialogContentPresenter { get; private set; }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public AvaloniaBrush TitleBarBackgroundColor
    {
        get => GetValue(TitleBarBackgroundColorProperty);
        private set => SetValue(TitleBarBackgroundColorProperty, value);
    }

    public AvaloniaBrush TitleBarTextColor
    {
        get => GetValue(TitleBarTextColorProperty);
        private set => SetValue(TitleBarTextColorProperty, value);
    }

    public object StartupPage
    {
        get => GetValue(StartupPageProperty);
        set => SetValue(StartupPageProperty, value);
    }

    public string CurrentTitle
    {
        get => GetValue(CurrentTitleProperty);
        private set => SetValue(CurrentTitleProperty, value);
    }

    public bool HasBackButton
    {
        get => GetValue(HasBackButtonProperty);
        private set => SetValue(HasBackButtonProperty, value);
    }

    public bool HasBackButtonModal
    {
        get => GetValue(HasBackButtonModalProperty);
        private set => SetValue(HasBackButtonModalProperty, value);
    }

    public bool HasNavigationBar
    {
        get => GetValue(HasNavigationBarProperty);
        private set => SetValue(HasNavigationBarProperty, value);
    }

    public string BackButtonTitle
    {
        get => GetValue(BackButtonTitleProperty);
        private set => SetValue(BackButtonTitleProperty, value);
    }

    public object CurrentModalPage
    {
        get => GetValue(CurrentModalPageProperty);
        private set => SetValue(CurrentModalPageProperty, value);
    }

    public IContentLoader ContentLoader
    {
        get => GetValue(ContentLoaderProperty);
        set => SetValue(ContentLoaderProperty, value);
    }

    public bool HasContentDialog
    {
        get => GetValue(HasContentDialogProperty);
        private set => SetValue(HasContentDialogProperty, value);
    }

    public bool HasModalPage
    {
        get => GetValue(HasModalPageProperty);
        private set => SetValue(HasModalPageProperty, value);
    }

    public bool HasTopAppBar
    {
        get => GetValue(HasTopAppBarProperty);
        private set => SetValue(HasTopAppBarProperty, value);
    }

    public bool HasBottomAppBar
    {
        get => GetValue(HasBottomAppBarProperty);
        private set => SetValue(HasBottomAppBarProperty, value);
    }


    public ContentDialog? CurrentContentDialog
    {
        get => GetValue(CurrentContentDialogProperty);
        set => SetValue(CurrentContentDialogProperty, value);
    }

    public AvaloniaNavigationPage? CurrentNavigationPage
    {
        get => GetValue(CurrentNavigationPageProperty);
        private set => SetValue(CurrentNavigationPageProperty, value);
    }

    public AvaloniaMasterDetailPage? CurrentMasterDetailPage
    {
        get => GetValue(CurrentMasterDetailPageProperty);
        private set => SetValue(CurrentMasterDetailPageProperty, value);
    }


    public EventHandler? Opened;
    public EventHandler? Closed;
    public ApplicationControl()
    {
        this.Opened += (sender, e) => Appearing();
        this.Closed += (sender, e) => Disappearing();
    }

    public void SetStartupPage(object page) { this.StartupPage = page; }

    protected virtual void Appearing() { }

    protected virtual void Disappearing() { }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        topAppBar = e.NameScope.Find<FormsCommandBar>("PART_TopAppBar");
        bottomAppBar = e.NameScope.Find<FormsCommandBar>("PART_BottomAppBar");

        previousButton = e.NameScope.Find<AvaloniaButton>("PART_Previous");
        if (previousButton != null)
        {
            previousButton.Click += OnPreviousButtonClick;
        }

        previousModalButton = e.NameScope.Find<AvaloniaButton>("PART_Previous_Modal");
        if (previousButton != null)
        {
            previousModalButton.Click += OnPreviousModalButtonClick;
        }

        hamburgerButton = e.NameScope.Find<AvaloniaButton>("PART_Hamburger");
        if (hamburgerButton != null)
        {
            hamburgerButton.Click += OmHamburgerButtonClick;
        }

        ContentDialogContentPresenter = e.NameScope.Find<AvaloniaContentPresenter>("PART_ContentDialog_ContentPresenter");
    }


    protected virtual void OmHamburgerButtonClick(object sender, RoutedEventArgs e)
    {
        if (CurrentMasterDetailPage != null)
        {
            CurrentMasterDetailPage.IsPresented = !CurrentMasterDetailPage.IsPresented;
        }
    }

    protected virtual void OnPreviousModalButtonClick(object? sender, RoutedEventArgs e) { OnBackSystemButtonPressed(); }

    protected virtual void OnPreviousButtonClick(object? sender, RoutedEventArgs e)
    {
        if (CurrentNavigationPage is {StackDepth: > 1})
        {
            CurrentNavigationPage.OnBackButtonPressed();
        }
    }

    public virtual void OnBackSystemButtonPressed() { PopModal(); }

    public void ShowContentDialog(ContentDialog contentDialog)
    {
        this.CurrentContentDialog = contentDialog;
        this.HasContentDialog = true;
        if (ContentDialogContentPresenter.Content == null)
        {
            ContentDialogContentPresenter.Content = contentDialog;
        }
    }

    public void HideContentDialog()
    {
        this.CurrentContentDialog = null;
        if (ContentDialogContentPresenter.Content != null)
        {
            ContentDialogContentPresenter.Content = null;
        }

        this.HasContentDialog = false;
    }

    public ObservableCollection<object> InternalChildren { get; } = new ObservableCollection<object>();

    public void PushModal(object page) { PushModal(page, true); }

    public void PushModal(object page, bool animated)
    {
        InternalChildren.Add(page);
        this.CurrentModalPage = InternalChildren.Last();
        this.HasModalPage = true;
        this.HasBackButtonModal = true;
    }

    public object PopModal() { return PopModal(true); }

    public object PopModal(bool animated)
    {
        if (InternalChildren.Count < 1)
        {
            return null;
        }

        var modal = InternalChildren.Last();

        if (InternalChildren.Remove(modal))
        {
            CurrentModalPage = InternalChildren.LastOrDefault();
        }

        this.HasBackButtonModal = InternalChildren.Count >= 1;
        this.HasModalPage = InternalChildren.Count >= 1;

        return modal;
    }

    public void SynchronizeAppBar()
    {
        IEnumerable<AvaloniaDynamicContentPage> children = this.FindVisualChildren<AvaloniaDynamicContentPage>();
        var avaloniaDynamicContentPages = children as AvaloniaDynamicContentPage[] ?? children.ToArray();
        CurrentTitle = avaloniaDynamicContentPages.FirstOrDefault()?.GetTitle();
        HasNavigationBar = avaloniaDynamicContentPages.FirstOrDefault()?.GetHasNavigationBar() ?? false;
        CurrentNavigationPage = avaloniaDynamicContentPages.OfType<AvaloniaNavigationPage>()?.FirstOrDefault();
        CurrentMasterDetailPage = avaloniaDynamicContentPages.OfType<AvaloniaMasterDetailPage>()?.FirstOrDefault();
        var page = avaloniaDynamicContentPages.FirstOrDefault();
        if (page != null)
        {
            TitleBarBackgroundColor = page.GetTitleBarBackgroundColor();
            TitleBarTextColor = page.GetTitleBarTextColor();
        }
        else
        {
            ClearValue(TitleBarBackgroundColorProperty);
            ClearValue(TitleBarTextColorProperty);
        }

        if (hamburgerButton != null)
        {
            hamburgerButton.IsVisible = CurrentMasterDetailPage != null;
        }

        if (CurrentNavigationPage != null)
        {
            HasBackButton = CurrentNavigationPage.GetHasBackButton();
            BackButtonTitle = CurrentNavigationPage.GetBackButtonTitle();
        }
        else
        {
            HasBackButton = false;
            BackButtonTitle = "";
        }
    }

    public void SynchronizeToolbarCommands()
    {
        IEnumerable<AvaloniaDynamicContentPage> childrens = this.FindVisualChildren<AvaloniaDynamicContentPage>();

        var page = childrens.FirstOrDefault();
        if (page == null) return;

        if (topAppBar != null)
        {
            topAppBar.PrimaryCommands = page.GetPrimaryTopBarCommands();
            topAppBar.SecondaryCommands = page.GetSecondaryTopBarCommands();
            topAppBar.Reset();

            // TODO:
            HasTopAppBar = false;
        }

        if (bottomAppBar != null)
        {
            bottomAppBar.PrimaryCommands = page.GetPrimaryBottomBarCommands();
            bottomAppBar.SecondaryCommands = page.GetSecondaryBottomBarCommands();
            bottomAppBar.Content = childrens.LastOrDefault(x => x.ContentBottomBar != null)?.ContentBottomBar;
            bottomAppBar.Reset();

            // TODO:
            HasBottomAppBar = bottomAppBar.PrimaryCommands.Count() > 0 || bottomAppBar.SecondaryCommands.Count() > 0 || bottomAppBar.Content != null;
        }
    }
}