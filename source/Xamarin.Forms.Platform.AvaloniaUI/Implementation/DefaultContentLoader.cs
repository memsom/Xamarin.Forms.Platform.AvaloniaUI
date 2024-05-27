using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public class DefaultContentLoader : IContentLoader
{
    public Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken)
    {
        if (!Avalonia.Application.Current.CheckAccess())
        {
            throw new InvalidOperationException("UIThreadRequired");
        }

        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        return Task.Factory.StartNew(() => LoadContent(newContent), cancellationToken, TaskCreationOptions.None, scheduler);
    }

    protected virtual object? LoadContent(object content)
    {
        if (content is Control)
        {
            return content;
        }

        var uri = content as Uri;
        if (content is string)
        {
            Uri.TryCreate(content as string, UriKind.RelativeOrAbsolute, out uri);
        }
        if (uri != null)
        {
            return AvaloniaXamlLoader.Load(uri);
        }
        return null;
    }

    public void OnSizeContentChanged(Control parent, object content)
    {
    }
}