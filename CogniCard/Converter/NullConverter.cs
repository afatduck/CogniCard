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
    public class NullConverter<T> : IValueConverter
    {
        public NullConverter(T trueValue, T falseValue)
        {
            True = trueValue;
            False = falseValue;
        }

        public T True { get; set; }
        public T False { get; set; }

        public virtual object? Convert(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (parameter is string stringPar)
            {
                if (stringPar == "String" && value is string stringValue)
                {
                    return String.IsNullOrEmpty(stringValue) ? True : False;
                }
                else if (stringPar == "Collection" && value is IEnumerable<object> enumerableValue)
                {
                    return enumerableValue.Count() == 0 ? True : False;
                }
            }
            return value == null ? True : False;
        }

        public virtual object ConvertBack(object value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
