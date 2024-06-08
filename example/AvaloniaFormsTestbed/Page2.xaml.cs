using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AvaloniaFormsTestbed;

[XamlCompilation(XamlCompilationOptions.Compile)]
public partial class Page2 : ContentPage
{
    public Page2() => InitializeComponent();

    private async void Button_OnClicked(object? sender, EventArgs e)
    {
        await Navigation.PopAsync();

    }
}