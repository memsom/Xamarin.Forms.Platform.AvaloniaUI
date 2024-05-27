namespace Xamarin.Forms.Platform.AvaloniaUI;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class ExportCellAttribute(Type handler, Type target) : HandlerAttribute(handler, target);