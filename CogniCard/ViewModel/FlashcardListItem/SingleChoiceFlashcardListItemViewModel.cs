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
    public partial class SingleChoiceFlashcardListItemViewModel : BaseFlashcardListItemViewModel
    {

        private static readonly string[] DefaultOptions = { "Choice 1", "Choice 2" };

        public ObservableCollection<ChoiceOption> Options { get; set; }

        [ObservableProperty]
        private string questionText; 
        public ICommand DeleteOptionCommand { get; set; }
        public ICommand AddOptionCommand {  get; set; }

        public SingleChoiceFlashcardListItemViewModel(Flashcard flashcard) : base(flashcard)
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
                    question.Choices.Select(c => new ChoiceOption() {
                        Name = c,
                        IsChecked = false
                    }).ToArray());
                QuestionText = question.Question;
            }
            else
            {
                Options = new ObservableCollection<ChoiceOption>(
                    DefaultOptions.Select(c => new ChoiceOption() {
                        Name = c,
                        IsChecked = false
                    }).ToArray());
                QuestionText = "Pick one...";
            }

            int answer = int.Parse(Flashcard.Answer ?? "0");
            Options[answer].IsChecked = true;

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
                if (option.IsChecked) Options.First().IsChecked = true;
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
            if (e.PropertyName == nameof(ChoiceOption.IsChecked) && sender is ChoiceOption selected && selected.IsChecked)
            {
                foreach (var option in Options)
                {
                    option.IsChecked = option == selected;
                }
            }
            SetToQuestion();
        }

        private void SetToQuestion()
        {
            int index = Options.ToList().FindIndex(c => c.IsChecked);
            if (index < 0) return;

            Flashcard.Answer = index.ToString();
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
