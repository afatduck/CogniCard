using CogniCard.ViewModel.FlashcardListItem;
using CogniCard.ViewModel.Review;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CogniCard.Model.Flashcards
{
    public abstract partial class Flashcard : ObservableObject
    {
        [ObservableProperty]
        private uint flashcardID;

        [ObservableProperty]
        private FlashcardType flashcardType;

        [ObservableProperty]
        private DateTime? lastReview;

        [ObservableProperty]
        private DateTime? nextReview;

        [ObservableProperty]
        private string? title;

        [ObservableProperty]
        private string? hint;

        [ObservableProperty]
        private string? answer;

        [ObservableProperty]
        private string? question;

        [ObservableProperty]
        private int queuePriority;

        public required virtual Collection Collection { get; set; }

        [NotMapped]
        public uint DaysSinceLastReview
        {
            get
            {
                if (NextReview == null || LastReview == null)
                {
                    return 0;
                }
                int? difference = (((DateTime)NextReview).AddMinutes(1) - LastReview)?.Days;
                if (difference.HasValue && difference >= 0)
                {
                    return (uint)difference.Value;
                }
                throw new Exception("Last review cannot be after next review.");
            }
        }

        [NotMapped]
        public uint GreatNextDays
        {
            get
            {
                var d = DaysSinceLastReview;
                return d == 0 ? 2 : d * 2;
            }
        }

        [NotMapped]
        public string FlashcardTypeName { get => Flashcard.FlashcardTypeNames[FlashcardType]; }

        [NotMapped]
        public static Dictionary<FlashcardType, string> FlashcardTypeNames = new()
        {
            { FlashcardType.Classic, "Classic" },
            { FlashcardType.SingleChoice, "Single choice" },
            { FlashcardType.MultiChoice, "Multi choice" },
            { FlashcardType.FillTheBlank, "Fill in the blank" },
            { FlashcardType.TrueOrFalse, "True or false" }
        };

        [NotMapped]
        public abstract BaseFlashcardListItemViewModel ListItemViewModel { get; }

        [NotMapped]
        public abstract BaseReviewViewModel ReviewViewModel { get; }

        [NotMapped]
        public static Dictionary<FlashcardType, Type> FlashcardClasses = new()
        {
            { FlashcardType.Classic, typeof(ClassicFlashcard) },
            { FlashcardType.SingleChoice, typeof(SingleChoiceFlashcard) },
            { FlashcardType.MultiChoice, typeof(MultiChoiceFlashcard) },
            { FlashcardType.FillTheBlank, typeof(FillTheBlankFlashcard) },
            { FlashcardType.TrueOrFalse, typeof(TrueOrFalseFlashcard) }
        };
        public abstract void InitializeViewModels();
    }
}
