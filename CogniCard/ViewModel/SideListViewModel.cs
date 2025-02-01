using CogniCard.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.ViewModel
{
    public partial class SideListViewModel : ObservableObject
    {
        private readonly DataService _dataService;
        public ObservableCollection<SideListItemViewModel> SideItems { get; }
        public AddCollectionViewModel AddCollectionViewModel { get; }
        public bool IsEmpty { get => SideItems.Count == 0; }

        [ObservableProperty]
        private SideListItemViewModel? selected;

        public SideListViewModel(DataService dataService)
        {
            _dataService = dataService;
            SideItems = new (dataService.Collections.Select(c => new SideListItemViewModel(c)));
            AddCollectionViewModel = new(dataService);

            _dataService.Collections.CollectionChanged += (s, e) =>
            {
                if (e.NewItems != null)
                {
                    foreach (var item in e.NewItems)
                    {
                        SideItems.Add(new SideListItemViewModel((Collection)item));
                    }
                }
                if (e.OldItems != null)
                {
                    foreach(var item in e.OldItems)
                    {
                        var itemToRemove = SideItems.FirstOrDefault(vm => vm.Collection.CollectionID == ((Collection)item).CollectionID);
                        if (itemToRemove != null)
                            SideItems.Remove(itemToRemove);
                    }
                }
                OnPropertyChanged(nameof(IsEmpty));
            };

            _dataService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(_dataService.Selected)) OnDataServiceSelectedChanged();
            };

            OnDataServiceSelectedChanged();
        }

        private void OnDataServiceSelectedChanged()
        {
            Selected = SideItems.FirstOrDefault(x => x.Collection == _dataService.Selected);
        }

        partial void OnSelectedChanged(SideListItemViewModel? value)
        {
            Collection? col = value?.Collection;
            if (col != null && col != _dataService.Selected)
            {
                _dataService.Selected = col;
            }
        }
    }
}
