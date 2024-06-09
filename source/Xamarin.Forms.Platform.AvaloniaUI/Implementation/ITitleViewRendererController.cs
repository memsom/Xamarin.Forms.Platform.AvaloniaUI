using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public interface ITitleViewRendererController
{
    object? TitleViewContent { get; }
    Control? TitleViewPresenter { get; }
    bool TitleViewVisibility { get; set; }
    FormsCommandBar? CommandBar { get; }
}