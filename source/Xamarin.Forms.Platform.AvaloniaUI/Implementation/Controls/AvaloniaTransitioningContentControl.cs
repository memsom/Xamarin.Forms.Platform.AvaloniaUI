using System.Diagnostics.CodeAnalysis;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls.Enums;
using AvaloniaContentPresenter = Avalonia.Controls.Presenters.ContentPresenter;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaTransitioningContentControl : AvaloniaDynamicContentControl
{
    internal const string PreviousContentPresentationSitePartName = "PreviousContentPresentationSite";
    internal const string CurrentContentPresentationSitePartName = "CurrentContentPresentationSite";

    public static readonly AvaloniaProperty IsTransitioningProperty = AvaloniaProperty.Register<AvaloniaTransitioningContentControl, bool>(nameof(IsTransitioning));
    public static readonly StyledProperty<TransitionType> TransitionProperty = AvaloniaProperty.Register<AvaloniaTransitioningContentControl, TransitionType>(nameof(Transition));
    public static readonly AvaloniaProperty RestartTransitionOnContentChangeProperty = AvaloniaProperty.Register<AvaloniaTransitioningContentControl, bool>(nameof(RestartTransitionOnContentChange));

    static AvaloniaTransitioningContentControl()
    {
        ContentProperty.Changed.AddClassHandler<AvaloniaTransitioningContentControl>((x, e) => x.OnContentPropertyChanged(e));
        IsTransitioningProperty.Changed.AddClassHandler<AvaloniaTransitioningContentControl>((x, e) => x.OnIsTransitioningPropertyChanged(e));
        TransitionProperty.Changed.AddClassHandler<AvaloniaTransitioningContentControl>((x, e) => x.OnTransitionPropertyChanged(e));
    }

    protected override Type StyleKeyOverride => typeof(AvaloniaTransitioningContentControl);

    private bool allowIsTransitioningPropertyWrite;

    private AvaloniaContentPresenter? currentContentPresentationSite;
    private AvaloniaContentPresenter? previousContentPresentationSite;

    public event EventHandler? TransitionCompleted;

    /// <summary>
    /// Gets/sets if the content is transitioning.
    /// </summary>
    public bool IsTransitioning
    {
        get => (bool)GetValue(IsTransitioningProperty);
        private set
        {
            allowIsTransitioningPropertyWrite = true;
            SetValue(IsTransitioningProperty, value);
            allowIsTransitioningPropertyWrite = false;
        }
    }

    public TransitionType Transition
    {
        get => GetValue(TransitionProperty);
        set => SetValue(TransitionProperty, value);
    }

    public bool RestartTransitionOnContentChange
    {
        get => (bool)GetValue(RestartTransitionOnContentChangeProperty);
        set => SetValue(RestartTransitionOnContentChangeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (IsTransitioning)
        {
            AbortTransition();
        }

        base.OnApplyTemplate(e);

        previousContentPresentationSite = e.NameScope.Find<AvaloniaContentPresenter>(PreviousContentPresentationSitePartName);
        currentContentPresentationSite = e.NameScope.Find<AvaloniaContentPresenter>(CurrentContentPresentationSitePartName);
    }

    private void OnTransitionPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (IsTransitioning)
        {
            AbortTransition();
        }
    }

    private void OnIsTransitioningPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        if (!allowIsTransitioningPropertyWrite)
        {
            IsTransitioning = (bool)e.OldValue;
            throw new InvalidOperationException();
        }
    }

    private void OnContentPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        StartTransition(e.OldValue, e.NewValue);
    }

    #region Transition
    [SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", MessageId = "newContent", Justification = "Should be used in the future.")]
    private void StartTransition(object? oldContent, object? newContent)
    {
        // both presenters must be available, otherwise a transition is useless.
        if (currentContentPresentationSite != null)
        {
            currentContentPresentationSite.Content = newContent;
        }
        if (previousContentPresentationSite != null)
        {
            previousContentPresentationSite.Content = oldContent;
            ResetTransition();
        }

        // and start a new transition
        if (!IsTransitioning || RestartTransitionOnContentChange)
        {
            IsTransitioning = true;
        }
    }

    /// <summary>
    /// Reload the current transition if the content is the same.
    /// </summary>
    public void ReloadTransition()
    {
        // both presenters must be available, otherwise a transition is useless.
        if (currentContentPresentationSite != null && previousContentPresentationSite != null)
        {
            if (!IsTransitioning || RestartTransitionOnContentChange)
            {
                IsTransitioning = true;
            }
        }
    }

    public void AbortTransition()
    {
        // go to normal state and release our hold on the old content.
        IsTransitioning = false;
        if (previousContentPresentationSite != null)
        {
            previousContentPresentationSite.Content = null;
        }
    }

    private string GetTransitionName(TransitionType transition) =>
        transition switch
        {
            TransitionType.Default => "DefaultTransition",
            TransitionType.Normal => "Normal",
            TransitionType.Up => "UpTransition",
            TransitionType.Down => "DownTransition",
            TransitionType.Right => "RightTransition",
            TransitionType.RightReplace => "RightReplaceTransition",
            TransitionType.Left => "LeftTransition",
            TransitionType.LeftReplace => "LeftReplaceTransition",
            _ => "DefaultTransition"
        };

    protected virtual void ResetTransition()
    {
        DispatcherTimer.RunOnce(() =>
        {
            previousContentPresentationSite.Content = null;
            TransitionCompleted?.Invoke(this, EventArgs.Empty);
        }, TimeSpan.FromSeconds(0.2));
    }

    #endregion
}