using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportCell(typeof(EntryCell), typeof(EntryCellRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class EntryCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell) { return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["EntryCell"]; }
}