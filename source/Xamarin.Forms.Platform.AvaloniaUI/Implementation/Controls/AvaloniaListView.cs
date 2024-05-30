using Avalonia;
using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaListView : ListBox
{
    public static readonly StyledProperty<object> ItemTemplateSelectorProperty = AvaloniaProperty.Register<AvaloniaListView, object>(nameof(ItemTemplateSelector));

    static AvaloniaListView()
    {
    }

    protected override Type StyleKeyOverride => typeof(ListBox);

    public object ItemTemplateSelector
    {
        get => GetValue(ItemTemplateSelectorProperty);
        set => SetValue(ItemTemplateSelectorProperty, value);
    }

    public AvaloniaListView()
    {
    }
}