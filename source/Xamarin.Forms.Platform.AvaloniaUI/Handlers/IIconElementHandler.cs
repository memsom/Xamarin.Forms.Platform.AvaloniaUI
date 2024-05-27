using Avalonia.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Handlers;

public interface IIconElementHandler : IRegisterable
{
    Task<IconElement> LoadIconElementAsync(ImageSource imagesource, CancellationToken cancellationToken = default(CancellationToken));
}