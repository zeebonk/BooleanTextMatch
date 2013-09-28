﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooleanTextSearch.Interpreter;
using System.Linq.Expressions;

namespace BooleanTextSearch
{
    public class BooleanTextSearchFactory
    {
        private static readonly ParameterExpression INPUT_PARAMETER = Expression.Parameter(typeof(string), "value");
        private static readonly ParameterExpression STRING_COMPARISON_PARAMETER = Expression.Parameter(typeof(StringComparison), "comparisonType");

        private static readonly LexRule<TokenType>[] LEX_RULES = new[] {
            LexRuleFactory.New(@"^\(", TokenType.OpenBrace),
            LexRuleFactory.New(@"^\)", TokenType.CloseBrace),
            LexRuleFactory.New(@"^AND", TokenType.And),
            LexRuleFactory.New(@"^OR", TokenType.Or),
            LexRuleFactory.New(@"^NOT", TokenType.Not),
            LexRuleFactory.New(@"^'((?:[^'\\]|\\.)+)'", TokenType.Literal),
            LexRuleFactory.New(@"^[\r\t\n ]+", TokenType.Whitespace),
        };

        private static readonly ParseRule<TokenType>[] PARSE_RULES = new[] {
            ParseRuleFactory.NewSkip(
                new[] { TokenType.Whitespace }
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.Literal }, 
                TokenType.Result,
                (tokens) => Expression.GreaterThan(
                    Expression.Call(INPUT_PARAMETER, typeof(string).GetMethod("IndexOf", new [] { typeof(string), typeof(StringComparison) }), Expression.Constant(tokens[0].Value), STRING_COMPARISON_PARAMETER), 
                    Expression.Constant(-1, typeof(int))
                )
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.Result, TokenType.And, TokenType.Result }, 
                TokenType.Result,
                (tokens) => Expression.And(
                    tokens[0].Expression, 
                    tokens[2].Expression
                )
            ),
            ParseRuleFactory.NewNormal(
                new[] { TokenType.Result, TokenType.Or, TokenType.Result }, 
                TokenType.Result,
                (tokens) => Expression.Or(tokens[0].Expression, tokens[2].Expression)
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

        public static Func<string, StringComparison, bool> New(string query)
        {
            var lexer = new Lexer<TokenType>(LEX_RULES);
            var parser = new Parser<TokenType>(PARSE_RULES);

            foreach (var token in lexer.GetTokens(query))
                parser.Parse(token);

            return GetCompiledExpression(parser);
        }

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
    }
}
