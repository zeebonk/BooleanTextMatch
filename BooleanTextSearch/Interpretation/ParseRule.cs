using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BooleanTextSearch.Interpretation
{
    public class ParseRule
    {
        public ParseRule(TokenType[] tokens, TokenType resultType, Func<List<Token>, Expression> reduction)
        {
            Tokens = tokens;
            ResultType = resultType;
            Reduction = reduction;
        }

        public TokenType[] Tokens { get; set; }
        public TokenType ResultType { get; set; }
        public Func<List<Token>, Expression> Reduction { get; set; }
    }
}
