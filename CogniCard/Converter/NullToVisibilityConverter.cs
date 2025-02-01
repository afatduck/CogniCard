using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CogniCard.Converter
{
    public sealed class NullToVisibilityConverter : NullConverter<Visibility>
    {
        public NullToVisibilityConverter() :
            base(Visibility.Visible, Visibility.Collapsed)
        { }
    }
}
