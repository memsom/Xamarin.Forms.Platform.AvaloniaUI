using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaCellControl : ContentControl
{
    public static readonly StyledProperty<object> CellProperty = AvaloniaProperty.Register<AvaloniaCellControl, object>(nameof(Cell));
    public static readonly StyledProperty<bool> ShowContextActionsProperty = AvaloniaProperty.Register<AvaloniaCellControl, bool>(nameof(ShowContextActions), true);

    readonly PropertyChangedEventHandler? propertyChangedHandler;

    protected override Type StyleKeyOverride => typeof(ContentControl);

    public AvaloniaCellControl()
    {
        CellProperty.Changed.AddClassHandler<AvaloniaCellControl>((x, e) => x.SetSource(e.OldValue, e.NewValue));

        DetachedFromVisualTree += OnEventHandler;

        LayoutUpdated += CellControl_LayoutUpdated;

        propertyChangedHandler = OnCellPropertyChanged;
    }

    private void OnEventHandler(object? sender, VisualTreeAttachmentEventArgs args)
    {
        if (DataContext is ICellController cell)
        {
            cell.SendDisappearing();
        }
    }

    private void CellControl_LayoutUpdated(object? sender, EventArgs e)
    {
        OnRenderSizeChanged(this.Bounds.Size);
    }

    public Cell Cell
    {
        get => (Cell)GetValue(CellProperty);
        set => SetValue(CellProperty, value);
    }

    public bool ShowContextActions
    {
        get => GetValue(ShowContextActionsProperty);
        set => SetValue(ShowContextActionsProperty, value);
    }

    global::Avalonia.Markup.Xaml.Templates.DataTemplate GetTemplate(Cell cell)
    {
        var renderer = Registrar.Registered.GetHandlerForObject<ICellRenderer>(cell);
        return renderer.GetTemplate(cell);
    }

    void OnCellPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == "HasContextActions")
        {
            SetupContextMenu();
        }
    }

    void SetSource(object? oldCellObj, object? newCellObj)
    {
        var oldCell = oldCellObj as Cell;
        var newCell = newCellObj as Cell;

        if (oldCell != null)
        {
            oldCell.PropertyChanged -= propertyChangedHandler;
            ((ICellController)oldCell).SendDisappearing();
        }

        if (newCell != null)
        {
            ((ICellController)newCell).SendAppearing();

            if (oldCell == null || oldCell.GetType() != newCell.GetType())
            {
                ContentTemplate = GetTemplate(newCell);
            }

            Content = newCell;

            SetupContextMenu();

            newCell.PropertyChanged += propertyChangedHandler;
        }
        else
        {
            Content = null;
        }
    }

    protected virtual void OnRenderSizeChanged(global::Avalonia.Size newSize)
    {
        if (Content is ViewCell {LogicalChildren: not null} vc && vc.LogicalChildren.Count != 0)
        {
            foreach (var child in vc.LogicalChildren)
            {
                if (child is Layout {HorizontalOptions.Expands: true} layout)
                {
                    layout.Layout(new Rectangle(layout.X, layout.Y, newSize.Width, newSize.Height));
                }
            }
        }
    }

    void SetupContextMenu()
    {
        if (Content == null || !ShowContextActions)
        {
            return;
        }

        //if (!Cell.HasContextActions)
        //{
        //	ContextMenuService.SetContextMenu(this, null);
        //	return;
        //}

        ApplyTemplate();

        //ContextMenu menu = new CustomContextMenu();
        //menu.SetBinding(ItemsControl.ItemsSourceProperty, new System.Windows.Data.Binding("ContextActions"));

        //ContextMenuService.SetContextMenu(this, menu);
    }
}