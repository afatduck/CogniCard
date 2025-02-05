using CogniCard.Model;
using CogniCard.Model.Flashcards;
using CogniCard.ViewModel.Review;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class ReviewTabViewModel : ObservableObject
    {
        private DataService _dataService;

        [ObservableProperty]
        private bool collectionSelected;

        [ObservableProperty]
        private bool showButtons;

        private List<Flashcard> ForReview;

        public Flashcard? CurrentlyReviewing { get => ForReview.Count == 0 ? null : ForReview[0]; }

        public ICommand BadCommand { get; set; }
        public ICommand GoodCommand { get; set; }
        public ICommand GreatCommand { get; set; }

        public ReviewTabViewModel(DataService dataService)
        {
            _dataService = dataService;
            ForReview = new List<Flashcard>();
            ResetTab();

            dataService.PropertyChanging += (s, e) =>
            {
                if (e.PropertyName != nameof(_dataService.Selected)) return;
                if (_dataService.Selected == null) return;

                _dataService.Selected.Flashcards.CollectionChanged -= OnFlashcardCollectionChanged;
                foreach (var item in ForReview)
                {
                    item.ReviewViewModel.PropertyChanged -= ReviewPropertyChanged;
                }
            };

            dataService.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName != nameof(_dataService.Selected)) return;
                if (_dataService.Selected == null) return;

                _dataService.Selected.Flashcards.CollectionChanged += OnFlashcardCollectionChanged;
                ResetTab();
            };

            BadCommand = new RelayCommand(OnBad);
            GoodCommand = new RelayCommand(OnGood);
            GreatCommand = new RelayCommand(OnGreat);
        }

        private void OnFlashcardCollectionChanged (object? sender, EventArgs e)
        {
            foreach (var item in ForReview)
            {
                item.ReviewViewModel.PropertyChanged -= ReviewPropertyChanged;
            }
            CreateList();
        }

        private void ReviewPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(BaseReviewViewModel.Revealed))
            {
                ShowButtons = CurrentlyReviewing!.ReviewViewModel.Revealed;
            }
        }

        private void ResetTab()
        {
            ShowButtons = false;
            ForReview.Clear();
            if (_dataService.Selected == null)
            {
                CollectionSelected = false;
                return;
            }

            CollectionSelected = true;
            CreateList();
        }

        private void CreateList()
        {
            var readyForReview = _dataService.Selected!
                .Flashcards
                .Where(f => f.NextReview == null || f.NextReview <= DateTime.Now);
            ForReview.AddRange(readyForReview);
            SortList();

            foreach (var item in ForReview)
            {
                item.ReviewViewModel.ResetVM();
                item.ReviewViewModel.PropertyChanged += ReviewPropertyChanged;
            }
        }

        private void SortList()
        {
            ForReview.Sort((f1, f2) => f2.QueuePriority - f1.QueuePriority);
            OnPropertyChanged(nameof(CurrentlyReviewing));
        }

        private void OnBad()
        {
            ShowButtons = false;
            int minPriority = 0;
            foreach (var f in ForReview)
            {
                if (f.QueuePriority < minPriority) minPriority = f.QueuePriority;
            }
            CurrentlyReviewing!.QueuePriority = minPriority - 1;
            CurrentlyReviewing!.ReviewViewModel.ResetVM();
            SortList();
        }

        private void OnPass(DateTime nextReview)
        {
            ShowButtons = false;
            CurrentlyReviewing!.LastReview = DateTime.Now;
            CurrentlyReviewing!.NextReview = nextReview;
            CurrentlyReviewing!.QueuePriority = 0;
            ForReview.RemoveAt(0);
            OnPropertyChanged(nameof(CurrentlyReviewing));
        }

        private void OnGood()
        {
            OnPass(DateTime.Now.AddDays(1));
        }

        private void OnGreat()
        {
            OnPass(DateTime.Now.AddDays(CurrentlyReviewing!.GreatNextDays));
        }
    }
}
