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
    public class DateToStringConverter : IValueConverter
    {
        public virtual object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime dateTime)
            {
                return dateTime.ToString("dd. MMM yy, HH:mm");
            }
            else if (value is string stringValue)
            {
                DateTime dt;
                DateTime.TryParse(stringValue, out dt);

                return dt.ToString("dd. MMM yy, HH:mm");
            }
            return null;
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
