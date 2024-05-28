namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsRectangle() : Avalonia.Controls.Shapes.Rectangle
{
    protected override Type StyleKeyOverride => typeof(Avalonia.Controls.Shapes.Rectangle);
}