namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class SwitchCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell) { return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["SwitchCell"]; }
}