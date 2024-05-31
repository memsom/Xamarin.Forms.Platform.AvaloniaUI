using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportCell(typeof(Cell), typeof(TextCellRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class TextCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
    {
        var textCell = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["TextCell"];
        if (cell.RealParent is ListView)
        {
            if (cell.GetIsGroupHeader<ItemsView<Cell>, Cell>())
            {
                return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ListViewHeaderTextCell"];
            }

            if (global::Avalonia.Application.Current.Resources.ContainsKey("ListViewTextCell"))
            {
                return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ListViewTextCell"];
            }
        }

        return textCell;
    }
}