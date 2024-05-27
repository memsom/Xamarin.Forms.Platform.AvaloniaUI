using Avalonia.Controls;
using Avalonia.Media.Imaging;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI.Handlers;
using AvaloniaStretch = Avalonia.Media.Stretch;

namespace Xamarin.Forms.Platform.AvaloniaUI.Extensions;

public static class ImageExtensions
{
    public static AvaloniaStretch ToStretch(this Aspect aspect)
    {
        switch (aspect)
        {
            case Aspect.Fill:
                return AvaloniaStretch.Fill;
            case Aspect.AspectFill:
                return AvaloniaStretch.UniformToFill;
            default:
            case Aspect.AspectFit:
                return AvaloniaStretch.Uniform;
        }
    }

    public static Size GetImageSourceSize(this Bitmap source)
    {
        if (source is null)
        {
            return Size.Zero;
        }
        else if (source is Bitmap bitmap)
        {
            return new Size
            {
                Width = bitmap.PixelSize.Width,
                Height = bitmap.PixelSize.Height
            };
        }

        throw new InvalidCastException($"\"{source.GetType().FullName}\" is not supported.");
    }

    public static IconElement ToNativeIconElement(this ImageSource source) { return source.ToNativeIconElementAsync().GetAwaiter().GetResult(); }

    public static async Task<IconElement> ToNativeIconElementAsync(this ImageSource source, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (source == null || source.IsEmpty) return null;

        var handler = Internals.Registrar.Registered.GetHandlerForObject<IIconElementHandler>(source);
        if (handler == null) return null;

        try
        {
            return await handler.LoadIconElementAsync(source, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            // no-op
        }

        return null;
    }

    public static Bitmap ToWindowsImageSource(this ImageSource source) { return source.ToNativeImageSourceAsync().GetAwaiter().GetResult(); }

    public static async Task<Bitmap> ToNativeImageSourceAsync(this ImageSource source, CancellationToken cancellationToken = default(CancellationToken))
    {
        if (source == null || source.IsEmpty) return null;

        var handler = Registrar.Registered.GetHandlerForObject<IImageSourceHandler>(source);
        if (handler == null) return null;

        try
        {
            return await handler.LoadImageAsync(source, cancellationToken);
        }
        catch (OperationCanceledException)
        {
            Log.Warning("Image loading", "Image load cancelled");
        }
        catch (Exception ex)
        {
            Log.Warning("Image loading", $"Image load failed: {ex}");
        }

        return null;
    }
}