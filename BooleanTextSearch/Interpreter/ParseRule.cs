using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace BooleanTextSearch.Interpreter
{
    public class ParseRule<T>
    {
        internal ParseRule() { }

        public bool SkipMe { get; internal set; }
        public T[] Tokens { get; internal set; }
        public T ResultType { get; internal set; }
        public Func<List<Token<T>>, Expression> Reduction { get; internal set; }
    }

    public class ParseRuleFactory
    {
        public static ParseRule<T> NewNormal<T>(T[] tokens, T resultType, Func<List<Token<T>>, Expression> reduction)
        {
            var rule = new ParseRule<T>();
            rule.Tokens = tokens;
            rule.ResultType = resultType;
            rule.Reduction = reduction;
            return rule;
        }

        public static ParseRule<T> NewSkip<T>(T[] tokens)
        {
            var rule = new ParseRule<T>();
            rule.Tokens = tokens;
            rule.SkipMe = true;
            return rule;
        }
    }
}
