using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using BooleanTextSearch.Interpreter.Exceptions;

namespace BooleanTextSearch.Interpreter
{
    public class Parser<T>
    {
        public Parser(IEnumerable<ParseRule<T>> rules)
        {
            Rules = rules.ToArray();
            LengthLongestRule = Rules.Max(r => r.Tokens.Length);
            TokenStack = new Stack<Token<T>>();

            // Done at runtime because Enum's are not supported for generic constaints (where T : Enum)
            if (!typeof(T).IsEnum)
                throw new ArgumentException("Only enums are supported as type T");
        }

        public void Parse(Token<T> token)
        {
            TokenStack.Push(token);

            while (TryReduce()) ;
        }

        private bool TryReduce()
        {
            var tokensToPop = Math.Min(TokenStack.Count, LengthLongestRule);
            var currentTokens = new List<Token<T>>();

            var foundOverlap = false;

            for (var i = 0; i < tokensToPop; i++)
            {
                currentTokens.Insert(0, TokenStack.Pop());
                var currentTokenTypes = currentTokens.Select(t => t.Type).ToArray();

                foreach (var rule in Rules)
                {
                    int overlap = GetOverlap(currentTokenTypes, rule.Tokens);

                    if (overlap == rule.Tokens.Length) // Found matching rule
                    {
                        if (rule.SkipMe)
                            return true;

                        var reducedToken = new Token<T>()
                        {
                            Type = rule.ResultType,
                            Expression = rule.Reduction(currentTokens),
                            Index = currentTokens.First().Index,
                        };

                        TokenStack.Push(reducedToken);

                        return true;
                    }
                    else if (overlap > 0) // Partial matching rule
                    {
                        foundOverlap = true;
                    }
                }
            }

            foreach (var t in currentTokens)
                TokenStack.Push(t);

            if (!foundOverlap)
                throw new UnexpectedTokenException<T>(TokenStack.Peek().Type, TokenStack.Peek().Index);

            return false;
        }

        private int GetOverlap(T[] currentTypes, T[] ruleTypes)
        {
            int shortestSequenceLength = Math.Min(currentTypes.Length, ruleTypes.Length);
            int maxOverlap = 0;

            for (var i = 0; i < shortestSequenceLength; i++)
            {
                var currentTypesSub = currentTypes.Reverse().Take(i + 1).Reverse();
                var ruleTypesSub = ruleTypes.Take(i + 1);

                if (currentTypesSub.SequenceEqual(ruleTypesSub))
                    maxOverlap = i + 1;
            }

            return maxOverlap;
        }

        public Expression GetReducedExpression(T expectedRootType)
        {
            if (TokenStack.Count != 1 || !Enum.Equals(TokenStack.Peek().Type, expectedRootType))
                throw new IncompleteExpressionException();

            return TokenStack.Peek().Expression;
        }

        private ParseRule<T>[] Rules { get; set; }
        private Stack<Token<T>> TokenStack { get; set; }
        private int LengthLongestRule { get; set; }
    }
}
