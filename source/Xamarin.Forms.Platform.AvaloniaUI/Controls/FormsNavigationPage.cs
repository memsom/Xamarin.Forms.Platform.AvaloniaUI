using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsNavigationPage : AvaloniaNavigationPage
{
    NavigationPage NavigationPage;

    public FormsNavigationPage(NavigationPage navigationPage)
    {
        ContentLoader = new FormsContentLoader();
        NavigationPage = navigationPage;
    }

    public override void OnBackButtonPressed()
    {
        if (!NavigationPage.CurrentPage?.SendBackButtonPressed() ?? false)
        {
            NavigationPage.PopAsync();
        }
    }        
}