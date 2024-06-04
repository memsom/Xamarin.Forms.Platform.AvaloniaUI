using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Extensions;
using AvaloniaBrush = Avalonia.Media.Brush;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaMasterDetailPage : AvaloniaDynamicContentPage
{
    public static readonly StyledProperty<object> MasterPageProperty = AvaloniaProperty.Register<AvaloniaMasterDetailPage, object>(nameof(MasterPage));
    public static readonly StyledProperty<object> DetailPageProperty = AvaloniaProperty.Register<AvaloniaMasterDetailPage, object>(nameof(DetailPage));
    public static readonly StyledProperty<bool> IsPresentedProperty = AvaloniaProperty.Register<AvaloniaMasterDetailPage, bool>(nameof(IsPresented));

    static AvaloniaMasterDetailPage()
    {
        IsPresentedProperty.Changed.AddClassHandler<AvaloniaMasterDetailPage>((x, e) => x.OnIsPresentedPropertyChanged(e));
    }

    protected override Type StyleKeyOverride => typeof(AvaloniaMasterDetailPage);

    public object MasterPage
    {
        get => GetValue(MasterPageProperty);
        set => SetValue(MasterPageProperty, value);
    }

    public object DetailPage
    {
        get => GetValue(DetailPageProperty);
        set => SetValue(DetailPageProperty, value);
    }

    public bool IsPresented
    {
        get => GetValue(IsPresentedProperty);
        set => SetValue(IsPresentedProperty, value);
    }

    public AvaloniaDynamicContentControl? MasterContentControl { get; private set; }
    public AvaloniaDynamicContentControl? DetailContentControl  { get; private set; }
    public Grid? GridContainer { get; private set; }

    public double? MasterColumnWidth { get; private set; }

    private void OnIsPresentedPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (GridContainer != null)
        {
            if ((bool)e.NewValue)
            {
                if ((GridContainer.ColumnDefinitions[0].Width.Value <= 0) && (MasterColumnWidth ?? 0) > 0)
                {
                    GridContainer.ColumnDefinitions[0].Width = new GridLength((double)MasterColumnWidth);
                }
            }
            else
            {
                var value = GridContainer.ColumnDefinitions[0]?.Width.Value;
                if (value > 0)
                {
                    MasterColumnWidth = value;
                }
                GridContainer.ColumnDefinitions[0].Width = new GridLength(0);
            }
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        MasterContentControl = e.NameScope.Find<AvaloniaDynamicContentControl>("PART_Master");
        DetailContentControl = e.NameScope.Find<AvaloniaDynamicContentControl>("PART_Detail_Content");

        GridContainer = e.NameScope.Find<Grid>("PART_Container");

        MasterColumnWidth = GridContainer?.ColumnDefinitions[0]?.Width.Value;
        if (!IsPresented)
        {
            GridContainer.ColumnDefinitions[0].Width = new GridLength(0);
        }
    }


    public override string GetTitle()
    {
        if (DetailContentControl?.Content is AvaloniaDynamicContentPage page)
        {
            return page.GetTitle();
        }
        return Title;
    }

    public override AvaloniaBrush GetTitleBarBackgroundColor()
    {
        if (DetailContentControl?.Content is AvaloniaDynamicContentPage page)
        {
            return page.GetTitleBarBackgroundColor();
        }
        return TitleBarBackgroundColor;
    }

    public override AvaloniaBrush GetTitleBarTextColor()
    {
        if (DetailContentControl?.Content is AvaloniaDynamicContentPage page)
        {
            return page.GetTitleBarTextColor();
        }
        return TitleBarTextColor;
    }

    public override IEnumerable<Control> GetPrimaryTopBarCommands() =>
        PrimaryTopBarCommands.Merge(MasterContentControl, page => page.GetPrimaryTopBarCommands()).Merge(DetailContentControl, page => page.GetPrimaryTopBarCommands());

    public override IEnumerable<Control> GetSecondaryTopBarCommands() =>
        SecondaryTopBarCommands.Merge(MasterContentControl, page => page.GetSecondaryTopBarCommands()).Merge(DetailContentControl, page => page.GetSecondaryTopBarCommands());

    public override IEnumerable<Control> GetPrimaryBottomBarCommands() =>
        PrimaryBottomBarCommands.Merge(MasterContentControl, page => page.GetPrimaryBottomBarCommands()).Merge(DetailContentControl, page => page.GetPrimaryBottomBarCommands());

    public override IEnumerable<Control> GetSecondaryBottomBarCommands() =>
        SecondaryBottomBarCommands.Merge(MasterContentControl, page => page.GetSecondaryBottomBarCommands()).Merge(DetailContentControl, page => page.GetSecondaryBottomBarCommands());
}