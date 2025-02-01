using CogniCard.Model;
using OpenAI.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.OpenAI
{
    public static class PromptCreator
    {
        const string OutputScehma = @"
Use the following schema to generate a collection object:
- 'Name': Name of the collection
- 'Description': Description of the topic of the collection
- 'Tags': Comma separated string defining relevant tags for the collection
- 'Flashcards': List of Flashcard objects defined bellow

- 'Title': Descriptive title of the flashcard
- 'Hint': Hint user can reveal for some help
- 'FlashcardType': Intiger, specifies flashcard type
    - Type 0: Classic
        - 'Question': Plain text, additional question info.
        - 'Answer': Plain text, definition/answer.
    - Type 1: SingleChoice
        - 'Question': JSON stringified object of this type
            - 'Question': Plain text, additional question info.
            - 'Choices': List of strings, possible options to pick
        - 'Answer': Index of correct answer as string
    - Type 2: MultiChoice
        - 'Question': JSON stringified object of this type
            - 'Question': Plain text, additional question info.
            - 'Choices': List of strings, possible options to pick
        - 'Answer': Comma-separated indices of the correct options.
    - Type 3: FillTheBlank
        - 'Question': JSON stringified object of this type
            - 'FirstQuestionPart': First part of the sentence.
            - 'LastQuestionPart': Last part of the sentence.
        - 'Answer': Expected answer in between
    - Type 4: TrueOrFalse
        - 'Question': Plain text, additional question info.
        - 'Answer': True or false as lowercase string
";

        static byte[] JSONSchema = @"
{
    ""type"": ""object"",
    ""properties"": {
      ""Name"": {
        ""type"": ""string"",
        ""description"": ""Name of the collection.""
      },
      ""Description"": {
        ""type"": ""string"",
        ""description"": ""Description of the topic of the collection.""
      },
      ""Tags"": {
        ""type"": ""string"",
        ""description"": ""Comma separated string defining relevant tags for the collection.""
      },
      ""Flashcards"": {
        ""type"": ""array"",
        ""description"": ""List of Flashcard objects."",
        ""items"": {
          ""type"": ""object"",
          ""properties"": {
            ""Title"": {
              ""type"": ""string"",
              ""description"": ""Descriptive title of the flashcard.""
            },
            ""Hint"": {
              ""type"": ""string"",
              ""description"": ""Hint user can reveal for some help.""
            },
            ""FlashcardType"": {
              ""type"": ""integer"",
              ""description"": ""Specifies flashcard type.""
            },
            ""Question"": {
              ""type"": ""string"",
              ""description"": ""Specifies question based on defined format.""
            },
            ""Answer"": {
              ""type"": ""string"",
              ""description"": ""Specifies answer based on defined format.""
            }
          },
          ""required"": [
            ""Title"",
            ""Hint"",
            ""FlashcardType"",
            ""Question"",
            ""Answer""
          ],
          ""additionalProperties"": false
        }
      }
    },
    ""required"": [
      ""Name"",
      ""Description"",
      ""Tags"",
      ""Flashcards""
    ],
    ""additionalProperties"": false
}
"u8.ToArray();

        public static string CreatePrompt(string userInput, int numberOfFlashcards, FlashcardType[] flashcardTypes)
        {
            return $@"
{OutputScehma}

Target number of flashcards: {numberOfFlashcards}

Flashcard types: {String.Join(",", flashcardTypes.Select(t => t.ToString()))}

Make sure you don't include any type not listed here!!!

Generare a collection based on the following user input:

{userInput}
";
        }

        public static ChatCompletionOptions CollectionSchemaOptions
        {
            get
            {
                return new ChatCompletionOptions()
                {
                    ResponseFormat = ChatResponseFormat.CreateJsonSchemaFormat(
                            jsonSchemaFormatName: "structured",
                            jsonSchema: BinaryData.FromBytes(JSONSchema),
                            jsonSchemaIsStrict: true
                        )
                };

            }
        }
    }
}
