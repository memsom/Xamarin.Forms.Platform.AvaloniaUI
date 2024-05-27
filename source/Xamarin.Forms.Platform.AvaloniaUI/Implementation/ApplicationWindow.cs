using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Dialogs;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public class ApplicationWindow : Window
{
    static ApplicationWindow()
    {
    }

    public Avalonia.Controls.Presenters.ContentPresenter? ContentDialogContentPresenter { get; private set; }

    public ObservableCollection<object> InternalChildren { get; } = new ObservableCollection<object>();

    public void PushModal(object page)
    {
        PushModal(page, true);
    }

    public void PushModal(object page, bool animated)
    {
        InternalChildren.Add(page);
        this.CurrentModalPage = InternalChildren.Last();
        this.HasModalPage = true;
        //this.HasBackButtonModal = true;
    }

    public object? PopModal(bool animated = true)
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
        //this.HasBackButtonModal = InternalChildren.Count >= 1;
        this.HasModalPage = InternalChildren.Count >= 1;

        return modal;
    }

    public static readonly StyledProperty<object> CurrentModalPageProperty = AvaloniaProperty.Register<ApplicationWindow, object>(nameof(CurrentModalPage));

    public object CurrentModalPage
    {
        get => (object)GetValue(CurrentModalPageProperty);
        private set => SetValue(CurrentModalPageProperty, value);
    }

    public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<ApplicationWindow, IContentLoader>(nameof(ContentLoader));
    public IContentLoader ContentLoader
    {
        get => (IContentLoader)GetValue(ContentLoaderProperty);
        set => SetValue(ContentLoaderProperty, value);
    }

    public static readonly StyledProperty<bool> HasModalPageProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasModalPage));

    public bool HasModalPage
    {
        get => (bool)GetValue(HasModalPageProperty);
        private set => SetValue(HasModalPageProperty, value);
    }

    public static readonly StyledProperty<object> StartupPageProperty = AvaloniaProperty.Register<ApplicationWindow, object>(nameof(StartupPage));

    public object StartupPage
    {
        get => (object)GetValue(StartupPageProperty);
        set => SetValue(StartupPageProperty, value);
    }

    public static readonly StyledProperty<ContentDialog> CurrentContentDialogProperty = AvaloniaProperty.Register<ApplicationWindow, ContentDialog>(nameof(CurrentContentDialog));

    public ContentDialog CurrentContentDialog
    {
        get => GetValue(CurrentContentDialogProperty);
        set => SetValue(CurrentContentDialogProperty, value);
    }

    public static readonly StyledProperty<bool> HasContentDialogProperty = AvaloniaProperty.Register<ApplicationWindow, bool>(nameof(HasContentDialog));

    public bool HasContentDialog
    {
        get => (bool)GetValue(HasContentDialogProperty);
        private set => SetValue(HasContentDialogProperty, value);
    }

    public ApplicationWindow()
    {
        this.Opened += (sender, e) => Appearing();
        this.Closed += (sender, e) => Disappearing();
    }

    public void SetStartupPage(object page)
    {
        this.StartupPage = page;
    }

    protected virtual void Appearing()
    {
    }

    protected virtual void Disappearing()
    {
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        ContentDialogContentPresenter = e.NameScope.Find<Avalonia.Controls.Presenters.ContentPresenter>("PART_ContentDialog_ContentPresenter");
    }

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

}