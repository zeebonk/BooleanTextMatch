using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using BooleanTextSearch.Interpreter.Exceptions;

namespace BooleanTextSearch.Interpreter
{
    public class Lexer<T>
    {      
        public Lexer(IEnumerable<LexRule<T>> rules)
        {
            Rules = rules.ToArray();
        }

        public IEnumerable<Token<T>> GetTokens(string query)
        {
            var currentCharacterIndex = 0;

            while (query.Length > 0)
            {
                var result = GetFirstMatchingRuleAndResult(query);

                // When the result is null, no rule has matched and the characters are therefore unexpected
                if (result == null)
                    throw new UnexpectedCharacterException(query[0], currentCharacterIndex);

                // Unpack tuple for readability
                var matchResult = result.Item1;
                var matchRule = result.Item2;

                var token = new Token<T>() { Type = matchRule.ResultType, Index = currentCharacterIndex };
                if (matchResult.Groups.Count >= 2) // Groups[0] is the entire match, Groups[1] is the first real group
                    token.Value = matchResult.Groups[1].Value.Replace(@"\'", "'");

                yield return token;

                query = query.Substring(matchResult.Length);
                currentCharacterIndex += matchResult.Length;
            }
        }

        private Tuple<Match, LexRule<T>> GetFirstMatchingRuleAndResult(string query)
        {
            foreach (var rule in Rules)
            {
                var match = rule.Regex.Match(query);

                if (match.Success)
                    return Tuple.Create(match, rule);
            }

            return null;
        }

        private IEnumerable<LexRule<T>> Rules { get; set; }
    }
}
