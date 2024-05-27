using System.Net;
using Avalonia.Media.Imaging;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Handlers;

[assembly: ExportImageSourceHandler(typeof(UriImageSource), typeof(UriImageSourceHandler))]
namespace Xamarin.Forms.Platform.AvaloniaUI.Handlers;

public sealed class UriImageSourceHandler : IImageSourceHandler
{
    public Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesoure, CancellationToken cancelationToken = new CancellationToken())
    {
        Bitmap bitmapimage = null;
        var imageLoader = imagesoure as UriImageSource;
        if (imageLoader?.Uri != null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(imageLoader?.Uri);
            var responseStream = webRequest.GetResponse().GetResponseStream();
            byte[] buffer = new byte[1024];
            var memoryStream = new MemoryStream();
            while (true)
            {
                int read = responseStream.Read(buffer, 0, buffer.Length);
                if (read <= 0) break;
                memoryStream.Write(buffer, 0, read);
            }
            memoryStream.Position = 0;
            bitmapimage = new Bitmap(memoryStream);
        }
        return Task.FromResult<Bitmap>(bitmapimage);
    }
}