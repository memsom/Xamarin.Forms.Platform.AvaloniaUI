using Avalonia.Controls.Templates;

namespace Xamarin.Forms.Platform.AvaloniaUI.TemplateSelectors;

public interface IDataTemplateSelector
{
    IDataTemplate SelectTemplate(object item, object container);
}