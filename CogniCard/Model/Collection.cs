using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Model
{
    public partial class Collection : ObservableObject
    {
        [ObservableProperty]
        private uint collectionID;

        [ObservableProperty]
        private string? name;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? tags;
        public virtual ObservableCollection<Flashcard> Flashcards { get; private set; } = new();

        [NotMapped]
        public int ReadyForReviewCount { get => Flashcards.Where(f => f.NextReview == null || f.NextReview <= DateTime.Now).Count(); }

        public void UpdateReadyForReviewCount ()
        {
            OnPropertyChanged(nameof(ReadyForReviewCount));
        }
        partial void OnNameChanging(string? oldValue, string? newValue)
        {
            if (newValue == String.Empty)
            {
                throw new ArgumentException("Name cannot be empty.");
            }
        }
    }
}
