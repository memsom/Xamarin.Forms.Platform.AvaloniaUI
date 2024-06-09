using Xamarin.Forms.Platform.AvaloniaUI.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

internal interface IToolbarProvider
{
    Task<FormsCommandBar>? GetCommandBarAsync();
}