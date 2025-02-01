using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CogniCard.Model.Json
{
    public class FlashcardJson
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public FlashcardType FlashcardType { get; set; }
        public string? Title { get; set; }
        public string? Hint { get; set; }
        public string? Answer { get; set; }
        public string? Question { get; set; }
    }
}
