using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CogniCard.OpenAI;
using CogniCard.Properties;
using CogniCard.Util;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class GenerateTabViewModel : ObservableObject
    {
        const string InputPlaceholder = "Your input...";
        const string APIKeyPlaceholder = "Your API key...";

        DataService _dataSrvice;

        [ObservableProperty]
        private bool showError;

        [ObservableProperty]
        private string? apiKey;

        [ObservableProperty]
        private bool canUse;

        [ObservableProperty]
        private bool loading;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GenerateCommand))]
        private string? inputText;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(GenerateCommand))]
        private int? numberOfFlashcards;

        public ObservableCollection<ChoiceOption> FlashcardTypes { get; set; }

        public ICommand ConfirmKeyCommand { get; set; }
        public ICommand CancelKeyCommand { get; set; }
        public ICommand ResetKeyCommand { get; set; }
        public IRelayCommand GenerateCommand { get; set; }

        public GenerateTabViewModel(DataService dataService)
        {
            _dataSrvice = dataService;

            ConfirmKeyCommand = new RelayCommand(CheckKey);
            CancelKeyCommand = new RelayCommand(() => { });
            ResetKeyCommand = new RelayCommand(OnResetKey);
            GenerateCommand = new RelayCommand(OnGenerate, CanGenerate);

            ApiKey = Settings.Default.OpenAIKey;
            NumberOfFlashcards = 5;
            CheckKey();

            FlashcardTypes = new();
            foreach (var flashcardTypeName in Flashcard.FlashcardTypeNames.Values)
            {
                FlashcardTypes.Add(new ChoiceOption 
                {
                    Name = flashcardTypeName,
                    IsChecked = true,
                });
            }

            FlashcardTypes.CollectionChanged += (s, e) =>
            {
                GenerateCommand.NotifyCanExecuteChanged();
            };
        }

        private void CheckKey()
        {
            if (String.IsNullOrEmpty(ApiKey) || ApiKey == APIKeyPlaceholder) return;
            Loading = true;
            Task.Run(async () =>
            {
                CanUse = await OpenAIService.ValidateApiKey(ApiKey ?? "");
                Loading = false;

                if (CanUse)
                {
                    Settings.Default.OpenAIKey = ApiKey;
                    Settings.Default.Save();
                    OpenAIService.Initialize(ApiKey!);
                }

                ShowError = true;
            });
        }

        private void OnResetKey()
        {
            CanUse = false;
            ApiKey = "";
            OpenAIService.Reset();
            Settings.Default.OpenAIKey = "";
            Settings.Default.Save();
            ShowError = false;
        }

        private async void OnGenerate()
        {
            List<FlashcardType> flashcardTypes = [];
            for (int i = 0; i < FlashcardTypes.Count; i++)
            {
                var option = FlashcardTypes[i];
                if (option.IsChecked)
                {
                    flashcardTypes.Add((FlashcardType)i);
                }
            }

            Loading = true;
            var collection = await OpenAIService.GenerateCollection(InputText!, NumberOfFlashcards!.Value, [..flashcardTypes]);
            _dataSrvice.Selected = collection;
            _dataSrvice.SelectedTab = 1;
            Loading = false;
        }

        private bool CanGenerate()
        {
            if (String.IsNullOrEmpty(InputText)) return false;
            if (InputPlaceholder == InputText) return false;
            if (NumberOfFlashcards <= 0) return false;
            if (!FlashcardTypes.Any(t => t.IsChecked)) return false;
            return true;
        }

        partial void OnApiKeyChanged(string? value)
        {
            if (String.IsNullOrEmpty(value) || value == APIKeyPlaceholder) ShowError = false;
        }
    }
}
