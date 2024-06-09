using Avalonia;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaAppBarButton : Avalonia.Controls.Button
{
    public static readonly StyledProperty<AvaloniaElementIcon> IconProperty = AvaloniaProperty.Register<AvaloniaAppBarButton, AvaloniaElementIcon>(nameof(Icon));
    public static readonly StyledProperty<string> LabelProperty = AvaloniaProperty.Register<AvaloniaAppBarButton, string>(nameof(Label));

    public AvaloniaElementIcon Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public string Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }
}