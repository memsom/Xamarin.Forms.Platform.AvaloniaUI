using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvaloniaFormsTestbed;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class MainPage
{
    public MainPage()
    {
        InitializeComponent();

        BindingContext = this;
    }

    private async void Button2_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PushAsync(new Page2());

    }
}