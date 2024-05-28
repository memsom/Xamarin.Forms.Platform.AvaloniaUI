using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvaloniaFormsTestbed;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("hello, world");
        //Label1.Text = "Hello, world";
    }
}