using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using Google.Apis.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CogniCard.ViewModel.FlashcardListItem
{
    public partial class FillTheBlankFlashcardListItemViewModel : BaseFlashcardListItemViewModel
    {
        [ObservableProperty]
        private string firstQuestionPart;

        [ObservableProperty]
        private string lastQuestionPart;

        public FillTheBlankFlashcardListItemViewModel(Flashcard flashcard) : base(flashcard) 
        {
            if (String.IsNullOrEmpty(flashcard.Question))
            {
                FirstQuestionPart = String.Empty;
                LastQuestionPart = String.Empty;
                return;
            }

            FillTheBlankQuestionJson? question = JsonSerializer.Deserialize<FillTheBlankQuestionJson>(Flashcard.Question!);
            if (question == null) throw new FormatException("Flashcard question inproperly stored");

            firstQuestionPart = question.FirstQuestionPart;
            lastQuestionPart = question.LastQuestionPart;
        }

        partial void OnFirstQuestionPartChanged(string value)
        {
            QuestionChanged();
        }

        partial void OnLastQuestionPartChanged(string value)
        {
            QuestionChanged();
        }

        private void QuestionChanged()
        {
            FillTheBlankQuestionJson fillTheBlankQuestionJson = new()
            {
                FirstQuestionPart = FirstQuestionPart,
                LastQuestionPart = LastQuestionPart,
            };

            string questionString = JsonSerializer.Serialize<FillTheBlankQuestionJson>(fillTheBlankQuestionJson);

            Flashcard.Question = questionString;
        }
    }
}
