using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooleanTextSearch.Interpreter;
using System.Linq.Expressions;

namespace BooleanTextSearch
{
    public class TextMatcher
    {
        private Func<string, StringComparison, bool> CompiledMatchExpression { get; set; }
        public string Query { get; private set; }
        public StringComparison StringComparison { get; private set; }

        // Private constructor, used by factory method New
        private TextMatcher(Func<string, StringComparison, bool> compiledMatchExpression, StringComparison stringComparison)
        {
            CompiledMatchExpression = compiledMatchExpression;
            StringComparison = stringComparison;
        }

        public bool Matches(string text)
        {
            return CompiledMatchExpression(text, StringComparison);
        }

        // Factory method
        public static TextMatcher New(string query, StringComparison compareType)
        {
            var lexer = new Lexer<TokenType>(LEX_RULES);
            var parser = new Parser<TokenType>(PARSE_RULES);

            foreach (var token in lexer.GetTokens(query))
                parser.Parse(token);

            var expression = GetCompiledExpression(parser);

            return new TextMatcher(expression, compareType);
        }

        // Expression builder
        private static Func<string, StringComparison, bool> GetCompiledExpression(Parser<TokenType> parser)
        {
            var expression = parser.GetReducedExpression(TokenType.Result);

            var lambda = Expression.Lambda<Func<string, StringComparison, bool>>(
                expression,
                INPUT_PARAMETER,
                STRING_COMPARISON_PARAMETER
            );

            return lambda.Compile();
        }

        // Expression parameters
        private static readonly ParameterExpression INPUT_PARAMETER = Expression.Parameter(typeof(string), "value");
        private static readonly ParameterExpression STRING_COMPARISON_PARAMETER = Expression.Parameter(typeof(StringComparison), "comparisonType");

        // Lex rules
        private static readonly LexRule<TokenType>[] LEX_RULES = new[] {
            LexRuleFactory.New(@"^\(", TokenType.OpenBrace),
            LexRuleFactory.New(@"^\)", TokenType.CloseBrace),
            LexRuleFactory.New(@"^AND", TokenType.And),
            LexRuleFactory.New(@"^OR", TokenType.Or),
            LexRuleFactory.New(@"^NOT", TokenType.Not),
            LexRuleFactory.New(@"^'((?:[^'\\]|\\.)+)'", TokenType.Literal),
            LexRuleFactory.New(@"^[\r\t\n ]+", TokenType.Whitespace),
        };

        // Parse rules
        private static readonly ParseRule<TokenType>[] PARSE_RULES = new[] {
            ParseRuleFactory.NewSkip(
                new[] { TokenType.Whitespace }
            ),
            ParseRuleFactory.NewNormal(
                new [] { TokenType.Literal },
                TokenType.LiteralIndex,
                (tokens) => Expression.Call(INPUT_PARAMETER, typeof(string).GetMethod("IndexOf", new [] { typeof(string), typeof(StringComparison) }), Expression.Constant(tokens[0].Value), STRING_COMPARISON_PARAMETER)
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.LiteralIndex }, 
                TokenType.Result,
                (tokens) => Expression.GreaterThan(tokens[0].Expression, Expression.Constant(-1, typeof(int)))
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.Or }, 
                TokenType.BinaryOperator,
                (tokens) => Expression.Or(Expression.Constant(false), Expression.Constant(false))
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.And }, 
                TokenType.BinaryOperator,
                (tokens) => Expression.And(Expression.Constant(false), Expression.Constant(false))
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.Result, TokenType.BinaryOperator, TokenType.Result }, 
                TokenType.Result,
                (tokens) => ((BinaryExpression)tokens[1].Expression).Update(tokens[0].Expression, ((BinaryExpression)tokens[1].Expression).Conversion, tokens[2].Expression)
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.Not, TokenType.Result },
                TokenType.Result,
                (tokens) => Expression.Not(tokens[1].Expression)
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.OpenBrace, TokenType.Result, TokenType.CloseBrace }, 
                TokenType.Result,
                (tokens) => tokens[1].Expression
            ),
        };

    }
}
