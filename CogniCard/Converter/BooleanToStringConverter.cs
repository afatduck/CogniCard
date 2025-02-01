using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CogniCard.Converter
{
    public sealed class BooleanToStringConverter : BooleanConverter<string>
    {
        public BooleanToStringConverter() :
            base("true", "false")
        { }
    }
}
