using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public interface IContentLoader
{
    Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken);

    // TODO:
    void OnSizeContentChanged(Control parent, object content);
}