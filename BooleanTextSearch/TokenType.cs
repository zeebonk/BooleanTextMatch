using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BooleanTextSearch
{
    internal enum TokenType
    {
        OpenBrace,
        CloseBrace,
        And,
        Or,
        Literal,
        Whitespace,
        Result,
        Not,
        BinaryOperator,
        LiteralIndex,
    }
}
