namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public interface ICellRenderer : IRegisterable
{
    global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell);
}