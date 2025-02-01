using CogniCard.Model;
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
    public class FlashcardTypeToIntConverter : IValueConverter
    {
        public virtual object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FlashcardType flashcardType)
            {
                return (int)flashcardType;
            }
            throw new ArgumentException("Must be a FlashcardType argument.");
        }

        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int index && Enum.IsDefined(typeof(FlashcardType), index))
            {
                return (FlashcardType)index;
            }
            return FlashcardType.Classic;
            throw new ArgumentException("Must be an int argument in FlashcardType enum range.");
        }
    }
}
