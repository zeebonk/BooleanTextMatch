using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanTextSearch.Interpretation
{
    public enum TokenType
    {
        OpenBrace,
        CloseBrace,
        And,
        Or,
        Literal,
        Whitespace,
        Result,
        Not,
    }
}
