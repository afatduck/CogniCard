using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.Review
{
    public partial class ClassicReviewViewModel : BaseReviewViewModel
    {
        public string PlaceholderText = "Type in your guess (optional)...";

        [ObservableProperty]
        private string? guess;

        public ClassicReviewViewModel(Flashcard flashcard) : base(flashcard) { }

        public override void ResetVM()
        {
            base.ResetVM();
            Guess = String.Empty;
        }

        protected override void OnReveal()
        {
            base.OnReveal();
            if (Guess == PlaceholderText) Guess = String.Empty;
        }
    }
}
