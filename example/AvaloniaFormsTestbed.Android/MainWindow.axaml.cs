using Xamarin.Forms.Platform.AvaloniaUI;

namespace AvaloniaFormsTestbed.Android;

public partial class MainWindow : FormsApplicationPage
{
    public MainWindow()
    {
        InitializeComponent();

        Forms.Init();
        LoadApplication(new AvaloniaFormsTestbed.App());
    }
}