using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;
using IAvaloniaNavigation = Xamarin.Forms.Platform.AvaloniaUI.Implementation.Navigation.INavigation;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaNavigationPage : AvaloniaDynamicContentPage, IAvaloniaNavigation
{
    public static readonly StyledProperty<object> CurrentPageProperty = AvaloniaProperty.Register<AvaloniaNavigationPage, object>(nameof(CurrentPage));

    static AvaloniaNavigationPage() { }

    protected override Type StyleKeyOverride => typeof(AvaloniaNavigationPage);

    public AvaloniaTransitioningContentControl? ContentControl { get; private set; }

    public ObservableCollection<object> InternalChildren { get; } = new();

    public object CurrentPage
    {
        get => GetValue(CurrentPageProperty);
        set => SetValue(CurrentPageProperty, value);
    }

    public AvaloniaNavigationPage() { }

    public AvaloniaNavigationPage(object root) : this() => Push(root);

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        ContentControl = e.NameScope.Find<AvaloniaTransitioningContentControl>("PART_Navigation_Content");
    }

    #region INavigation

    public int StackDepth => InternalChildren.Count;

    public void InsertPageBefore(object page, object before)
    {
        int index = InternalChildren.IndexOf(before);
        InternalChildren.Insert(index, page);
        ParentWindow?.SynchronizeAppBar();
    }

    public void RemovePage(object page)
    {
        if (InternalChildren.Remove(page))
        {
            if (ContentControl != null)
            {
                ContentControl.Transition = TransitionType.Normal;
            }

            CurrentPage = InternalChildren.Last();
        }

        ParentWindow?.SynchronizeAppBar();
    }

    public void Pop() => Pop(true);

    public void Pop(bool animated)
    {
        if (StackDepth <= 1) return;

        if (InternalChildren.Remove(InternalChildren.Last()))
        {
            if (ContentControl != null)
            {
                ContentControl.Transition = animated ? TransitionType.Right : TransitionType.Normal;
            }

            CurrentPage = InternalChildren.Last();
        }
    }

    public void PopToRoot() => PopToRoot(true);

    public void PopToRoot(bool animated)
    {
        if (StackDepth <= 1) return;

        object[] childrenToRemove = InternalChildren.Skip(1).ToArray();
        foreach (object child in childrenToRemove) InternalChildren.Remove(child);

        if (ContentControl != null)
        {
            ContentControl.Transition = animated ? TransitionType.Right : TransitionType.Normal;
        }

        CurrentPage = InternalChildren.Last();
    }

    public void Push(object page) => Push(page, true);

    public void Push(object page, bool animated)
    {
        InternalChildren.Add(page);
        if (ContentControl != null)
        {
            ContentControl.Transition = animated ? TransitionType.Left : TransitionType.Normal;
        }

        CurrentPage = page;
    }

    public void PopModal() => PopModal(true);

    public void PopModal(bool animated) => ParentWindow?.PopModal(animated);

    public void PushModal(object page) => PushModal(page, true);

    public void PushModal(object page, bool animated) => ParentWindow?.PushModal(page, animated);

    #endregion

    public override string GetTitle()
    {
        if (ContentControl is {Content: AvaloniaDynamicContentPage page})
        {
            return page.GetTitle();
        }

        return "";
    }

    public override bool GetHasNavigationBar()
    {
        if (ContentControl is {Content: AvaloniaDynamicContentPage page})
        {
            return page.GetHasNavigationBar();
        }

        return false;
    }

    public override IEnumerable<Control> GetPrimaryTopBarCommands() => PrimaryTopBarCommands.Merge(ContentControl, page => page.GetPrimaryTopBarCommands());

    public override IEnumerable<Control> GetSecondaryTopBarCommands() => SecondaryTopBarCommands.Merge(ContentControl, page => page.GetSecondaryTopBarCommands());

    public override IEnumerable<Control> GetPrimaryBottomBarCommands() => PrimaryBottomBarCommands.Merge(ContentControl, page => page.GetPrimaryBottomBarCommands());

    public override IEnumerable<Control> GetSecondaryBottomBarCommands() => SecondaryBottomBarCommands.Merge(ContentControl, page => page.GetSecondaryBottomBarCommands());

    public bool GetHasBackButton()
    {
        if (ContentControl is {Content: AvaloniaDynamicContentPage page})
        {
            return page.HasBackButton && StackDepth > 1;
        }

        return false;
    }

    public string GetBackButtonTitle()
    {
        if (StackDepth > 1)
        {
            return InternalChildren[StackDepth - 2].GetPropValue<string>("Title") ?? "Back";
        }

        return "";
    }

    public virtual void OnBackButtonPressed() => Pop();

    protected override void OnLayoutUpdated(object? sender, EventArgs e) { ContentLoader.OnSizeContentChanged(this, CurrentPage); }
}