using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BooleanTextSearch.Interpretation
{
    public class LexRule
    {
        public LexRule(string regex, TokenType type)
        {
            Regex = new Regex(regex, RegexOptions.Compiled);
            ResultType = type;
        }

        public Regex Regex { get; set; }
        public TokenType ResultType { get; set; }
    }
}
