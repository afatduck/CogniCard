using CogniCard.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.ViewModel
{
    public class SideListItemViewModel
    {
        public Collection Collection { get; set; }

        public SideListItemViewModel(Collection collection) 
        {
            Collection = collection;
        }
    }
}
