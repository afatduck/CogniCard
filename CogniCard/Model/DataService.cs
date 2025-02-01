using CogniCard.Model.Flashcards;
using CogniCard.Model.Json;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Model
{
    public partial class DataService : ObservableObject
    {
        private ObservableCollection<Collection> _collections;

        private FlashcardContext _dbContext;

        [ObservableProperty]
        private Collection? selected;

        [ObservableProperty]
        private int selectedTab;

        private CancellationTokenSource? _saveDebounceToken;
        public DataService(FlashcardContext flashcardContext)
        {
            _dbContext = flashcardContext;

            _collections = new ObservableCollection<Collection>(
                    _dbContext.Collections.ToList()
                );

            foreach (var collection in _collections)
            {
                collection.PropertyChanged += OnCollectionPropertyChanged;

                collection.Flashcards.CollectionChanged += (s, e) =>
                {
                    collection.UpdateReadyForReviewCount();
                };

                foreach (var flashcard in collection.Flashcards)
                {
                    flashcard.InitializeViewModels();
                    flashcard.PropertyChanged += OnFlashcardPropertyChanged;
                }
            }

            Selected = _collections.Count != 0 ? _collections.First() : null;
            ModelConverter.Initialize(this);
        }

        public ObservableCollection<Collection> Collections
        {
            get => _collections;
            set
            {
                _collections = value;
                OnPropertyChanged(nameof(Collections));
            }
        }

        public void AddCollection(Collection collection)
        {
            Collections.Add(collection);
            _dbContext.Collections.Add(collection);

            collection.PropertyChanged += OnCollectionPropertyChanged;

            CommitChanged();
        }

        public void AddFlashcard(Flashcard flashcard)
        {
            flashcard.Collection.Flashcards.Add(flashcard);
            _dbContext?.Flashcards.Add(flashcard);

            flashcard.PropertyChanged += OnFlashcardPropertyChanged;

            CommitChanged();
        }

        public void DeleteCollection(Collection? collection)
        {
            if (collection == null) return;

            collection.PropertyChanged -= OnCollectionPropertyChanged;

            Collections.Remove(collection);
            _dbContext.Collections.Remove(collection);
            CommitChanged();
            Selected = null;
        }

        public void DeleteFlashcard(Flashcard? flashcard)
        {
            if (flashcard == null) return;

            flashcard.PropertyChanged -= OnFlashcardPropertyChanged;

            flashcard.Collection.Flashcards.Remove(flashcard);

            _dbContext.Flashcards.Remove(flashcard);
            CommitChanged();
        }

        public async void CommitChanged()
        {
            _saveDebounceToken?.Cancel();
            _saveDebounceToken = new CancellationTokenSource();

            try
            {
                await Task.Delay(200, _saveDebounceToken.Token);
                await Task.Run(() => _dbContext.SaveChanges());
            }
            catch (TaskCanceledException) { }
        }

        private void OnCollectionPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CommitChanged();
        }

        private void OnFlashcardPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            CommitChanged();
            if (e.PropertyName ==  nameof(Flashcard.NextReview))
            {
                ((Flashcard)sender!).Collection.UpdateReadyForReviewCount();
            }
        }

    }
}
