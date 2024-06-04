namespace Xamarin.Forms.Platform.AvaloniaUI.Handlers;

public interface IImageSourceHandler : IRegisterable
{
    Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = default(CancellationToken));
}