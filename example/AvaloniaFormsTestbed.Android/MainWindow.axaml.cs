using Avalonia.Markup.Xaml;
using Xamarin.Forms.Platform.AvaloniaUI;

namespace AvaloniaFormsTestbed.Android;

public partial class MainWindow : FormsApplicationControl
{
    public MainWindow()
    {
        InitializeComponent();

        Forms.Init();
        LoadApplication(new AvaloniaFormsTestbed.App());
    }
}