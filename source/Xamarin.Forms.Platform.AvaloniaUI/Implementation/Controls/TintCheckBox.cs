using Avalonia;
using Avalonia.Media;
using Avalonia.Styling;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class TintCheckBox : Avalonia.Controls.CheckBox, IStyleable
{
    public static readonly StyledProperty<Avalonia.Media.Brush> TintBrushProperty = AvaloniaProperty.Register<TintCheckBox, Avalonia.Media.Brush>(nameof(TintBrush));

    static TintCheckBox()
    {
        TintBrushProperty.Changed.AddClassHandler<TintCheckBox>((x, e) => x.OnTintBrushPropertyChanged(e));
    }

    Type IStyleable.StyleKey => typeof(CheckBox);

    public Avalonia.Media.Brush TintBrush
    {
        get => GetValue(TintBrushProperty);
        set => SetValue(TintBrushProperty, value);
    }

    public TintCheckBox()
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