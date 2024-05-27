using Avalonia.Controls.ApplicationLifetimes;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using AvaloniaApplication = Avalonia.Application;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

internal class AvaloniaDeviceInfo : DeviceInfo
{
    public override Size PixelScreenSize
    {
        get
        {
            double scaling = ScalingFactor;
            Size scaled = ScaledScreenSize;
            double width = Math.Round(scaled.Width * scaling);
            double height = Math.Round(scaled.Height * scaling);

            return new Size(width, height);
        }
    }

    public override Size ScaledScreenSize
    {
        get
        {
            if (AvaloniaApplication.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime {MainWindow: {Screens: {Primary: {WorkingArea: { } workingArea}}}})
            {
                return workingArea.ToRect(ScalingFactor).Size.ToSize();
            }

            return Size.Zero;
        }
    }

    public override double ScalingFactor => 1.0;

    public AvaloniaDeviceInfo() { }
}