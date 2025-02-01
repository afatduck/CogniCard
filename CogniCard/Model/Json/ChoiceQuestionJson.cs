using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Model.Json
{
    public class ChoiceQuestionJson
    {
        public required string Question {  get; set; }
        public required string[] Choices { get; set; }
    }
}
