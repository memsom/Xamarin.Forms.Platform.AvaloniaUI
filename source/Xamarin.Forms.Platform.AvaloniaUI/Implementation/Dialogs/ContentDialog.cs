using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Styling;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Dialogs;

public class ContentDialog : ContentControl, IContentDialog
{
    public static readonly StyledProperty<object> TitleProperty = AvaloniaProperty.Register<ContentDialog, object>(nameof(Title));
    public static readonly StyledProperty<Avalonia.Markup.Xaml.Templates.DataTemplate> TitleTemplateProperty = AvaloniaProperty.Register<ContentDialog, Avalonia.Markup.Xaml.Templates.DataTemplate>(nameof(TitleTemplate));
    public static readonly StyledProperty<bool> FullSizeDesiredProperty = AvaloniaProperty.Register<ContentDialog, bool>(nameof(FullSizeDesired));
    public static readonly StyledProperty<bool> IsPrimaryButtonEnabledProperty = AvaloniaProperty.Register<ContentDialog, bool>(nameof(IsPrimaryButtonEnabled));
    public static readonly StyledProperty<bool> IsSecondaryButtonEnabledProperty = AvaloniaProperty.Register<ContentDialog, bool>(nameof(IsSecondaryButtonEnabled));
    public static readonly StyledProperty<ICommand> PrimaryButtonCommandProperty = AvaloniaProperty.Register<ContentDialog, ICommand>(nameof(PrimaryButtonCommand));
    public static readonly StyledProperty<ICommand> SecondaryButtonCommandProperty = AvaloniaProperty.Register<ContentDialog, ICommand>(nameof(SecondaryButtonCommand));
    public static readonly StyledProperty<string> PrimaryButtonTextProperty = AvaloniaProperty.Register<ContentDialog, string>(nameof(PrimaryButtonText));
    public static readonly StyledProperty<object> PrimaryButtonCommandParameterProperty = AvaloniaProperty.Register<ContentDialog, object>(nameof(PrimaryButtonCommandParameter));
    public static readonly StyledProperty<string> SecondaryButtonTextProperty = AvaloniaProperty.Register<ContentDialog, string>(nameof(SecondaryButtonText));
    public static readonly StyledProperty<object> SecondaryButtonCommandParameterProperty = AvaloniaProperty.Register<ContentDialog, object>(nameof(SecondaryButtonCommandParameter));

    static ContentDialog()
    {
    }

    protected override Type StyleKeyOverride => typeof(ContentDialog);

    TaskCompletionSource<ContentDialogResult> tcs;

    public Avalonia.Controls.Button PrimaryButton { get; private set; }
    public Avalonia.Controls.Button SecondaryButton { get; private set; }

    public object Title
    {
        get { return (object)GetValue(TitleProperty); }
        set { SetValue(TitleProperty, value); }
    }

    public Avalonia.Markup.Xaml.Templates.DataTemplate TitleTemplate
    {
        get { return (Avalonia.Markup.Xaml.Templates.DataTemplate)GetValue(TitleTemplateProperty); }
        set { SetValue(TitleTemplateProperty, value); }
    }

    public bool FullSizeDesired
    {
        get { return (bool)GetValue(FullSizeDesiredProperty); }
        set { SetValue(FullSizeDesiredProperty, value); }
    }

    public bool IsPrimaryButtonEnabled
    {
        get { return (bool)GetValue(IsPrimaryButtonEnabledProperty); }
        set { SetValue(IsPrimaryButtonEnabledProperty, value); }
    }

    public bool IsSecondaryButtonEnabled
    {
        get { return (bool)GetValue(IsSecondaryButtonEnabledProperty); }
        set { SetValue(IsSecondaryButtonEnabledProperty, value); }
    }

    public ICommand PrimaryButtonCommand
    {
        get { return (ICommand)GetValue(PrimaryButtonCommandProperty); }
        set { SetValue(PrimaryButtonCommandProperty, value); }
    }

    public ICommand SecondaryButtonCommand
    {
        get { return (ICommand)GetValue(SecondaryButtonCommandProperty); }
        set { SetValue(SecondaryButtonCommandProperty, value); }
    }

    public string PrimaryButtonText
    {
        get { return (string)GetValue(PrimaryButtonTextProperty); }
        set { SetValue(PrimaryButtonTextProperty, value); }
    }

    public object PrimaryButtonCommandParameter
    {
        get { return (object)GetValue(PrimaryButtonCommandParameterProperty); }
        set { SetValue(PrimaryButtonCommandParameterProperty, value); }
    }

