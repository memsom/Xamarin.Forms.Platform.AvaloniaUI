using Avalonia.Threading;

namespace Xamarin.Forms.Platform.AvaloniaUI;

static class UiHelper
{
    public static void ExecuteInUiThread(Action action)
    {
        if (global::Avalonia.Application.Current.CheckAccess())
        {
            action?.Invoke();
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync(action);
        }
    }
}