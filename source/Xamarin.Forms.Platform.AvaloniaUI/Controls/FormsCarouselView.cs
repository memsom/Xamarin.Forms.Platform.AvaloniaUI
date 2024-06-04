using System.Reactive;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using ReactiveUI;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsCarouselView : FormsMultiView
{
    public ReactiveCommand<Unit, Unit> NextCommand { get; }
    public ReactiveCommand<Unit, Unit> PreviousCommand { get; }

    private RepeatButton? nextButton;
    private RepeatButton? previousButton;

    public FormsCarouselView()
    {
        SelectedIndexProperty.Changed.AddClassHandler<FormsCarouselView>((x, e) => x.OnSelectedIndexChanged(e));

        NextCommand = ReactiveCommand.Create(OnNextExecuted, OnNextCanExecute());
        PreviousCommand = ReactiveCommand.Create(OnPreviousExecuted, OnPreviousCanExecute());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        nextButton = e.NameScope.Find<RepeatButton>("PART_NextButton");
        nextButton.Click += NextButton_Click;
        previousButton = e.NameScope.Find<RepeatButton>("PART_PreviousButton");
        previousButton.Click += PreviousButton_Click;
    }

    private void NextButton_Click(object? sender, RoutedEventArgs e) => OnNextExecuted();

    private void PreviousButton_Click(object? sender, RoutedEventArgs e) => OnPreviousExecuted();

    protected override void Appearing()
    {
        base.Appearing();
        SelectedIndex = 0;
    }

    private IObservable<bool> OnPreviousCanExecute()
    {
        return this.WhenAnyValue(x => x.SelectedIndex, (selectedIndex) => selectedIndex > 0);
    }

    private void OnPreviousExecuted()
    {
        if (SelectedIndex > 0)
        {
            SelectedIndex -= 1;
            ContentControl.Transition = TransitionType.Right;
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
        }
    }

    private void OnSelectedIndexChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (ItemsSource == null) return;
        var items = ItemsSource.Cast<object>();

        if ((int)e.NewValue >= 0 && (int)e.NewValue < items.Count())
        {
            SelectedItem = items.ElementAt((int)e.NewValue);
        }
    }
}