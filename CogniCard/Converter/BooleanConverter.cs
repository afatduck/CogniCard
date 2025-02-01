using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace CogniCard.Converter
{
    public class BooleanConverter<T> : IValueConverter
    {
        public BooleanConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public T True { get; set; }
        public T False { get; set; }

        public virtual object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is string && (string)parameter == "Invert") return ConvertBack(value, targetType, null, culture);
            return value is bool && ((bool)value) ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is string && (string)parameter == "Invert") return Convert(value, targetType, null, culture)!;
            return value is T && EqualityComparer<T>.Default.Equals((T)value, True);
        }
    }
}
