using System.ComponentModel;
using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public class FormsApplicationPage : ApplicationWindow
{
    public Application? Application { get; private set; }

    public Platform? Platform { get; private set; }

    public FormsApplicationPage()
    {
        this.DataContext = this;
        //System.Windows.Application.Current.Startup += OnLaunching;
        //System.Windows.Application.Current.Exit += OnClosing;

        //MessagingCenter.Send(this, AvaloniaDeviceInfo.BWPorientationChangedName, this.ToDeviceOrientation());
        //SizeChanged += OnOrientationChanged;

        this.ContentLoader = new FormsContentLoader();
    }

    public void LoadApplication(Application application)
    {
        Application.Current = application;
        application.PropertyChanged += ApplicationOnPropertyChanged;
        Application = application;

        // Hack around the fact that OnLaunching will have already happened by this point, sad but needed.
        application.SendStart();

        if (application.MainPage is not null)
        {
            SetMainPage();
        }
    }

    void ApplicationOnPropertyChanged(object? sender, PropertyChangedEventArgs args)
    {
        if (args.PropertyName == nameof(Application.MainPage) && IsInitialized)
        {
            SetMainPage();
        }
    }

    void OnOrientationChanged(object sender, SizeChangedEventArgs e)
    {
        //MessagingCenter.Send(this, WPFDeviceInfo.BWPorientationChangedName, this.ToDeviceOrientation());
    }

    // void OnClosing(object sender, ExitEventArgs e)
    // {
    //     Application.SendSleep();
    // }
    //
    // void OnDeactivated(object sender, System.EventArgs e)
    // {
    //     Application.SendSleep();
    // }
    //
    // void OnLaunching(object sender, StartupEventArgs e)
    // {
    //     Application.SendStart();
    // }

    void SetMainPage()
    {
        if (Platform == null)
        {
            Platform = new AvaloniaFormsPlatform(this);
        }
        if (Application == null)
        {
            Application = Application.Current;
        }

        if (Application is {MainPage: not null})
        {
            Platform.SetPage(Application.MainPage);
        }
    }
}