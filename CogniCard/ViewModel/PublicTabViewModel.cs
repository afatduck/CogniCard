using CogniCard.Firebase;
using CogniCard.Model;
using CogniCard.Model.Json;
using CogniCard.Properties;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CogniCard.ViewModel
{
    public partial class PublicTabViewModel : ObservableObject
    {
        public const string TagSearchPlaceholder = "Search by tags (seperate tags with commas)...";

        private DataService _dataService;
        private FirebaseService _firebaseService;

        [ObservableProperty]
        private string? displayName;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(UploadCommand))]
        private Collection? collectionToUpload;

        [ObservableProperty]
        private string? tagsSearch;

        public ObservableCollection<Collection> Collections { get => _dataService.Collections; }
        public ObservableCollection<CollectionJson> OnlineCollections { get; set; }
        public List<CollectionJson> FilteredCollections
        {
            get
            {
                var allCollections = OnlineCollections.ToList();
                allCollections.Sort((c1, c2) => DateTime.Compare(c2.UploadedAt ?? DateTime.Now, c1.UploadedAt ?? DateTime.Now));

                if (String.IsNullOrEmpty(TagsSearch) || TagsSearch == TagSearchPlaceholder) return allCollections;

                List<CollectionJson> filtered = [];
                List<string> tagList = TagsSearch?
                    .Split(',')
                    .Select(t => t.Trim())
                    .Where(t => !String.IsNullOrEmpty(t))
                    .ToList() ?? [];

                foreach (var collection in allCollections)
                {
                    List<string> collectionTagList = collection.Tags?
                        .Split(',')
                        .Select(t => t.Trim())
                        .Where(t => !String.IsNullOrEmpty(t))
                        .ToList() ?? [];
                    bool add = true;

                    foreach (var tag in tagList)
                    {
                        if (tag == tagList.Last())
                        {
                            if (!collectionTagList.Any(t => t.StartsWith(tag)))
                            {
                                add = false;
                                break;
                            }
                        }
                        else if (!collectionTagList.Contains(tag))
                        {
                            add = false;
                            break;
                        }
                    }
                    if (add) filtered.Add(collection);
                }

                return filtered;
            }
        }

        public IRelayCommand UploadCommand { get; set; }
        public ICommand SaveCommand { get; set; }
        public ICommand RefreshCommand { get; set; }

        public PublicTabViewModel(DataService dataService)
        {
            _dataService = dataService;
            _firebaseService = new();

            OnlineCollections = [];
            CollectionToUpload = null;

            UploadCommand = new RelayCommand(OnUpload, CanUpload);
            SaveCommand = new RelayCommand<CollectionJson>(OnSave);
            RefreshCommand = new RelayCommand(Retrive);

            OnlineCollections.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(FilteredCollections));
            };

            _dataService.Collections.CollectionChanged += (s, e) =>
            {
                OnPropertyChanged(nameof(Collections));
            };

            DisplayName = Settings.Default.UploadDisplayName;

            Task.Run(() => Retrive());
        }

        private bool CanUpload() => CollectionToUpload != null;
        private async void Retrive()
        {
            OnlineCollections.Clear();
            var onlineCollections = await _firebaseService.GetCollections();
            if (onlineCollections == null) return;
            foreach (var collectionJson in onlineCollections)
            {
                OnlineCollections.Add(collectionJson);
            }
        }

        private async void OnUpload()
        {
            if (!(MessageBox.Show(
                "Once the collection is uploaded it cannot be modified or deleted, do you wish to proceed?",
                "Upload collection",
                MessageBoxButton.OKCancel,
                MessageBoxImage.Question
                ) == MessageBoxResult.OK)) return;


            var collectionJson = ModelConverter.CollectionToJson(CollectionToUpload!);
            await _firebaseService.UploadCollection(collectionJson);
            OnlineCollections.Add(collectionJson);
            CollectionToUpload = null;
        }

        private void OnSave(object? parameter)
        {
            if (parameter is CollectionJson collectionJson)
            {
                var collection = ModelConverter.JsonToCollection(collectionJson, true);
                _dataService.Selected = collection;
                _dataService.SelectedTab = 1;
            }
        }

        partial void OnDisplayNameChanged(string? value)
        {
            Settings.Default.UploadDisplayName = value;
            Settings.Default.Save();
        }

        partial void OnTagsSearchChanged(string? value)
        {
            OnPropertyChanged(nameof(FilteredCollections));
        }
    }
}
