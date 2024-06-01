using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

[assembly: ExportCell(typeof(TextCell), typeof(TextCellRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class TextCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
    {
        //var textCell = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["TextCell"];
        var textCell = (global::Avalonia.Markup.Xaml.Templates.DataTemplate)XFAvaloniaUI.Instance["TextCell"];
        textCell.DataType = typeof(TextCell);

        if (cell.RealParent is ListView)
        {
            if (cell.GetIsGroupHeader<ItemsView<Cell>, Cell>())
            {
                //return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ListViewHeaderTextCell"];
                return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)XFAvaloniaUI.Instance["ListViewHeaderTextCell"];
            }

            if (XFAvaloniaUI.Instance.ContainsKey("ListViewTextCell"))
            {
                //return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)global::Avalonia.Application.Current.Resources["ListViewTextCell"];
                return (global::Avalonia.Markup.Xaml.Templates.DataTemplate)XFAvaloniaUI.Instance["ListViewTextCell"];
            }
        }

        return textCell;
    }
}