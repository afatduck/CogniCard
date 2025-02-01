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
    public class AddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double actualWidth && parameter is string amount)
            {
                try
                {
                    double parsed = double.Parse(amount);
                    return actualWidth + parsed;
                }
                catch (Exception) { }
            }

            return value; // Return original value if not applicable
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value; // Not used in this case
        }
    }
}
