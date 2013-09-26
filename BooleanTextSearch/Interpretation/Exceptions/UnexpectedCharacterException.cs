using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanTextSearch.Interpretation.Exceptions
{
    public class UnexpectedCharacterException : Exception
    {
        public UnexpectedCharacterException(char character, int characterIndex)
            : base(string.Format("Unexpected character '{0}' at position '{1}'", character, characterIndex))
        {
            Character = character;
            CharacterIndex = characterIndex;
        }

        public char Character { get; set; }
        public int CharacterIndex { get; set; }
    }
}
