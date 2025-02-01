using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CogniCard.Model.Json
{
    public class FillTheBlankQuestionJson
    {
        public required string FirstQuestionPart {  get; set; }
        public required string LastQuestionPart { get; set; }
    }
}
