using AvaloniaSolidColorBrush = Avalonia.Media.SolidColorBrush;
using AvaloniaBrush = Avalonia.Media.Brush;
using AvaloniaColor = Avalonia.Media.Color;
using AvaloniaColors = Avalonia.Media.Colors;
using FormsColor = Xamarin.Forms.Color;

namespace Xamarin.Forms.Platform.AvaloniaUI.Extensions;

public static class ColorExtensions
{
    public static AvaloniaColor GetContrastingColor(this AvaloniaColor color)
    {
        var nThreshold = 105;
        int bgLuminance = Convert.ToInt32(color.R * 0.2 + color.G * 0.7 + color.B * 0.1);

        var contrastingColor = 255 - bgLuminance < nThreshold ? AvaloniaColors.Black : AvaloniaColors.White;
        return contrastingColor;
    }

    public static AvaloniaBrush ToBrush(this FormsColor color) => new AvaloniaSolidColorBrush(color.ToNativeColor());

    public static AvaloniaColor ToNativeColor(this FormsColor color) =>
        AvaloniaColor.FromArgb((byte)(color.A * 255), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));

    public static FormsColor ToFormsColor(this AvaloniaColor color) => Color.FromRgba(color.R, color.G, color.B, color.A);

    public static FormsColor ToFormsColor(this AvaloniaSolidColorBrush solidColorBrush) => solidColorBrush.Color.ToFormsColor();
}