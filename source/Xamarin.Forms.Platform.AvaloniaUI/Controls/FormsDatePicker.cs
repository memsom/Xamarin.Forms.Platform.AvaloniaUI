using AvaloniaDatePicker = Avalonia.Controls.CalendarDatePicker;

namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsDatePicker : AvaloniaDatePicker
{
    protected override Type StyleKeyOverride => typeof(AvaloniaDatePicker);
}