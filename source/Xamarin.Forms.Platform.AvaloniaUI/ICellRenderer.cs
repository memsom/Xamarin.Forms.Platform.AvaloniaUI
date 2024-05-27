namespace Xamarin.Forms.Platform.AvaloniaUI;

public interface ICellRenderer : IRegisterable
{
    global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell);
}