using Avalonia;
using Avalonia.Media;
using AvaloniaCheckBox = Avalonia.Controls.CheckBox;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaTintCheckBox : AvaloniaCheckBox
{
    public static readonly StyledProperty<Avalonia.Media.Brush> TintBrushProperty = AvaloniaProperty.Register<AvaloniaTintCheckBox, Avalonia.Media.Brush>(nameof(TintBrush));

    static AvaloniaTintCheckBox()
    {
        TintBrushProperty.Changed.AddClassHandler<AvaloniaTintCheckBox>((x, e) => x.OnTintBrushPropertyChanged(e));
    }

    protected override Type StyleKeyOverride => typeof(AvaloniaCheckBox);

    public Avalonia.Media.Brush TintBrush
    {
        get => GetValue(TintBrushProperty);
        set => SetValue(TintBrushProperty, value);
    }

    public AvaloniaTintCheckBox()
    {
        BorderBrush = new Avalonia.Media.SolidColorBrush(Colors.Black);
    }

    protected void OnTintBrushPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var checkBox = this;

        if (e.NewValue is SolidColorBrush {Color.A: 0})
        {
            checkBox.BorderBrush = new Avalonia.Media.SolidColorBrush(Colors.Black);
        }
        else if (e.NewValue is Avalonia.Media.SolidColorBrush b)
        {
            checkBox.BorderBrush = b;
        }
    }
}