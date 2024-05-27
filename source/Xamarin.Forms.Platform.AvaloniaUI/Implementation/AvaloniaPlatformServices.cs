using System.IO.IsolatedStorage;
using System.Net;
using System.Reflection;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public class AvaloniaPlatformServices : IPlatformServices
{
    public bool IsInvokeRequired => !global::Avalonia.Application.Current.CheckAccess();
    public OSAppTheme RequestedTheme { get; }

    public string RuntimePlatform => "Avalonia";

    public void BeginInvokeOnMainThread(Action action) { Dispatcher.UIThread.InvokeAsync(action); }

    public Ticker CreateTicker() => new AvaloniaTicker();

    public Assembly[] GetAssemblies() { return AppDomain.CurrentDomain.GetAssemblies(); }

    public string GetHash(string input) => AvaloniaFormsPlatformCrc64.GetHash(input);

    string IPlatformServices.GetMD5Hash(string input) => GetHash(input);

    public double GetNamedSize(NamedSize size, Type targetElementType, bool useOldSizes) => size.GetFontSize();
    public Color GetNamedColor(string name) { throw new NotImplementedException(); }

    public SizeRequest GetNativeSize(VisualElement view, double widthConstraint, double heightConstraint) { return Platform.GetNativeSize(view, widthConstraint, heightConstraint); }

    public Task<Stream> GetStreamAsync(Uri uri, CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<Stream>();

        try
        {
            HttpWebRequest request = WebRequest.CreateHttp(uri);
            request.BeginGetResponse(ar =>
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    tcs.SetCanceled();
                    return;
                }

                try
                {
                    Stream stream = request.EndGetResponse(ar).GetResponseStream();
                    tcs.TrySetResult(stream);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            }, null);
        }
        catch (Exception ex)
        {
            tcs.TrySetException(ex);
        }

        return tcs.Task;
    }

    public IIsolatedStorageFile GetUserStoreForApplication() { return new AvaloniaIsolatedStorageFile(IsolatedStorageFile.GetUserStoreForAssembly()); }

    public void OpenUriAction(Uri uri) { System.Diagnostics.Process.Start(uri.AbsoluteUri); }

    public void QuitApplication()
    {
        var application = global::Avalonia.Application.Current;
        if (application.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.Shutdown();
        }
    }

    public void StartTimer(TimeSpan interval, Func<bool> callback)
    {
        var timer = new DispatcherTimer() {Interval = interval};
        timer.Start();
        timer.Tick += (sender, args) =>
        {
            bool result = callback();
            if (!result) timer.Stop();
        };
    }
}