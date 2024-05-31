using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportCell(typeof(ViewCell), typeof(ViewCellRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ViewCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell) { return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ViewCell"]; }
}