using Avalonia.Controls;
using Avalonia.Input;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation;

public class EntryCellPhoneTextBox : TextBox
{
    public event EventHandler KeyboardReturnPressed;

    protected override void OnKeyUp(KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            EventHandler handler = KeyboardReturnPressed;
            if (handler != null) handler(this, EventArgs.Empty);
        }

        base.OnKeyUp(e);
    }
}