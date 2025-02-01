using CogniCard.Model.Flashcards;
using CogniCard.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CogniCard.Model.Json
{
    public static class ModelConverter
    {
        static DataService? _dataService;

        public static void Initialize(DataService dataService)
        {
            if (_dataService != null) return;
            _dataService = dataService;
        }

        public static CollectionJson CollectionToJson(Collection collection)
        {
            CollectionJson collectionJson = new()
            {
                Name = collection.Name,
                Description = collection.Description,
                Tags = collection.Tags,
                Flashcards = [],
                UploadedBy = Settings.Default.UploadDisplayName,
                UploadedAt = DateTime.Now,
            };

            foreach (var flashcard in collection.Flashcards)
            {
                FlashcardJson flashcardJson = new()
                {
                    Title = flashcard.Title,
                    Hint = flashcard.Hint,
                    Answer = flashcard.Answer,
                    FlashcardType = flashcard.FlashcardType,
                    Question = flashcard.Question
                };
                collectionJson.Flashcards.Add(flashcardJson);
            }

            return collectionJson;
        }

        public static Collection JsonToCollection(CollectionJson collectionJson, bool save = true)
        {
            if (save && _dataService == null)
            {
                throw new Exception("Model converter needs to be initialized to save collections.");
            }

            Collection collection = new()
            {
                Name = collectionJson.Name,
                Description = collectionJson.Description,
                Tags = collectionJson.Tags,
            };

            if (save) _dataService!.AddCollection(collection);

            foreach (var flashcardJson in collectionJson.Flashcards!)
            {
                if (Flashcard.FlashcardClasses.TryGetValue(flashcardJson.FlashcardType, out Type? type))
                {
                    Flashcard? flashcard = Activator.CreateInstance(type) as Flashcard;
                    if (flashcard == null) continue;

                    flashcard.Answer = flashcardJson.Answer;
                    flashcard.Question = flashcardJson.Question;
                    flashcard.Title = flashcardJson.Title;
                    flashcard.FlashcardType = flashcardJson.FlashcardType;
                    flashcard.Hint = flashcardJson.Hint;
                    flashcard.Collection = collection;

                    try
                    {
                        flashcard.InitializeViewModels();
                    }
                    // Prevents impropertly generated Flashcards from being saved and throwing erros.
                    catch (Exception) { continue; }

                    if (save) _dataService!.AddFlashcard(flashcard);
                }
            }

            return collection;
        }
    }
}
