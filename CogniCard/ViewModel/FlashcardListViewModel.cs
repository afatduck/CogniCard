using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CogniCard.ViewModel
{
    public partial class FlashcardListViewModel : ObservableObject
    {
        private readonly DataService _dataService;

        [ObservableProperty]
        private FlashcardListItemViewModel? selected;

        [ObservableProperty]
        private bool isEmpty;

        public ObservableCollection<FlashcardListItemViewModel> FlashcardListItems { get; private set; }

        public FlashcardListViewModel (DataService dataService)
        {
            _dataService = dataService;
            FlashcardListItems = new();

            dataService.PropertyChanging += (s, e) =>
            {
                if (e.PropertyName != nameof(dataService.Selected)) return;
                if (_dataService.Selected == null) return;

                _dataService.Selected.Flashcards.CollectionChanged -= OnFlashcardsCollectionChanged;
            };

            dataService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(dataService.Selected)) return;
                OnCollectionSelectedChanged();
            };

            OnCollectionSelectedChanged();

            FlashcardListItems.CollectionChanged += (s, e) =>
            {
                IsEmpty = FlashcardListItems.Count == 0;
            };
        }

        private void OnCollectionSelectedChanged()
        {
            FlashcardListItems.Clear();
            Selected = null;
            if (_dataService.Selected == null) return;
            var newItems = _dataService.Selected
                .Flashcards
                .Select(f => new FlashcardListItemViewModel(_dataService, f));
            foreach (var item in newItems)
            {
                FlashcardListItems.Add(item);
            }
            _dataService.Selected.Flashcards.CollectionChanged += OnFlashcardsCollectionChanged;
        }

        private void OnFlashcardsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    if (item is Flashcard flashcard)
                    {
                        FlashcardListItems.Add(new FlashcardListItemViewModel(_dataService, flashcard));
                    }
                }
            }
            if (e.OldItems != null)
            {
                foreach(var item in e.OldItems)
                {
                    if (item is Flashcard flashcard)
                    {
                        var toRemove = FlashcardListItems.Where(f => f.Flashcard == flashcard).ToArray();
                        foreach (var itemToRemove in toRemove)
                        {
                            FlashcardListItems.Remove(itemToRemove);
                        }
                    }
                }
            }
        }

        partial void OnSelectedChanged(FlashcardListItemViewModel? value)
        {
            foreach (var item in FlashcardListItems)
            {
                item.Selected = item == value;
            }
        }
    }
}
