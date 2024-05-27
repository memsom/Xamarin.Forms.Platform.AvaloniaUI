using Avalonia.Media;
using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Handlers;
using AvaloniaBrush = Avalonia.Media.Brush;


[assembly: ExportImageSourceHandler(typeof(FontImageSource), typeof(FontImageSourceHandler))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Handlers;

public sealed class FontImageSourceHandler : IImageSourceHandler
{
    public Task<global::Avalonia.Media.Imaging.Bitmap> LoadImageAsync(ImageSource imagesource, CancellationToken cancelationToken = new CancellationToken())
    {
        var fontsource = imagesource as FontImageSource;
        var image = CreateGlyph(
            fontsource.Glyph,
            new FontFamily(new Uri("pack://application:,,,"), fontsource.FontFamily),
            FontStyle.Normal,
            FontWeight.Normal,
            fontsource.Size,
            (fontsource.Color != Color.Default ? fontsource.Color : Color.White).ToBrush());
        return Task.FromResult(image);
    }

    static global::Avalonia.Media.Imaging.Bitmap? CreateGlyph(
        string text,
        FontFamily fontFamily,
        FontStyle fontStyle,
        FontWeight fontWeight,
        //FontStretch fontStretch,
        double fontSize,
        AvaloniaBrush foreBrush)
    {
        if (fontFamily == null || string.IsNullOrEmpty(text))
        {
            return null;
        }
        //var typeface = new Typeface(fontFamily.Name, fontSize, fontStyle, fontWeight);
        //if (!typeface.TryGetGlyphTypeface(out GlyphTypeface glyphTypeface))
        //{
        //    //if it does not work
        //    return null;
        //}

        //var glyphIndexes = new ushort[text.Length];
        //var advanceWidths = new double[text.Length];
        //for (int n = 0; n < text.Length; n++)
        //{
        //    var glyphIndex = glyphTypeface.CharacterToGlyphMap[text[n]];
        //    glyphIndexes[n] = glyphIndex;
        //    var width = glyphTypeface.AdvanceWidths[glyphIndex] * 1.0;
        //    advanceWidths[n] = width;
        //}

        //var gr = new GlyphRun(glyphTypeface,
        //    0, false,
        //    fontSize,
        //    glyphIndexes,
        //    new global::Avalonia.Point(0, 0),
        //    advanceWidths,
        //    null, null, null, null, null, null);
        //var glyphRunDrawing = new GlyphRunDrawing(foreBrush, gr);
        //return new DrawingImage(glyphRunDrawing);
        // TODO:
        return null;
    }
}