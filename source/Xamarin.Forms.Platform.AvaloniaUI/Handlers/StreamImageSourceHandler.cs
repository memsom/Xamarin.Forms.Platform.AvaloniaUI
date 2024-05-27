using Avalonia.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Handlers;

[assembly: ExportImageSourceHandler(typeof(StreamImageSource), typeof(StreamImageSourceHandler))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Handlers;

public sealed class StreamImageSourceHandler : IImageSourceHandler
{
    public async Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = new CancellationToken())
    {
        Bitmap bitmapImage = null;
        StreamImageSource streamImageSource = imagesource as StreamImageSource;

        if (streamImageSource != null && streamImageSource.Stream != null)
        {
            using (Stream stream = await ((IStreamImageSource)streamImageSource).GetStreamAsync(cancelationToken))
            {
                bitmapImage = new Bitmap(stream);
            }
        }

        return bitmapImage;
    }
}