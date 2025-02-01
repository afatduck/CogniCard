using CogniCard.Model;
using CogniCard.Model.Json;
using OpenAI;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CogniCard.OpenAI
{
    public static class OpenAIService
    {
        static ChatClient? _client;

        public static void Initialize(string apiKey)
        {
            _client = new (
                    model: "gpt-4o",
                    apiKey: apiKey
                );
        }

        public static void Reset()
        {
            _client = null;
        }

        public static async Task<Collection> GenerateCollection(string userInput, int numberOfFlashcard, FlashcardType[] flashcardTypes)
        {
            if (_client == null)
            {
                throw new InvalidOperationException("Service not initialized");
            }

            string prompt = PromptCreator.CreatePrompt(userInput, numberOfFlashcard, flashcardTypes);
            ChatMessage[] messages =
            [
                new SystemChatMessage("You are a flashcard collection generator."),
                new UserChatMessage(prompt)
            ];

            var completionResult = await _client.CompleteChatAsync(messages, PromptCreator.CollectionSchemaOptions);

            var collectionJson = JsonSerializer.Deserialize<CollectionJson>(completionResult.Value.Content[0].Text);

            return ModelConverter.JsonToCollection(collectionJson!, true);
        }

        public static async Task<bool> ValidateApiKey(string apiKey)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");
            try
            {
                var response = await client.GetAsync("https://api.openai.com/v1/models");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}
