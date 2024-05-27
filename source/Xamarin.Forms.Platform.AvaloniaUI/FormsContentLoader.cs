using Avalonia.Controls;
using Xamarin.Forms.Platform.AvaloniaUI.Extensions;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation;
using AvaloniaApplication = Avalonia.Application;
using AvaloniaRect = Avalonia.Rect;

namespace Xamarin.Forms.Platform.AvaloniaUI;

public sealed class FormsContentLoader : IContentLoader
{
    public Task<object> LoadContentAsync(Control parent, object oldContent, object newContent, CancellationToken cancellationToken)
    {
        if (oldContent is VisualElement element)
        {
            element.Cleanup(); // Cleanup old content
        }

        if (!AvaloniaApplication.Current?.CheckAccess() ?? false)
        {
            throw new InvalidOperationException("UIThreadRequired");
        }

        var scheduler = TaskScheduler.FromCurrentSynchronizationContext();
        return Task.Factory.StartNew(() => LoadContent(parent, newContent), cancellationToken, TaskCreationOptions.None, scheduler);
    }

    private object? LoadContent(Control parent, object page)
    {
        if (page is VisualElement visualElement)
        {
            var renderer = CreateOrResizeContent(parent, visualElement);
            return renderer;
        }
        return null;
    }

    public void OnSizeContentChanged(Control parent, object page)
    {
        if (page is VisualElement visualElement)
        {
            CreateOrResizeContent(parent, visualElement);
        }
    }

    private object CreateOrResizeContent(Control parent, VisualElement visualElement)
    {
        //if (Debugger.IsAttached)
        //	Console.WriteLine("Page type : " + visualElement.GetType() + " (" + (visualElement as Page).Title + ") -- Parent type : " + visualElement.Parent.GetType() + " -- " + parent.ActualHeight + "H*" + parent.ActualWidth + "W");
        var parentBounds = parent.Bounds;
        return CreateOrResizeContent(parentBounds, visualElement);
    }

    private object CreateOrResizeContent(AvaloniaRect parentBounds, VisualElement visualElement)
    {
        var renderer = Platform.GetOrCreateRenderer(visualElement)!;
        var actualRect = new Rectangle(0, 0, parentBounds.Width, parentBounds.Height);
        visualElement.Layout(actualRect);

        // ControlTemplate adds an additional layer through which to send sizing changes.
        var contentPage = visualElement as ContentPage;
        if (contentPage?.Content != null)
        {
            contentPage.Content?.Layout(actualRect);
        }
        else
        {
            var contentView = visualElement as ContentView;
            if (contentView?.Content != null)
            {
                contentView.Content?.Layout(actualRect);
            }
        }

        if (visualElement.RealParent is IPageController pageController)
        {
            pageController.ContainerArea = actualRect;
        }

        return renderer.GetNativeElement(); // this should actually be something
    }
}