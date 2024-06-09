using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;
using AvaloniaButton = Avalonia.Controls.Button;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class TitleViewManager
{
    readonly ITitleViewRendererController titleViewRendererController;

    object TitleView => titleViewRendererController.TitleViewContent;
    FormsCommandBar CommandBar => titleViewRendererController.CommandBar;
    Control TitleViewPresenter => titleViewRendererController.TitleViewPresenter;

    public TitleViewManager(ITitleViewRendererController titleViewRendererController)
    {
        this.titleViewRendererController = titleViewRendererController;
        this.titleViewRendererController.TitleViewPresenter.AttachedToVisualTree += OnTitleViewPresenterLoaded;

        CommandBar.LayoutUpdated += CommandLayoutUpdated;
        CommandBar.Unloaded += CommandBarUnloaded;
    }

    internal void OnTitleViewPropertyChanged() { UpdateTitleViewWidth(); }

    void OnTitleViewPresenterLoaded(object? sender, EventArgs e)
    {
        UpdateTitleViewWidth();
        TitleViewPresenter.AttachedToVisualTree -= OnTitleViewPresenterLoaded;
    }

    void CommandBarUnloaded(object? sender, EventArgs e)
    {
        CommandBar.LayoutUpdated -= CommandLayoutUpdated;
        CommandBar.Unloaded -= CommandBarUnloaded;
    }

    void CommandLayoutUpdated(object? sender, object e) { UpdateTitleViewWidth(); }

    void UpdateTitleViewWidth()
    {
        if (TitleView == null || TitleViewPresenter == null || CommandBar == null) return;

        if (CommandBar.Width <= 0) return;

        double buttonWidth = 0;
        foreach (var item in CommandBar.GetDescendantsByName<AvaloniaButton>("MoreButton"))
            if (item.IsVisible)
                buttonWidth += item.Width;

        if (!CommandBar.IsDynamicOverflowEnabled)
            foreach (var item in CommandBar.GetDescendantsByName<ItemsControl>("PrimaryItemsControl"))
                buttonWidth += item.Width;

        TitleViewPresenter.Width = CommandBar.Width - buttonWidth;
        UpdateVisibility();
    }

    void UpdateVisibility()
    {
        if (TitleView == null)
            titleViewRendererController.TitleViewVisibility = false;
        else
            titleViewRendererController.TitleViewVisibility = true;
    }
}