using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class CollectionTabViewModel : ObservableObject
    {
        private DataService _dataService;

        [ObservableProperty]
        private Collection? selected;

        [ObservableProperty]
        private bool noneSelected;

        [ObservableProperty]
        private bool editingTags;

        [ObservableProperty]
        private string? newTags;

        public FlashcardListViewModel FlashcardListViewModel { get; private set; }

        public AddFlashcardViewModel AddFlashcardViewModel { get; private set; }

        public ICommand EditTagsCommand { get; set; }
        public ICommand ConfirmTagsCommand { get; set; }
        public ICommand CancelTagsCommand { get; set; }
        public ICommand DeleteCollectionCommand { get; set; }

        public CollectionTabViewModel(DataService dataService)
        {
            _dataService = dataService;
            NoneSelected = dataService.Selected == null;
            FlashcardListViewModel = new(dataService);
            AddFlashcardViewModel = new(dataService);
            EditingTags = false;

            AddFlashcardViewModel.Collection = Selected;
            AddFlashcardViewModel.FlashcardAdded += OnFlashcardAdded;

            dataService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(_dataService.Selected)) return;
                Selected = _dataService.Selected;
                NoneSelected = Selected == null;
                EditingTags = false;
            };

            Selected = _dataService.Selected;
            NoneSelected = Selected == null;

            EditTagsCommand = new RelayCommand(OnEditTagsButtonPress, () => !EditingTags);
            ConfirmTagsCommand = new RelayCommand(OnTagsConfirm, () => EditingTags);
            CancelTagsCommand = new RelayCommand(OnTagsCancel, () => EditingTags);
            DeleteCollectionCommand = new RelayCommand(() => _dataService.DeleteCollection(selected));
        }

        private void OnTagsConfirm()
        {
            if (Selected != null) Selected.Tags = NewTags;
            EditingTags = false;
        }

        private void OnTagsCancel()
        {
            EditingTags = false;
        }

        private void OnEditTagsButtonPress()
        {
            EditingTags = true;
            NewTags = Selected?.Tags?.ToString();
        }

        partial void OnSelectedChanged(Collection? value)
        {
            AddFlashcardViewModel.Collection = value;
        }

        private void OnFlashcardAdded(object? sender, Flashcard flashcard)
        {
            FlashcardListViewModel.Selected = FlashcardListViewModel.FlashcardListItems.FirstOrDefault(item => item.Flashcard == flashcard);
        }
    }
}
