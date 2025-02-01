using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class FlashcardListItemViewModel : ObservableObject
    {
        private DataService _dataService;
        public Flashcard Flashcard { get; set; }

        [ObservableProperty]
        private bool selected;

        [ObservableProperty]
        private string? newTitle;

        [ObservableProperty]
        private string? newHint;

        public ICommand ConfirmNewTitleCommand { get; set; }
        public ICommand CancelNewTitleCommand { get; set; }
        public ICommand ConfirmNewHintCommand { get; set; }
        public ICommand CancelNewHintCommand { get; set; }
        public ICommand DeleteFlashcardCommand { get; set; }

        public EventHandler<uint>? FlashcardDeleted;

        public FlashcardListItemViewModel(DataService dataService, Flashcard flashcard)
        {
            _dataService = dataService;
            Flashcard = flashcard;

            NewTitle = Flashcard.Title;
            NewHint = Flashcard.Hint;

            ConfirmNewTitleCommand = new RelayCommand(() => Flashcard.Title = NewTitle);
            CancelNewTitleCommand = new RelayCommand(() => NewTitle = Flashcard.Title);

            ConfirmNewHintCommand = new RelayCommand(() => Flashcard.Hint = NewHint);
            CancelNewHintCommand = new RelayCommand(() => NewHint = Flashcard.Hint);

            DeleteFlashcardCommand = new RelayCommand(DeleteFlashcard);
        }

        private void DeleteFlashcard()
        {
            FlashcardDeleted?.Invoke(this, Flashcard.FlashcardID);
            _dataService.DeleteFlashcard(Flashcard);
        }
    }
}
