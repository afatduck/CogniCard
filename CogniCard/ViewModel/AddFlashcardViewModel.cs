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
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class AddFlashcardViewModel : ObservableObject
    {
        private DataService _dataService;

        [ObservableProperty]
        private Collection? collection;

        [ObservableProperty]
        private FlashcardType selectedIndex;

        public string[] DropdownItems {  get; private set; }
        public ICommand AddFlashcardCommand { get; set; }

        public event EventHandler<Flashcard>? FlashcardAdded;

        public AddFlashcardViewModel(DataService dataService)
        {
            DropdownItems = Enum.GetValues<FlashcardType>().Select(t => Flashcard.FlashcardTypeNames[t]).ToArray();
            _dataService = dataService;
            AddFlashcardCommand = new RelayCommand(AddFlashcard);
            SelectedIndex = FlashcardType.Classic;
        }

        private void AddFlashcard()
        {
            if (Flashcard.FlashcardClasses.TryGetValue(SelectedIndex, out Type? type))
            {
                Flashcard? newFlashcard = Activator.CreateInstance(type) as Flashcard;

                if (newFlashcard == null) return;

                newFlashcard.FlashcardType = SelectedIndex;
                newFlashcard.Collection = Collection!;
                newFlashcard.Title = "New Flashcard";

                newFlashcard.InitializeViewModels();

                _dataService.AddFlashcard(newFlashcard);

                FlashcardAdded?.Invoke(this, newFlashcard);
            }
        }
    }
}
