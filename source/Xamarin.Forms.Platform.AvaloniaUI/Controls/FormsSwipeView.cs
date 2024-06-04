namespace Xamarin.Forms.Platform.AvaloniaUI.Controls;

public class FormsSwipeView : FormsMultiView
{
    protected override void Appearing()
    {
        base.Appearing();

        UpdateCurrentSelectedIndex(0);
    }

    private void UpdateCurrentSelectedIndex(object newValue)
    {
        if (ItemsSource == null) return;
        var items = ItemsSource.Cast<object>();

        if ((int)newValue >= 0 && (int)newValue < items.Count())
        {
            SelectedItem = items.ElementAt((int)newValue);
        }
    }
}