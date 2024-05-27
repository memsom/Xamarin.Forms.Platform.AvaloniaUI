using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI;

namespace AvaloniaFormsTestbed.Avalonia;

public partial class MainWindow : FormsApplicationPage
{
    public MainWindow()
    {
        InitializeComponent();

        Forms.Init();
        LoadApplication(new AvaloniaFormsTestbed.App());
    }
}