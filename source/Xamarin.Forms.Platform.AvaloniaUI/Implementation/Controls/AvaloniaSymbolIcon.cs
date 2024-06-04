using Avalonia;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaSymbolIcon : AvaloniaElementIcon
{
    public static readonly StyledProperty<Symbol> SymbolProperty = AvaloniaProperty.Register<AvaloniaSymbolIcon, Symbol>(nameof(Symbol));

    public Symbol Symbol
    {
        get => GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }
}