using System.Windows.Input;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Dialogs;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

internal interface IContentDialog
{
    void Hide();

    Task<ContentDialogResult> ShowAsync();

    bool FullSizeDesired { get; set; }
    bool IsPrimaryButtonEnabled { get; set; }
    bool IsSecondaryButtonEnabled { get; set; }
    ICommand PrimaryButtonCommand { get; set; }
    object PrimaryButtonCommandParameter { get; set; }
    string PrimaryButtonText { get; set; }
    ICommand SecondaryButtonCommand { get; set; }
    object SecondaryButtonCommandParameter { get; set; }
    string SecondaryButtonText { get; set; }
    object Title { get; set; }
    Avalonia.Markup.Xaml.Templates.DataTemplate TitleTemplate { get; set; }

    event EventHandler<ContentDialogClosedEventArgs> Closed;
    event EventHandler<ContentDialogClosingEventArgs> Closing;
    event EventHandler<ContentDialogOpenedEventArgs> Opened;
    event EventHandler<ContentDialogButtonClickEventArgs> PrimaryButtonClick;
    event EventHandler<ContentDialogButtonClickEventArgs> SecondaryButtonClick;
}