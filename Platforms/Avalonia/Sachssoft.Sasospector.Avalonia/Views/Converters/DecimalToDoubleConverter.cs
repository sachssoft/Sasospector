using Avalonia.Data.Converters;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sachssoft.Sasospector.Views.Converters
{
    public class DecimalToDoubleConverter : IValueConverter
    {
        public static readonly DecimalToDoubleConverter Instance = new();

        public object? Convert(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null)
                return 0.0;

            return System.Convert.ToDouble(value);
        }

        public object? ConvertBack(
            object? value,
            Type targetType,
            object? parameter,
            CultureInfo culture)
        {
            if (value == null)
                return 0m;

            return System.Convert.ToDecimal(value);
        }
    }
}
