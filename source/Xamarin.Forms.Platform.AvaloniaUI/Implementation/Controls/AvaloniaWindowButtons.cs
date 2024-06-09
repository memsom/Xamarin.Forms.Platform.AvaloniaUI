using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using AvaloniaButton = Avalonia.Controls.Button;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaWindowButtons : ContentControl
{
    private AvaloniaButton? min;
    private AvaloniaButton? max;
    private AvaloniaButton? close;

    private ApplicationWindow _parentWindow;
    public ApplicationWindow ParentWindow
    {
        get { return _parentWindow; }
        set
        {
            _parentWindow = value;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        close = e.NameScope.Find<AvaloniaButton>("PART_Close");
        if (close != null)
        {
            close.Click += CloseClick;
        }

        max = e.NameScope.Find<AvaloniaButton>("PART_Max");
        if (max != null)
        {
            max.Click += MaximizeClick;
        }

        min = e.NameScope.Find<AvaloniaButton>("PART_Min");
        if (min != null)
        {
            min.Click += MinimizeClick;
        }
        this.ParentWindow = this.TryFindParent<ApplicationWindow>();
    }

    private void MinimizeClick(object? sender, RoutedEventArgs e)
    {
        if (null == this.ParentWindow) return;
        SystemCommands.MinimizeWindow(this.ParentWindow);
    }

    private void MaximizeClick(object? sender, RoutedEventArgs e)
    {
        if (null == this.ParentWindow) return;
        if (this.ParentWindow.WindowState == WindowState.Maximized)
        {
            SystemCommands.RestoreWindow(this.ParentWindow);
        }
        else
        {
            SystemCommands.MaximizeWindow(this.ParentWindow);
        }
    }

    private void CloseClick(object? sender, RoutedEventArgs e)
    {
        if (null == this.ParentWindow) return;
        this.ParentWindow.Close();
    }
}