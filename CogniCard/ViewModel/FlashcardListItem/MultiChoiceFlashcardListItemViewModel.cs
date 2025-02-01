using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CogniCard.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CogniCard.ViewModel.FlashcardListItem
{
    public partial class MultiChoiceFlashcardListItemViewModel : BaseFlashcardListItemViewModel
    {

        private static readonly string[] DefaultOptions = { "Choice 1", "Choice 2" };

        public ObservableCollection<ChoiceOption> Options { get; set; }

        [ObservableProperty]
        private string questionText;
        public ICommand DeleteOptionCommand { get; set; }
        public ICommand AddOptionCommand { get; set; }

        public MultiChoiceFlashcardListItemViewModel(Flashcard flashcard) : base(flashcard)
        {
            DeleteOptionCommand = new RelayCommand<object>(DeleteOption, (object? o) => Options!.Count > 1);
            AddOptionCommand = new RelayCommand(AddOption);

            ChoiceQuestionJson? question;
            try
            {
                question = JsonSerializer.Deserialize<ChoiceQuestionJson?>(Flashcard.Question ?? String.Empty);
            }
            catch (JsonException)
            {
                question = null;
            }

            if (question != null)
            {
                Options = new ObservableCollection<ChoiceOption>(
                    question.Choices.Select(c => new ChoiceOption()
                    {
                        Name = c,
                        IsChecked = false
                    }).ToArray());
            }
            else
            {
                Options = new ObservableCollection<ChoiceOption>(
                    DefaultOptions.Select(c => new ChoiceOption()
                    {
                        Name = c,
                        IsChecked = false
                    }).ToArray());
            }

            if (!String.IsNullOrEmpty(Flashcard.Answer))
            {
                foreach(var choice in Flashcard.Answer.Split(','))
                {
                    int choiceIndex = -1;
                    int.TryParse(choice, out choiceIndex);
                    if (choiceIndex != -1)
                    {
                        Options[choiceIndex].IsChecked = true;
                    }
                }
            }

            QuestionText = question != null ? question.Question : "Pick one...";

            foreach (var option in Options)
            {
                option.PropertyChanged += OptionPropertyChanged;
            }

            Options.CollectionChanged += (s, e) => SetToQuestion();
        }

        private void DeleteOption(object? parameter)
        {
            if (parameter is ChoiceOption option)
            {
                Options.Remove(option);
            }
        }

        private void AddOption()
        {
            ChoiceOption newOption = new()
            {
                Name = "New choice",
                IsChecked = false,
            };
            newOption.PropertyChanged += OptionPropertyChanged;
            Options.Add(newOption);
        }

        private void OptionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            SetToQuestion();
        }

        private void SetToQuestion()
        {
            List<int> answers = new List<int>();
            for (int i = 0; i  < Options.Count; i++)
            {
                var option = Options[i];
                if (option.IsChecked)
                {
                    answers.Add(i);
                }
            }
            Flashcard.Answer = String.Join(",", answers);

            ChoiceQuestionJson choiceQuestion = new()
            {
                Choices = Options.Select(c => c.Name!).ToArray(),
                Question = QuestionText
            };
            var choiceQuestionJson = JsonSerializer.Serialize<ChoiceQuestionJson>(choiceQuestion);
            Flashcard.Question = choiceQuestionJson;
        }

        partial void OnQuestionTextChanged(string value)
        {
            SetToQuestion();
        }
    }
}
