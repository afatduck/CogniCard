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
    public abstract partial class BaseReviewViewModel : ObservableObject
    {
        public Flashcard Flashcard { get; set; }

        [ObservableProperty]
        private bool showRevealButton;

        [ObservableProperty]
        private bool revealed;

        [ObservableProperty]
        private bool showHint;

        public ICommand RevealCommand { get; set; }
        public ICommand ShowHintCommand { get; set; }

        public BaseReviewViewModel(Flashcard flashcard) 
        {
            ShowRevealButton = true;
            Flashcard = flashcard;
            Revealed = false;
            ShowHint = false;
            RevealCommand = new RelayCommand(OnReveal);
            ShowHintCommand = new RelayCommand(() => ShowHint = true);
        }

        protected virtual void OnReveal()
        {
            Revealed = true;
            ShowRevealButton = false;
        }

        public virtual void ResetVM()
        {
            Revealed = false;
            ShowHint = false;
            ShowRevealButton = true;
        }
    }
}