    public string SecondaryButtonText
    {
        get { return (string)GetValue(SecondaryButtonTextProperty); }
        set { SetValue(SecondaryButtonTextProperty, value); }
    }

    public object SecondaryButtonCommandParameter
    {
        get { return (object)GetValue(SecondaryButtonCommandParameterProperty); }
        set { SetValue(SecondaryButtonCommandParameterProperty, value); }
    }

    public event EventHandler<ContentDialogClosedEventArgs> Closed;
    public event EventHandler<ContentDialogClosingEventArgs> Closing;
    public event EventHandler<ContentDialogOpenedEventArgs> Opened;

    public event EventHandler<ContentDialogButtonClickEventArgs> PrimaryButtonClick;
    public event EventHandler<ContentDialogButtonClickEventArgs> SecondaryButtonClick;

    public ApplicationWindow ParentWindow => this.GetUiHost() as ApplicationWindow;

    public ContentDialog()
    {
        LayoutUpdated += OnLayoutUpdated;
    }

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
    protected virtual void Appearing()
    {
    }

    protected virtual void Disappearing()
    {
    }
    #endregion

    #region LayoutUpdated & SizeChanged
    public event EventHandler<EventArgs> SizeChanged;
    protected virtual void OnSizeChanged(EventArgs e) { SizeChanged?.Invoke(this, e); }

    protected virtual void OnLayoutUpdated(object sender, EventArgs e)
    {
        OnSizeChanged(e);
    }
    #endregion

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        this.PrimaryButton = e.NameScope.Find<Avalonia.Controls.Button>("PART_PrimaryButton");
        this.PrimaryButton.Click += OnPrimaryButtonClick;
        this.SecondaryButton = e.NameScope.Find<Avalonia.Controls.Button>("PART_SecondaryButton");
        this.SecondaryButton.Click += OnSecondaryButtonClick;
    }

    protected virtual void OnPrimaryButtonClick(object sender, EventArgs e)
    {
        OnPrimaryButtonRoutedExecuted();
    }

    protected virtual void OnSecondaryButtonClick(object sender, EventArgs e)
    {
        OnSecondaryButtonRoutedExecuted();
    }

    private void OnPrimaryButtonRoutedExecuted()
    {
        ContentDialogButtonClickEventArgs ContentDialogButtonClickEventArgs = new ContentDialogButtonClickEventArgs();

        PrimaryButtonClick?.Invoke(this, ContentDialogButtonClickEventArgs);

        if (!ContentDialogButtonClickEventArgs.Cancel)
        {
            PrimaryButtonCommand?.Execute(PrimaryButtonCommandParameter);
            tcs.TrySetResult(ContentDialogResult.Primary);
        }
    }

    private void OnSecondaryButtonRoutedExecuted()
    {
        ContentDialogButtonClickEventArgs ContentDialogButtonClickEventArgs = new ContentDialogButtonClickEventArgs();

        SecondaryButtonClick?.Invoke(this, ContentDialogButtonClickEventArgs);

        if (!ContentDialogButtonClickEventArgs.Cancel)
        {
            SecondaryButtonCommand?.Execute(SecondaryButtonCommandParameter);
            tcs.TrySetResult(ContentDialogResult.Secondary);
        }
    }

    public async Task<ContentDialogResult> ShowAsync()
    {
        if (ParentWindow != null)
        {
            ParentWindow.ShowContentDialog(this);
            ContentDialogOpenedEventArgs ContentDialogOpenedEventArgs = new ContentDialogOpenedEventArgs();
            Opened?.Invoke(this, ContentDialogOpenedEventArgs);
        }

        ContentDialogResult contentDialogResult = ContentDialogResult.None;
        bool exit = false;

        while (!exit)
        {
            tcs = new TaskCompletionSource<ContentDialogResult>();
            contentDialogResult = await tcs.Task;
            exit = InternalHide(contentDialogResult);
        }

        return contentDialogResult;
    }

    private bool InternalHide(ContentDialogResult contentDialogResult)
    {
        ContentDialogClosingEventArgs ContentDialogClosingEventArgs = new ContentDialogClosingEventArgs(contentDialogResult);
        Closing?.Invoke(this, ContentDialogClosingEventArgs);

        if (!ContentDialogClosingEventArgs.Cancel && ParentWindow != null)
        {
            ParentWindow.HideContentDialog();
            ContentDialogClosedEventArgs ContentDialogClosedEventArgs = new ContentDialogClosedEventArgs(contentDialogResult);
            Closed?.Invoke(this, ContentDialogClosedEventArgs);
            return true;
        }
        return false;
    }

    public void Hide()
    {
        InternalHide(ContentDialogResult.None);
    }
}