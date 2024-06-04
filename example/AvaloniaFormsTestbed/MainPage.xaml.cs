using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvaloniaFormsTestbed;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        BindingContext = this;
    }

    private void Button_OnClicked(object? sender, EventArgs e)
    {
        System.Diagnostics.Debug.WriteLine("hello, world");
        Label1.Text = "Hello, world";
    }

    public IList<string> Items { get; } = new List<string>
    {
        "test1",
        "test2",
        "test3",
        "test4",
        "test5",
        "test6",
        "test7",
    };
}