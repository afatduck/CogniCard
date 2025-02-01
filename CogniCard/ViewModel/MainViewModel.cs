using CogniCard.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.ViewModel
{
    public class MainViewModel
    {
        public DataService DataService { get; set; }
        public SideListViewModel SideListViewModel { get; set; }
        public CollectionTabViewModel CollectionTabViewModel { get; set; }
        public ReviewTabViewModel ReviewTabViewModel { get; set; }
        public PublicTabViewModel PublicTabViewModel { get; set; }
        public GenerateTabViewModel GenerateTabViewModel { get; set; }

        public MainViewModel(DataService dataService)
        {
            DataService = dataService;
            SideListViewModel = new(dataService);
            CollectionTabViewModel = new(dataService);
            ReviewTabViewModel = new(dataService);
            PublicTabViewModel = new(dataService);
            GenerateTabViewModel = new(dataService);
        }
    }
}
