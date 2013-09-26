using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanTextSearch.Interpretation.Exceptions
{
    public class UnexpectedCharactersException : Exception
    {
        public UnexpectedCharactersException(int characterIndex)
            : base(string.Format("Unexpected characters at {0}", characterIndex))
        {
            CharacterIndex = characterIndex;
        }

        public int CharacterIndex { get; set; }
    }
}
