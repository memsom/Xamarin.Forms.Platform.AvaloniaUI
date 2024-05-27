using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Xamarin.Forms.Internals;
using AvaloniaApplication = Avalonia.Application;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

internal class AvaloniaTicker : Ticker
{
    [ThreadStatic]
    static Ticker? ticker;

    readonly DispatcherTimer timer;

    public AvaloniaTicker()
    {
        timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(15) };
        timer.Tick += (_, _) => SendSignals();
    }

    protected override void DisableTimer() => timer.Stop();

    protected override void EnableTimer() => timer.Start();

    protected override Ticker GetTickerInstance()
    {
        if(AvaloniaApplication.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime {MainWindow: not null})
        {
            // We've got multiple windows open, we'll need to use the local ThreadStatic Ticker instead of the
            // singleton in the base class
            if (ticker == null)
            {
                ticker = new AvaloniaTicker();
            }

            return ticker;
        }

        return base.GetTickerInstance();
    }
}