using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Util
{
    public partial class ChoiceOption : ObservableObject
    {
        [ObservableProperty]
        private string? name;
        [ObservableProperty]
        private bool isChecked;
    }
}
