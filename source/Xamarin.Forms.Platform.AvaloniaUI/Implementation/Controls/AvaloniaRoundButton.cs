using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaRoundButton : Avalonia.Controls.Button
{
    public static readonly StyledProperty<int> CornerRadiusProperty = AvaloniaProperty.Register<AvaloniaRoundButton, int>(nameof(CornerRadius));

    static AvaloniaRoundButton()
    {
        CornerRadiusProperty.Changed.AddClassHandler<AvaloniaRoundButton>((x, e) => x.OnCornerRadiusPropertyChanged(e));
    }

    protected override Type StyleKeyOverride => typeof(Avalonia.Controls.Button);

    public int CornerRadius
    {
        get => GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    Avalonia.Controls.Presenters.ContentPresenter? contentPresenter;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        contentPresenter = VisualChildren.OfType<Avalonia.Controls.Presenters.ContentPresenter>().FirstOrDefault();
        UpdateCornerRadius();
    }

    private void OnCornerRadiusPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateCornerRadius();
    }

    void UpdateCornerRadius()
    {
        if (contentPresenter != null)
        {
            contentPresenter.CornerRadius = new Avalonia.CornerRadius(CornerRadius);
        }
    }
}