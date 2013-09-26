using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BooleanTextSearch.Interpretation
{
    public class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }
        public Expression Expression { get; set; }
        public int Index { get; set; }
    }
}
