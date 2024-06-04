using Xamarin.Forms;
using Xamarin.Forms.Platform.AvaloniaUI;
using Xamarin.Forms.Platform.AvaloniaUI.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Renderers;
using Xamarin.Forms.Platform.AvaloniaUI.TemplateSelectors;
using AvaloniaSelectionChangedEventArgs = Avalonia.Controls.SelectionChangedEventArgs;

[assembly: ExportRenderer(typeof(TableView), typeof(TableViewRenderer))]

namespace Xamarin.Forms.Platform.AvaloniaUI.Renderers;

public class TableViewRenderer : ViewRenderer<TableView, FormsListView>
{
    public override SizeRequest GetDesiredSize(double widthConstraint, double heightConstraint)
    {
        SizeRequest result = base.GetDesiredSize(widthConstraint, heightConstraint);
        result.Minimum = new Size(40, 40);
        return result;
    }


    protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.TableView> e)
    {
        if (e.OldElement != null)
        {
            Element.ModelChanged -= OnModelChanged;
        }

        if (e.NewElement != null)
        {
            if (Control == null) // construct and SetNativeControl and suscribe control event
            {
                var listView = new FormsListView()
                {
                    ItemTemplateSelector = new TableViewDataTemplateSelector(),
                    //Style = (System.Windows.Style)System.Windows.Application.Current.Resources["TableViewTemplate"],
                };

                SetNativeControl(listView);
                Control.SelectionChanged += Control_SelectionChanged;
            }

            // Update control property
            Control.Items.ReplaceRange(GetTableViewRow());

            // Element event
            Element.ModelChanged += OnModelChanged;
        }

        base.OnElementChanged(e);
    }


    private void Control_SelectionChanged(object? sender, AvaloniaSelectionChangedEventArgs e)
    {
        foreach (object item in e.AddedItems)
        {
            if (item is Cell cell)
            {
                if (cell is {IsEnabled: true}) Element?.Model.RowSelected(cell);
                break;
            }
        }

        Control.SelectedItem = null;
    }

    void OnModelChanged(object? sender, EventArgs eventArgs) => Control.Items.ReplaceRange(GetTableViewRow());

    public IList<object> GetTableViewRow()
    {
        List<object> result = new List<object>();

        foreach (var item in Element.Root)
        {
            if (!string.IsNullOrWhiteSpace(item.Title)) result.Add(item);

            result.AddRange(item);
        }

        return result;
    }

    bool isDisposed;

    protected override void Dispose(bool disposing)
    {
        if (isDisposed) return;

        if (disposing)
        {
            if (Control != null)
            {
                Control.SelectionChanged -= Control_SelectionChanged;
            }

            if (Element != null)
            {
                Element.ModelChanged -= OnModelChanged;
            }
        }

        isDisposed = true;
        base.Dispose(disposing);
    }
}