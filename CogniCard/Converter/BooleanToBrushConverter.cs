using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CogniCard.Converter
{
    public sealed class BooleanToBrushConverter : BooleanConverter<SolidColorBrush>
    {
        public BooleanToBrushConverter() :
            base(Brushes.LimeGreen, Brushes.Tomato)
        { }
    }
}
