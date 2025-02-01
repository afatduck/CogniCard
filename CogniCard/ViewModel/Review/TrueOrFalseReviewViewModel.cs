using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CogniCard.ViewModel.Review
{
    public partial class TrueOrFalseReviewViewModel : BaseReviewViewModel
    {
        public bool IsCorrect { get; set; }

        public ICommand TrueCommand { get; set; }
        public ICommand FalseCommand { get; set; }

        public TrueOrFalseReviewViewModel(Flashcard flashcard) : base(flashcard) 
        {
            ShowRevealButton = false;
            TrueCommand = new RelayCommand(OnTrue);
            FalseCommand = new RelayCommand(OnFalse);
        }

        private void OnTrue()
        {
            IsCorrect = Flashcard.Answer == "true";
            OnPropertyChanged(nameof(IsCorrect));
            OnReveal();
        }

        private void OnFalse()
        {
            IsCorrect = Flashcard.Answer != "true";
            OnPropertyChanged(nameof(IsCorrect));
            OnReveal();
        }

        protected override void OnReveal()
        {
            base.OnReveal();
            OnPropertyChanged(nameof(IsCorrect));
        }

        public override void ResetVM()
        {
            base.ResetVM();
            ShowRevealButton = false;
        }
    }
}
