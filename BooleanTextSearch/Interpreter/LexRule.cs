using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BooleanTextSearch.Interpreter
{
    public class LexRule<T>
    {
        internal LexRule() { }

        public Regex Regex { get; internal set; }
        public T ResultType { get; internal set; }
    }

    public class LexRuleFactory
    {
        public static LexRule<T> New<T>(string regex, T type)
        {
            var rule = new LexRule<T>();
            rule.Regex = new Regex(regex, RegexOptions.Compiled);
            rule.ResultType = type;
            return rule;
        }
    }
}
