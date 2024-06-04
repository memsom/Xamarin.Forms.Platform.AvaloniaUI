using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaMultiContentPage : AvaloniaDynamicContentPage
{
    public static readonly StyledProperty<ObservableCollection<object>> ItemsSourceProperty = AvaloniaProperty.Register<AvaloniaMultiContentPage, ObservableCollection<object>>(nameof(ItemsSource));
    public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<AvaloniaMultiContentPage, object>(nameof(SelectedItem));
    public static readonly StyledProperty<int> SelectedIndexProperty = AvaloniaProperty.Register<AvaloniaMultiContentPage, int>(nameof(SelectedIndex), 0);

    static AvaloniaMultiContentPage()
    {
        SelectedItemProperty.Changed.AddClassHandler<AvaloniaMultiContentPage>((x, e) => x.OnSelectedItemPropertyChanged(e));
    }

    public ObservableCollection<object> ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public object SelectedItem
    {
        get => GetValue(SelectedItemProperty);
        set => SetValue(SelectedItemProperty, value);
    }

    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public AvaloniaTransitioningContentControl? ContentControl { get; private set; }

    public event EventHandler<AvaloniaSelectionChangedEventArgs>? SelectionChanged;

    protected AvaloniaMultiContentPage() => SetValue(ItemsSourceProperty, new ObservableCollection<object>());

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        ContentControl = e.NameScope.Find<AvaloniaTransitioningContentControl>("PART_Multi_Content");
    }

    public override bool GetHasNavigationBar()
    {
        if (ContentControl is {Content: AvaloniaDynamicContentPage page})
        {
            return page.GetHasNavigationBar();
        }
        return false;
    }

    public override IEnumerable<Control> GetPrimaryTopBarCommands()
    {
        List<Control> frameworkElements = new List<Control>();
        frameworkElements.AddRange(this.PrimaryTopBarCommands);

        if (ContentControl != null && ContentControl.Content is AvaloniaDynamicContentPage page)
        {
            frameworkElements.AddRange(page.GetPrimaryTopBarCommands());
        }

        return frameworkElements;
    }

    public override IEnumerable<Control> GetSecondaryTopBarCommands()
    {
        List<Control> frameworkElements = new List<Control>();
        frameworkElements.AddRange(this.SecondaryTopBarCommands);

        if (ContentControl != null && ContentControl.Content is AvaloniaDynamicContentPage page)
        {
            frameworkElements.AddRange(page.GetSecondaryTopBarCommands());
        }

        return frameworkElements;
    }

    public override IEnumerable<Control> GetPrimaryBottomBarCommands()
    {
        List<Control> frameworkElements = new List<Control>();
        frameworkElements.AddRange(this.PrimaryBottomBarCommands);

        if (ContentControl != null && ContentControl.Content is AvaloniaDynamicContentPage page)
        {
            frameworkElements.AddRange(page.GetPrimaryBottomBarCommands());
        }

        return frameworkElements;
    }

    public override IEnumerable<Control> GetSecondaryBottomBarCommands()
    {
        List<Control> frameworkElements = new List<Control>();
        frameworkElements.AddRange(this.SecondaryBottomBarCommands);

        if (ContentControl != null && ContentControl.Content is AvaloniaDynamicContentPage page)
        {
            frameworkElements.AddRange(page.GetSecondaryBottomBarCommands());
        }

        return frameworkElements;
    }


    private void OnSelectedItemPropertyChanged(AvaloniaPropertyChangedEventArgs e) => OnSelectedItemChanged(e.OldValue, e.NewValue);

    protected virtual void OnSelectedItemChanged(object? oldValue, object? newValue)
    {
        if (ItemsSource == null) return;
        SelectedIndex = ItemsSource.Cast<object>().ToList().IndexOf(newValue);
        SelectionChanged?.Invoke(this, new AvaloniaSelectionChangedEventArgs(oldValue, newValue));
    }

    protected override void OnLayoutUpdated(object? sender, EventArgs e) => ContentLoader?.OnSizeContentChanged(this, SelectedItem);
}