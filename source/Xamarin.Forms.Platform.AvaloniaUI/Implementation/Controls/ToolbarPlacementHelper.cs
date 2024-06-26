using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

internal class ToolbarPlacementHelper
{
    Border? bottomCommandBarArea;
    FormsCommandBar? commandBar;
    Func<ToolbarPlacement>? getToolbarPlacement;
    Border? titleArea;
    Border? topCommandBarArea;

    public void Initialize(FormsCommandBar newCommandBar, Func<ToolbarPlacement> getToolbarPlacementFunc,
        Func<string, AvaloniaObject> getTemplateChild)
    {
        this.commandBar = newCommandBar;
        this.getToolbarPlacement = getToolbarPlacementFunc;
        bottomCommandBarArea = getTemplateChild("BottomCommandBarArea") as Border;
        topCommandBarArea = getTemplateChild("TopCommandBarArea") as Border;
        titleArea = getTemplateChild("TitleArea") as Border;

        if (this.commandBar != null && bottomCommandBarArea != null && topCommandBarArea != null)
        {
            // We have to wait for the command bar to load so that it'll be in the control hierarchy
            // otherwise we can't properly move it to wherever the toolbar is supposed to be
            this.commandBar.Loaded += OnCommandBarOnLoaded;
        }
    }

    private void OnCommandBarOnLoaded(object? sender, RoutedEventArgs args)
    {
        UpdateToolbarPlacement();
        UpdateIsInValidLocation();
    }

    public void UpdateToolbarPlacement()
    {
        if (commandBar == null || getToolbarPlacement == null || bottomCommandBarArea == null ||
            topCommandBarArea == null)
        {
            // Template hasn't been applied yet, so we're not ready to update the toolbar placement
            return;
        }

        UpdateToolbarPlacement(commandBar, getToolbarPlacement(), bottomCommandBarArea, topCommandBarArea, titleArea);
    }

    static void UpdateToolbarPlacement(FormsCommandBar toolbar, ToolbarPlacement toolbarPlacement, Border? bottomCommandBarArea, Border? topCommandBarArea, Border? titleArea)
    {
        // Figure out what's hosting the command bar right now
        var current = toolbar.Parent as Border;

        // And figure out where it should be
        Border? target = null;

        switch (toolbarPlacement)
        {
            case ToolbarPlacement.Top:
                target = topCommandBarArea;
                break;
            case ToolbarPlacement.Bottom:
                target = bottomCommandBarArea;
                break;
            default:
                target = topCommandBarArea;
                break;
        }

        if (current == null || target == null || current == target)
        {
            return;
        }

        // Remove the command bar from its current host and add it to the new one
        current.Child = null;
        target.Child = toolbar;

        if (titleArea != null)
        {
            if (target == bottomCommandBarArea)
            {
                // If the title is hosted in the command bar and we're moving the command bar to the bottom,
                // put the title into the topCommandBarArea
                toolbar.Content = null;
                topCommandBarArea.Child = titleArea;
            }
            else
            {
                // Put the title back into the command bar
                toolbar.Content = titleArea;
            }
        }
    }

    // For the time being, keeping this logic for dealing with consistency between the platforms
    // re: toolbar visibility here; at some point we should be separating toolbars from navigation bars,
    // and this won't be necessary
    bool shouldShowToolBar;
    public bool ShouldShowToolBar
    {
        get => shouldShowToolBar;
        set
        {
            shouldShowToolBar = value;
            UpdateIsInValidLocation();
        }
    }

    void UpdateIsInValidLocation()
    {
        if (commandBar != null)
        {
            commandBar.IsInValidLocation = ShouldShowToolBar;
        }
    }
}