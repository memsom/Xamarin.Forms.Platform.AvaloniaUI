using System.Windows.Input;

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class EntryCellRendererCompleted : ICommand
{
    public bool CanExecute(object parameter) { return true; }

    public event EventHandler CanExecuteChanged;

    public void Execute(object parameter)
    {
        var entryCell = (IEntryCellController)parameter;
        entryCell.SendCompleted();
    }

    protected virtual void OnCanExecuteChanged() { CanExecuteChanged?.Invoke(this, EventArgs.Empty); }
}