using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsCommandBar : AvaloniaAppBar
{
    public static readonly StyledProperty<IEnumerable<Control>> PrimaryCommandsProperty = AvaloniaProperty.Register<FormsCommandBar, IEnumerable<Control>>(nameof(PrimaryCommands));
    public static readonly StyledProperty<IEnumerable<Control>> SecondaryCommandsProperty = AvaloniaProperty.Register<FormsCommandBar, IEnumerable<Control>>(nameof(SecondaryCommands));
    public static readonly StyledProperty<bool> IsDynamicOverflowEnabledProperty = AvaloniaProperty.Register<FormsCommandBar, bool>(nameof(IsDynamicOverflowEnabled));

    static FormsCommandBar()
    {
    }


    public IEnumerable<Control> PrimaryCommands
    {
        get => GetValue(PrimaryCommandsProperty);
        set => SetValue(PrimaryCommandsProperty, value);
    }

    public IEnumerable<Control> SecondaryCommands
    {
        get => GetValue(SecondaryCommandsProperty);
        set => SetValue(SecondaryCommandsProperty, value);
    }

    public bool IsDynamicOverflowEnabled
    {
        get => GetValue(IsDynamicOverflowEnabledProperty);
        set => SetValue(IsDynamicOverflowEnabledProperty, value);
    }

    Button? moreButton;
    ItemsControl? primaryItemsControl;
    bool isInValidLocation;

    // Set by the container if the container is a valid place to show a toolbar.
    // This exists to provide consistency with the other platforms; we've got
    // rules in place that limit toolbars to Navigation Page and to Tabbed
    // and Master-Detail Pages when they're currently displaying a Navigation Page
    public bool IsInValidLocation
    {
        get => isInValidLocation;
        set
        {
            isInValidLocation = value;
            UpdateVisibility();
        }
    }


    public FormsCommandBar() => UpdateVisibility();

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        moreButton = e.NameScope.Find<Button>("MoreButton");
        primaryItemsControl = e.NameScope.Find<ItemsControl>("PrimaryItemsControl");
    }

    public void Reset()
    {
    }

    void UpdateVisibility()
    {
        // Determine whether we have a title (or some other content) inside this command bar
        var frameworkElement = Content as Control;

        // Apply the rules for consistency with other platforms

        // Not in one of the acceptable toolbar locations from the other platforms
        if (!IsInValidLocation)
        {
            // If there's no title to display (e.g., toolbarplacement is set to bottom)
            // or the title is collapsed (e.g., because it's empty)
            if (frameworkElement == null || !frameworkElement.IsVisible)
            {
                // Just collapse the whole thing
                IsVisible = false;
                return;
            }

            // The title needs to be visible, but we're not allowed to show a toolbar
            // So we need to hide the toolbar items

            IsVisible = true;

            if (moreButton != null)
            {
                moreButton.IsVisible = false;
            }

            if (primaryItemsControl != null)
            {
                primaryItemsControl.IsVisible = false;
            }

            return;
        }

        // We're in one of the acceptable toolbar locations from the other platforms so the normal rules apply

        if (primaryItemsControl != null)
        {
            // This is normally visible by default, but it might have been collapsed by the toolbar consistency rules above
            primaryItemsControl.IsVisible = true;
        }

        // Are there any commands to display?
        var visibility = PrimaryCommands.Count() + SecondaryCommands.Count() > 0;

        if (moreButton != null)
        {
            // The "..." button should only be visible if we have commands to display
            moreButton.IsVisible = visibility;

            // There *is* an OverflowButtonVisibility property that does more or less the same thing,
            // but it became available in 10.0.14393.0 and we have to support 10.0.10240
        }

        if (frameworkElement is {IsVisible: true})
        {
            // If there's a title to display, we have to be visible whether or not we have commands
            IsVisible = true;
        }
        else
        {
            // Otherwise, visibility depends on whether we have commands
            IsVisible = visibility;
        }
    }

}