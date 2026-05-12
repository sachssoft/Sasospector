using Avalonia;
using Avalonia.Data.Converters;
using Avalonia.Media;
using System;
using System.Globalization;

namespace Sachssoft.Sasospector.Views.Converters
{
    class ColorToHexConverter : IValueConverter
    {
        public static readonly ColorToHexConverter Instance = new();

        public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not Color c)
                return "#000000";

            // #RRGGBB oder #AARRGGBB
            return c.A == 255
                ? $"#{c.R:X2}{c.G:X2}{c.B:X2}"
                : $"#{c.A:X2}{c.R:X2}{c.G:X2}{c.B:X2}";
        }

        public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string s)
                return AvaloniaProperty.UnsetValue;

            if (Color.TryParse(s, out var color))
                return color;

            return AvaloniaProperty.UnsetValue;
        }
    }
}