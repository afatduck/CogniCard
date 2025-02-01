using CogniCard.Model;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class AddCollectionViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private string newCollectionName;

        [ObservableProperty]
        private bool currentlyAdding;

        public ICommand ConfirmCommand { get; set; }
        public ICommand CancelCommand { get; set; }
        public ICommand AddPressedCommand { get; set; }

        public AddCollectionViewModel(DataService dataService)
        {
            _dataService = dataService;
            NewCollectionName = string.Empty;
            CurrentlyAdding = false;

            ConfirmCommand = new RelayCommand(OnConfirm, () => CurrentlyAdding);
            CancelCommand = new RelayCommand(OnCancel, () => CurrentlyAdding);
            AddPressedCommand = new RelayCommand(OnAddPressed, () => !CurrentlyAdding);
        }

        private void OnConfirm()
        {
            if (NewCollectionName == string.Empty)
            {
                CurrentlyAdding = false;
                return;
            }

            Collection newCollection = new()
            {
                Name = NewCollectionName,
                Description = String.Empty,
                Tags = String.Empty
            };

            _dataService.AddCollection(newCollection);
            _dataService.CommitChanged();

            _dataService.Selected = newCollection;

            OnCancel();
        }

        private void OnCancel()
        {
            NewCollectionName = String.Empty;
            CurrentlyAdding = false;
        }

        private void OnAddPressed()
        {
            CurrentlyAdding = true;
        }

    }
}
