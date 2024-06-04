using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using ReactiveUI;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaCarouselPage : AvaloniaMultiContentPage
{
    protected override Type StyleKeyOverride => typeof(AvaloniaCarouselPage);

    public RepeatButton? NextButton { get; private set; }
    public RepeatButton? PreviousButton { get; private set; }

    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        if (e.Property == SelectedIndexProperty)
        {
            OnSelectedIndexChanged(e);
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        NextButton = e.NameScope.Find<RepeatButton>("PART_NextButton");
        NextButton.Click += NextButton_Click;
        PreviousButton = e.NameScope.Find<RepeatButton>("PART_PreviousButton");
        PreviousButton.Click += PreviousButton_Click;
    }

    private void PreviousButton_Click(object? sender, RoutedEventArgs e) => OnPreviousExecuted();

    private void NextButton_Click(object? sender, RoutedEventArgs e) => OnNextExecuted();

    protected override void Appearing()
    {
        base.Appearing();
        SelectedIndex = 0;
    }

    private IObservable<bool> OnPreviousCanExecute()
    {
        return this.WhenAnyValue(x => x.SelectedIndex, selectedIndex => selectedIndex > 0);
    }

    private void OnPreviousExecuted()
    {
        if (SelectedIndex > 0)
        {
            SelectedIndex -= 1;
            ContentControl.Transition = TransitionType.Right;
            ResetTransition();
        }
    }

    private IObservable<bool> OnNextCanExecute()
    {
        return this.WhenAnyValue(x => x.ItemsSource, x => x.SelectedIndex, (itemsSource, selectedIndex) => selectedIndex < (itemsSource.Cast<object>().Count() - 1));
    }

    private void OnNextExecuted()
    {
        if (SelectedIndex < ItemsSource.Count - 1)
        {
            SelectedIndex += 1;
            ContentControl.Transition = TransitionType.Left;
            ResetTransition();
        }
    }

    private void OnSelectedIndexChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (ItemsSource == null)
        {
            return;
        }

        var items = ItemsSource.Cast<object>();

        if ((int)e.NewValue >= 0 && (int)e.NewValue < items.Count())
        {
            SelectedItem = items.ElementAt((int)e.NewValue);
        }
    }

    protected virtual void ResetTransition()
    {
        DispatcherTimer.RunOnce(() =>
        {
            ContentControl.Transition = TransitionType.Default;
        }, TimeSpan.FromSeconds(0.2));
    }
}