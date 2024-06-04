using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;

namespace Xamarin.Forms.Platform.AvaloniaUI.Implementation.Controls;

public class AvaloniaTimePicker : TextBox
{
    public static readonly StyledProperty<TimeSpan?> TimeProperty = AvaloniaProperty.Register<AvaloniaTimePicker, TimeSpan?>(nameof(Time));
    public static readonly StyledProperty<string> TimeFormatProperty = AvaloniaProperty.Register<AvaloniaTimePicker, string>(nameof(TimeFormat));

    static AvaloniaTimePicker()
    {
        TimeProperty.Changed.AddClassHandler<AvaloniaTimePicker>((x, e) => x.OnTimePropertyChanged(e));
        TimeFormatProperty.Changed.AddClassHandler<AvaloniaTimePicker>((x, e) => x.OnTimeFormatPropertyChanged(e));
    }

    protected override Type StyleKeyOverride => typeof(TextBox);

    public TimeSpan? Time
    {
        get => GetValue(TimeProperty);
        set => SetValue(TimeProperty, value);
    }

    public String TimeFormat
    {
        get => GetValue(TimeFormatProperty);
        set => SetValue(TimeFormatProperty, value);
    }

    public delegate void TimeChangedEventHandler(object sender, AvaloniaTimeChangedEventArgs e);
    public event TimeChangedEventHandler TimeChanged;


    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        SetText();
    }

    private void SetText()
    {
        if (Time == null)
        {
            Text = null;
        }
        else
        {
            var dateTime = new DateTime(Time.Value.Ticks);
            String text = dateTime.ToString(String.IsNullOrWhiteSpace(TimeFormat) ? @"hh\:mm" : TimeFormat.ToLower());
            if (text.CompareTo(Text) != 0)
            {
                Text = text;
            }
        }
    }

    private void SetTime()
    {
        DateTime dateTime = DateTime.MinValue;
        String timeFormat = String.IsNullOrWhiteSpace(TimeFormat) ? @"hh\:mm" : TimeFormat.ToLower();

        if (DateTime.TryParseExact(Text, timeFormat, null, System.Globalization.DateTimeStyles.None, out dateTime))
        {
            if ((Time == null) || (Time != null && Time.Value.CompareTo(dateTime.TimeOfDay) != 0))
            {
                if (dateTime.TimeOfDay < TimeSpan.FromHours(24) && dateTime.TimeOfDay > TimeSpan.Zero)
                {
                    Time = dateTime.TimeOfDay;
                }
                else
                {
                    SetText();
                }
            }
        }
        else
        {
            SetText();
        }
    }

    #region Overrides
    protected override void OnLostFocus(RoutedEventArgs e)
    {
        SetTime();
        base.OnLostFocus(e);
    }
    #endregion

    #region Property Changes
    private void OnTimePropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        OnTimeChanged(e.OldValue as TimeSpan?, e.NewValue as TimeSpan?);
    }

    protected virtual void OnTimeChanged(TimeSpan? oldValue, TimeSpan? newValue)
    {
        SetText();

        TimeChanged?.Invoke(this, new AvaloniaTimeChangedEventArgs(oldValue, newValue));
    }

    private void OnTimeFormatPropertyChanged(AvaloniaPropertyChangedEventArgs e)
    {
        OnTimeFormatChanged();
    }

    protected virtual void OnTimeFormatChanged()
    {
        SetText();
    }
    #endregion
}