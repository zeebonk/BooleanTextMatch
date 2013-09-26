using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooleanTextSearch.Interpretation;

namespace BooleanTextSearch
{
    public class BooleanTextSearchFactory
    {
        public static Func<string, bool> New(string query)
        {
            var lexer = new Lexer();
            var parser = new Parser();

            foreach (var token in lexer.GetTokens(query))
                parser.Parse(token);

            return parser.GetExpression();
        }
    }
}
