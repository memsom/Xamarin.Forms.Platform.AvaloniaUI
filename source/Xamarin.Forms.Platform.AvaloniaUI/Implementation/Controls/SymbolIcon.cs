using Avalonia;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class SymbolIcon : ElementIcon
{
    public static readonly StyledProperty<Symbol> SymbolProperty = AvaloniaProperty.Register<SymbolIcon, Symbol>(nameof(Symbol));

    public Symbol Symbol
    {
        get => (Symbol)GetValue(SymbolProperty);
        set => SetValue(SymbolProperty, value);
    }
}