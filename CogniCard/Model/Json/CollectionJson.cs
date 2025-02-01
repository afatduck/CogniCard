using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Model.Json
{
    public class CollectionJson
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Tags { get; set; }
        public string? UploadedBy { get; set; }
        public DateTime? UploadedAt { get; set; }
        public required List<FlashcardJson> Flashcards { get; set; }
    }
}
