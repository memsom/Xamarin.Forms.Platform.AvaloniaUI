using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaApplication = Avalonia.Application;
using AvaloniaDataTemplate = Avalonia.Markup.Xaml.Templates.DataTemplate;

namespace Xamarin.Forms.Platform.AvaloniaUI.TemplateSelectors;

public class TableViewDataTemplateSelector : IDataTemplateSelector
{
    public IDataTemplate SelectTemplate(object item, object container)
    {
        if (item is Cell)
        {
            return AvaloniaApplication.Current!.FindResource("CellTemplate") as AvaloniaDataTemplate;
        }

        return AvaloniaApplication.Current!.FindResource("TableSectionHeader") as AvaloniaDataTemplate;
    }
}