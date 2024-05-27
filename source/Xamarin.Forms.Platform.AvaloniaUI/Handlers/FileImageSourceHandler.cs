using Avalonia.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Handlers;

[assembly: ExportImageSourceHandler(typeof(FileImageSource), typeof(FileImageSourceHandler))]
namespace Xamarin.Forms.Platform.AvaloniaUI.Handlers;

public sealed class FileImageSourceHandler : IImageSourceHandler
{
    public Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancelationToken = new CancellationToken())
    {
        global::Avalonia.Media.Imaging.Bitmap image = null;
        FileImageSource filesource = imagesoure as FileImageSource;
        if (filesource != null)
        {
            string file = filesource.File;
            image = new Bitmap(file);
        }
        return Task.FromResult(image);
    }
}