using System.Reflection;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation;
using AvaloniaApplication = Avalonia.Application;
using AvaloniaSolidColorBrush = Avalonia.Media.SolidColorBrush;

// ReSharper disable once CheckNamespace
namespace Xamarin.Forms.Platform.AvaloniaUI;

public static class Forms
{
    public static bool IsInitialized { get; private set; }

    static bool FlagsSet { get; set; }

    static IReadOnlyList<string>? sFlags;
    public static IReadOnlyList<string> Flags => sFlags ??= new List<string>().AsReadOnly();

    public static void Init<T>(IEnumerable<Assembly>? rendererAssemblies = null) where T : Application, new()
    {
        Init(null, rendererAssemblies);
        Application.Current = new T();
    }

    public static void Init(Type? applicationType = null, IEnumerable<Assembly>? rendererAssemblies = null)
    {
        if (IsInitialized)
            return;

        if (AvaloniaApplication.Current != null && AvaloniaApplication.Current.Resources.ContainsKey("SystemColorControlAccentBrush") && AvaloniaApplication.Current.Resources["SystemColorControlAccentBrush"] is AvaloniaSolidColorBrush accent)
        {
            Color.SetAccent(accent.ToFormsColor());
        }

        Log.Listeners.Add(new DelegateLogListener((c, m) => Console.WriteLine("[{0}] {1}", m, c)));
        Registrar.ExtraAssemblies = rendererAssemblies?.ToArray();

        Device.SetTargetIdiom(TargetIdiom.Desktop);
        Device.PlatformServices = new AvaloniaPlatformServices();
        Device.Info = new AvaloniaDeviceInfo();
        ExpressionSearch.Default = new AvaloniaExpressionSearch();

        Registrar.RegisterAll([typeof(ExportRendererAttribute), typeof(ExportCellAttribute), typeof(ExportImageSourceHandlerAttribute)]);

        Ticker.SetDefault(new AvaloniaTicker());
        Device.SetIdiom(TargetIdiom.Desktop);
        Device.SetFlags(sFlags);

        IsInitialized = true;

        if (applicationType != null)
        {
            Application.Current = Activator.CreateInstance(applicationType) as Application;
        }
    }

    public static void SetFlags(params string[] flags)
    {
        if (FlagsSet)
        {
            return;
        }

        if (IsInitialized)
        {
            throw new InvalidOperationException($"{nameof(SetFlags)} must be called before {nameof(Init)}");
        }

        sFlags = flags.ToList().AsReadOnly();
        FlagsSet = true;
    }
}