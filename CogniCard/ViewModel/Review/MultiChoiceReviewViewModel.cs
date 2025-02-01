using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CogniCard.Model.Util;
using CogniCard.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.Review
{
    public partial class MultiChoiceReviewViewModel : BaseReviewViewModel
    {
        public List<ChoiceOption> GuessOptions { get; private set; }
        public ObservableCollection<CorrectChoiceOption> ResultOptions { get; private set; }
        public string QuestionText { get; private set; }
        public bool IsCorrect
        {
            get
            {
                return ResultOptions.All(o => o.Correct);
            }
        }
        private int[] AnswerIndecies { get; set; }

        public MultiChoiceReviewViewModel(Flashcard flashcard) : base(flashcard)
        {
            var question = JsonSerializer.Deserialize<ChoiceQuestionJson>(flashcard.Question!)!;
            QuestionText = question.Question;
            GuessOptions = [];
            ResultOptions = [];
            foreach (var choice in question.Choices)
            {
                GuessOptions.Add(new ChoiceOption()
                {
                    Name = choice,
                    IsChecked = false
                });
            }

            var split = Flashcard.Answer!.Split(',');
            AnswerIndecies = new int[split.Length];
            for (int i = 0; i < split.Length; i++)
            {
                int index = int.Parse(split[i]);
                AnswerIndecies[i] = index;
            }
        }

        public override void ResetVM()
        {
            base.ResetVM();
            foreach (var item in GuessOptions)
            {
                item.IsChecked = false;
            }
            OnPropertyChanged(nameof(GuessOptions));
        }

        protected override void OnReveal()
        {
            base.OnReveal();

            ResultOptions.Clear();
            for (int i = 0; i < GuessOptions.Count; i++)
            {
                var choice = GuessOptions[i];
                CorrectChoiceOption correctChoiceOption = new()
                {
                    Name = choice.Name,
                    IsChecked = choice.IsChecked,
                    Correct = AnswerIndecies.Contains(i) == choice.IsChecked
                };
                ResultOptions.Add(correctChoiceOption);
            }

            OnPropertyChanged(nameof(IsCorrect));
        }
    }
}
