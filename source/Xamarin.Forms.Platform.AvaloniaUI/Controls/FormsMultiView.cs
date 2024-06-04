using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsMultiView : UserControl
{
    public AvaloniaTransitioningContentControl? ContentControl { get; private set; }

    public event EventHandler<AvaloniaSelectionChangedEventArgs>? SelectionChanged;

    public static readonly StyledProperty<IContentLoader> ContentLoaderProperty = AvaloniaProperty.Register<FormsMultiView, IContentLoader>(nameof(ContentLoader), new DefaultContentLoader());
    public static readonly StyledProperty<ObservableCollection<object>?> ItemsSourceProperty = AvaloniaProperty.Register<FormsMultiView, ObservableCollection<object>?>(nameof(ItemsSource));
    public static readonly StyledProperty<object> SelectedItemProperty = AvaloniaProperty.Register<FormsMultiView, object>(nameof(SelectedItem));
    public static readonly StyledProperty<int> SelectedIndexProperty = AvaloniaProperty.Register<FormsMultiView, int>(nameof(SelectedIndex), 0);

    public IContentLoader ContentLoader
    {
        get => GetValue(ContentLoaderProperty);
        set => SetValue(ContentLoaderProperty, value);
    }

    public ObservableCollection<object>? ItemsSource
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

    protected FormsMultiView()
    {
        LayoutUpdated += OnLayoutUpdated;

        AttachedToVisualTree += OnAttachedToVisualTree;
        DetachedFromVisualTree += OnDetachedToVisualTree;

        SelectedItemProperty.Changed.AddClassHandler<FormsMultiView>((x, e) => x.OnSelectedItemChanged(e));
        SetValue(ItemsSourceProperty, new ObservableCollection<object>());
    }

    private void OnDetachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e) { Disappearing(); }

    private void OnAttachedToVisualTree(object? sender, VisualTreeAttachmentEventArgs e) { Appearing(); }

    protected virtual void OnLayoutUpdated(object? sender, EventArgs e) => OnContentLoaderLayoutUpdated(this, e);

    protected virtual void Appearing()
    {
    }

    protected virtual void Disappearing()
    {
    }

    protected virtual void OnSelectedItemChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (e.OldValue == e.NewValue) return;
        OnSelectedItemChanged(e.OldValue, e.NewValue);
        OnContentLoaderLayoutUpdated(this, e);
    }

    private void OnSelectedItemChanged(object? oldValue, object? newValue)
    {
        if (ItemsSource == null) return;
        SelectedIndex = ItemsSource.ToList().IndexOf(newValue);
        SelectionChanged?.Invoke(this, new AvaloniaSelectionChangedEventArgs(oldValue, newValue));
    }

    protected virtual void OnContentLoaderLayoutUpdated(object sender, EventArgs e)
    {
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        ContentControl = e.NameScope.Find<AvaloniaTransitioningContentControl>("PART_Multi_Content");
    }
}