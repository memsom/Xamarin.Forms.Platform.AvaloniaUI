using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Xamarin.Forms.Platform.AvaloniaUI.Extensions;

public static class ControlExtensions
{
    public static object? UpdateDependencyColor(this AvaloniaObject? instance, AvaloniaProperty property, Color newColor)
    {
        if (instance is null) return null;

        switch (newColor.IsDefault)
        {
            case false:
                instance.SetValue(property, newColor.ToBrush());
                break;
            default:
                instance.ClearValue(property);
                break;
        }

        return instance.GetValue(property);
    }

    public static T? GetDefaultValue<T>(this StyledPropertyMetadata<T> propertyMetadata) => default;

    public static T? Find<T>(this Control control, string name, TemplateAppliedEventArgs? e = null) where T : Control
    {
        if (e != null)
        {
            var child = global::Avalonia.Controls.NameScopeExtensions.Find<T>(e.NameScope, name);
            if (child == null)
            {
                child = control.FindControl<T>(name);
            }
            return child;
        }
        return control.FindControl<T>(name);
    }
}