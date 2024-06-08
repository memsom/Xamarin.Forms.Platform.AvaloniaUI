using Xamarin.Forms.Platform.AvaloniaUI;

namespace AvaloniaFormsTestbed.Avalonia;

public partial class MainWindow : FormsApplicationPage
{
    public MainWindow()
    {
        InitializeComponent();

        Xamarin.Forms.Forms.Init();
        LoadApplication(new AvaloniaFormsTestbed.App());
    }
}