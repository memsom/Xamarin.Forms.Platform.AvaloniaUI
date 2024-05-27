using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Styling;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class RoundButton : Avalonia.Controls.Button, IStyleable
{
    public static readonly StyledProperty<int> CornerRadiusProperty = AvaloniaProperty.Register<RoundButton, int>(nameof(CornerRadius));

    static RoundButton()
    {
        CornerRadiusProperty.Changed.AddClassHandler<RoundButton>((x, e) => x.OnCornerRadiusPropertyChanged(e));
    }

    Type IStyleable.StyleKey => typeof(Avalonia.Controls.Button);

    public int CornerRadius
    {
        get
        {
            return (int)GetValue(CornerRadiusProperty);
        }
        set
        {
            SetValue(CornerRadiusProperty, value);
        }
    }

    Avalonia.Controls.Presenters.ContentPresenter _contentPresenter;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _contentPresenter = VisualChildren.OfType<Avalonia.Controls.Presenters.ContentPresenter>().FirstOrDefault();
        UpdateCornerRadius();
    }

    private void OnCornerRadiusPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        UpdateCornerRadius();
    }

    void UpdateCornerRadius()
    {
        if (_contentPresenter != null)
        {
            _contentPresenter.CornerRadius = new Avalonia.CornerRadius(CornerRadius);
        }
    }
}