using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanTextSearch.Interpretation.Exceptions
{
    public class UnexpectedTokenException : Exception
    {
        public UnexpectedTokenException(TokenType type, int index)
            : base(string.Format("Unexpected token '{0}' at position '{1}'", type, index))
        {
            Type = type;
            Index = index;
        }

        public TokenType Type { get; set; }
        public int Index { get; set; }
    }
}
