using Avalonia.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using AvaloniaApplication = Avalonia.Application;
using AvaloniaDataTemplate = Avalonia.Markup.Xaml.Templates.DataTemplate;

[assembly: ExportCell(typeof(Cell), typeof(TextCellRenderer))]
[assembly: ExportCell(typeof(TextCell), typeof(TextCellRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class TextCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
    {
        var textCell = AvaloniaApplication.Current!.FindResource("TextCell") as AvaloniaDataTemplate;
        //textCell.DataType = typeof(TextCell);

        if (cell.RealParent is ListView)
        {
            if (cell.GetIsGroupHeader<ItemsView<Cell>, Cell>())
            {
                return AvaloniaApplication.Current!.FindResource("ListViewHeaderTextCell") as AvaloniaDataTemplate;
            }

            if (AvaloniaApplication.Current!.Resources.ContainsKey("ListViewTextCell"))
            {
                return AvaloniaApplication.Current!.FindResource("ListViewTextCell") as AvaloniaDataTemplate;
            }
        }

        return textCell;
    }
}