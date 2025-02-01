using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.FlashcardListItem
{
    public abstract class BaseFlashcardListItemViewModel : ObservableObject
    {
        public Flashcard Flashcard {  get; set; }

        public BaseFlashcardListItemViewModel(Flashcard flashcard)
        {
            Flashcard = flashcard;
        }
    }
}
