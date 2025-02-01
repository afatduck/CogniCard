using CogniCard.ViewModel.FlashcardListItem;
using CogniCard.ViewModel.Review;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CogniCard.Model.Flashcards
{
    public class TrueOrFalseFlashcard : Flashcard
    {
        private TrueOrFalseFlashcardListItemViewModel? _listItemViewModel;
        public override BaseFlashcardListItemViewModel ListItemViewModel => _listItemViewModel!;

        private TrueOrFalseReviewViewModel? _reviewViewModel;
        public override BaseReviewViewModel ReviewViewModel => _reviewViewModel!;

        public override void InitializeViewModels()
        {
            _listItemViewModel = new(this);
            _reviewViewModel = new(this);
        }
    }
}
