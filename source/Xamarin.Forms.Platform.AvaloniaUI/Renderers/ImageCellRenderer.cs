using Avalonia.Controls;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using AvaloniaApplication = Avalonia.Application;
using AvaloniaDataTemplate = Avalonia.Markup.Xaml.Templates.DataTemplate;

[assembly: ExportCell(typeof(ImageCell), typeof(ImageCellRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class ImageCellRenderer : ICellRenderer
{
    public virtual global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell) =>
        AvaloniaApplication.Current!.FindResource("ImageCell") as AvaloniaDataTemplate;
}