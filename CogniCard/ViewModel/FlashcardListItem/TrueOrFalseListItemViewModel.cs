using CogniCard.Model;
using CogniCard.Model.Flashcards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.FlashcardListItem
{
    public partial class TrueOrFalseFlashcardListItemViewModel : BaseFlashcardListItemViewModel
    {
        public TrueOrFalseFlashcardListItemViewModel(Flashcard flashcard) : base(flashcard) { }
    }
}
