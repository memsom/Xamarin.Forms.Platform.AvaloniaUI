using Avalonia.Controls;
using Avalonia.Styling;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsContentPage : AvaloniaDynamicContentPage
{
    protected override Type StyleKeyOverride => typeof(ContentControl);
}