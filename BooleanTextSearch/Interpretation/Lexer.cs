using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BooleanTextSearch.Interpretation.Exceptions;

namespace BooleanTextSearch.Interpretation
{
    public class Lexer
    {
        public static readonly LexRule[] RULES = new[] {
            new LexRule(@"^\(", TokenType.OpenBrace),
            new LexRule(@"^\)", TokenType.CloseBrace),
            new LexRule(@"^AND", TokenType.And),
            new LexRule(@"^OR", TokenType.Or),
            new LexRule(@"^'([^']+)'", TokenType.Literal),
            new LexRule(@"^[\r\t\n ]+", TokenType.Whitespace),
        };

        public IEnumerable<Token> GetTokens(string query)
        {
            var currentCharacterIndex = 0;

            while (query.Length > 0)
            {
                var result = GetFirstMatchingRuleAndResult(query);

                // When the result is null, no rule has matched and the characters are therefore unexpected
                if (result == null)
                    throw new UnexpectedCharactersException(currentCharacterIndex);

                // Unpack tuple for readability
                var matchResult = result.Item1;
                var matchRule = result.Item2;

                var token = new Token() { Type = matchRule.ResultType, Index = currentCharacterIndex };
                if (matchResult.Groups.Count >= 2) // Groups[0] is the entire match, Groups[1] is the first real group
                    token.Value = matchResult.Groups[1].Value;

                yield return token;

                query = query.Substring(matchResult.Length);
                currentCharacterIndex += matchResult.Length;
            }
        }

        private Tuple<Match, LexRule> GetFirstMatchingRuleAndResult(string query)
        {
            foreach (var rule in RULES)
            {
                var match = rule.Regex.Match(query);

                if (match.Success)
                    return Tuple.Create(match, rule);
            }

            return null;
        }
    }
}
