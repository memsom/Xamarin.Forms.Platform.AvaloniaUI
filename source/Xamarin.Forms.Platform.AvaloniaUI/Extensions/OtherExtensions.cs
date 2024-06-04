using Avalonia;
using IAvaloniaValueConverter = Avalonia.Data.Converters.IValueConverter;
using AvaloniaBinding = Avalonia.Data.Binding;
using AvaloniaScrollBarVisibility = Avalonia.Controls.Primitives.ScrollBarVisibility;

namespace Xamarin.Forms.Platform.AvaloniaUI.Extensions;

internal static class OtherExtensions
	{
		public static void SetBinding(this AvaloniaObject self, AvaloniaProperty property, string path) =>
			self.Bind(property, new AvaloniaBinding
			{
				Path = path
			});

		public static void SetBinding(this AvaloniaObject self, AvaloniaProperty property, string path, IAvaloniaValueConverter converter) =>
			self.Bind(property, new AvaloniaBinding
			{
				Path = path,
				Converter = converter
			});

		internal static InputScopeNameValue GetKeyboardButtonType(this ReturnType returnType) =>
			returnType switch
			{
				ReturnType.Default or ReturnType.Done or ReturnType.Go or ReturnType.Next or ReturnType.Send => InputScopeNameValue.Default,
				ReturnType.Search => InputScopeNameValue.Search,
				_ => throw new NotImplementedException($"ReturnType {returnType} not supported")
			};

		internal static InputScope ToInputScope(this ReturnType returnType)
		{
			var scopeName = new InputScopeName()
			{
				NameValue = GetKeyboardButtonType(returnType)
			};

			var inputScope = new InputScope
			{
				Names = { scopeName }
			};

			return inputScope;
		}

		internal static AvaloniaScrollBarVisibility ToNativeScrollBarVisibility(this ScrollBarVisibility visibility) =>
			visibility switch
			{
				ScrollBarVisibility.Always => AvaloniaScrollBarVisibility.Visible,
				ScrollBarVisibility.Default => AvaloniaScrollBarVisibility.Auto,
				ScrollBarVisibility.Never => AvaloniaScrollBarVisibility.Hidden,
				_ => AvaloniaScrollBarVisibility.Auto
			};

		public static T Clamp<T>(this T value, T min, T max) where T : IComparable<T>
		{
			if (value.CompareTo(min) < 0)
				return min;
			if (value.CompareTo(max) > 0)
				return max;
			return value;
		}


		internal static int ToEm(this double pt) => Convert.ToInt32( pt * 0.0624f * 1000); //Coefficient for converting Pt to Em. The value is uniform spacing between characters, in units of 1/1000 of an em.
	}