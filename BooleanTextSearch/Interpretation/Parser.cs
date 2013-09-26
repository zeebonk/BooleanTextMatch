using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using BooleanTextSearch.Interpretation.Exceptions;

namespace BooleanTextSearch.Interpretation
{
    public class Parser
    {
        public static readonly ParseRule[] RULES = new[] {
            new ParseRule(
                new[] { TokenType.Literal }, 
                TokenType.Result,
                (tokens) => Expression.Call(InputParameter, typeof(string).GetMethod("Contains"), Expression.Constant(tokens[0].Value))
            ),
            new ParseRule(
                new[] { TokenType.Result, TokenType.And, TokenType.Result }, 
                TokenType.Result,
                (tokens) => Expression.And(tokens[0].Expression, tokens[2].Expression)
            ),
            new ParseRule(
                new[] { TokenType.Result, TokenType.Or, TokenType.Result }, 
                TokenType.Result,
                (tokens) => Expression.Or(tokens[0].Expression, tokens[2].Expression)
            ),
            new ParseRule(
                new[] { TokenType.OpenBrace, TokenType.Result, TokenType.CloseBrace }, 
                TokenType.Result,
                (tokens) => tokens[1].Expression
            ),
        };

        public Parser()
        {
            TokenStack = new Stack<Token>();
        }

        public void Parse(Token token)
        {
            if (token.Type == TokenType.Whitespace)
                return;

            TokenStack.Push(token);

            while (TryReduce()) ;
        }

        public bool TryReduce()
        {
            var tokensToPop = Math.Min(TokenStack.Count, LONGEST_RULE);
            var currentTokens = new List<Token>();

            var foundOverlap = false;

            for (var i = 0; i < tokensToPop; i++)
            {
                currentTokens.Insert(0, TokenStack.Pop());
                var currentTokenTypes = currentTokens.Select(t => t.Type).ToArray();

                foreach (var rule in RULES)
                {
                    int overlap = GetOverlap(currentTokenTypes, rule.Tokens);

                    if (overlap == rule.Tokens.Length) // Found matching rule
                    {
                        var reducedToken = new Token()
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
                throw new UnexpectedTokenException(TokenStack.Peek().Type, TokenStack.Peek().Index);

            return false;
        }

        public int GetOverlap(TokenType[] currentTypes, TokenType[] ruleTypes)
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

        public Func<string, bool> GetExpression()
        {
            var lambda = Expression.Lambda<Func<string, bool>>(
                TokenStack.Peek().Expression,
                InputParameter
            );

            return lambda.Compile();
        }

        public Stack<Token> TokenStack { get; set; }
        public static readonly int LONGEST_RULE = RULES.Max(r => r.Tokens.Length);
        public static readonly ParameterExpression InputParameter = Expression.Parameter(typeof(string), "value");
    }
}
