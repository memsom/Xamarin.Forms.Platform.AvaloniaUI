using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using AvaloniaButton = Avalonia.Controls.Button;


namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsStepper : Border, IDisposable
{
    readonly AvaloniaButton downButton;
    readonly AvaloniaButton upButton;

    public FormsStepper()
    {
        upButton = new() {Content = "+", Width = 100};
        upButton.Click += UpButtonOnClick;

        downButton = new() {Content = "-", Width = 100};
        downButton.Click += DownButtonOnClick;

        var panel = new StackPanel
        {
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Orientation = Orientation.Horizontal,
        };

        panel.Children.Add(downButton);
        panel.Children.Add(upButton);

        Child = panel;
    }

    public EventHandler<RoutedEventArgs>? DownClicked;
    public EventHandler<RoutedEventArgs>? UpClicked;

    void DownButtonOnClick(object? sender, RoutedEventArgs routedEventArgs) => DownClicked?.Invoke(this, routedEventArgs);

    void UpButtonOnClick(object? sender, RoutedEventArgs routedEventArgs) => UpClicked?.Invoke(this, routedEventArgs);

    public bool IsUpEnabled
    {
        get => upButton.IsEnabled;
        set => upButton.IsEnabled = value;
    }

    public bool IsDownEnabled
    {
        get => downButton.IsEnabled;
        set => downButton.IsEnabled = value;
    }

    bool isDisposed;

    private void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
                upButton.Click -= UpButtonOnClick;
                downButton.Click -= DownButtonOnClick;
        }

        isDisposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
    }
}