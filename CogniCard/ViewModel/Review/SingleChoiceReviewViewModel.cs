using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CogniCard.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.Review
{
    public partial class SingleChoiceReviewViewModel : BaseReviewViewModel
    {
        public List<ChoiceOption> GuessOptions { get; private set; }
        public string QuestionText { get; private set; }
        public string AnswerText { get; private set; }
        public string? Picked { get; private set; }

        public bool IsCorrect 
        {
            get => int.Parse(Flashcard.Answer!) == GuessOptions.FindIndex(o => o.IsChecked);
        }
        public SingleChoiceReviewViewModel(Flashcard flashcard) : base(flashcard)
        {
            var question = JsonSerializer.Deserialize<ChoiceQuestionJson>(flashcard.Question!)!;
            QuestionText = question.Question;
            GuessOptions = [];
            foreach (var choice in question.Choices)
            {
                GuessOptions.Add(new ChoiceOption()
                {
                    Name = choice,
                    IsChecked = GuessOptions.Count == 0
                });
            }
            AnswerText = GuessOptions[int.Parse(Flashcard.Answer!)].Name!;
        }

        public override void ResetVM()
        {
            base.ResetVM();
            bool first = true;
            foreach (var item in GuessOptions)
            {
                item.IsChecked = first;
                first = false;
            }
            OnPropertyChanged(nameof(GuessOptions));
        }

        protected override void OnReveal()
        {
            base.OnReveal();
            Picked = GuessOptions.Where(o => o.IsChecked).First().Name;
            OnPropertyChanged(nameof(IsCorrect));
            OnPropertyChanged(nameof(Picked));
        }
    }
}
