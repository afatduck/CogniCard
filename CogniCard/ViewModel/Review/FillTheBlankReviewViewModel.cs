using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.Review
{
    public partial class FillTheBlankReviewViewModel : BaseReviewViewModel
    {
        [ObservableProperty]
        private string? guess;
        public FillTheBlankQuestionJson QuestionParts { get; private set; }
        public bool IsCorrect { get => (Guess ?? "").ToUpper() == (Flashcard.Answer ?? "").ToUpper(); }
        public FillTheBlankReviewViewModel(Flashcard flashcard) : base(flashcard) 
        {
            QuestionParts = JsonSerializer.Deserialize<FillTheBlankQuestionJson>(flashcard.Question!)!;
        }
        public override void ResetVM()
        {
            base.ResetVM();
            Guess = String.Empty;
        }

        protected override void OnReveal()
        {
            base.OnReveal();
            OnPropertyChanged(nameof(IsCorrect));
        }
    }
}
