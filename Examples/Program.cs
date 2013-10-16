using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooleanTextSearch;
using System.Diagnostics;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            //ValidExample();
            //InvalidCharactersExample();
            //InvalidTokensExample();
            IncompleteExpressionExample();
            
            Console.ReadKey();
        }

        static void ValidExample()
        {
            var query = @"('foo' AND 'ba\'r') OR NOT 'baz'";

            var matcher = TextMatcher.New(query, StringComparison.Ordinal);
            
            // True
            Console.WriteLine(matcher.Matches("test tost tast tust tist"));
            // False
            Console.WriteLine(matcher.Matches("test tost tast tust baz tist"));
            // False
            Console.WriteLine(matcher.Matches("test foo tost tast tust baz tist"));
            // False
            Console.WriteLine(matcher.Matches("test bar tost tast tust baz foo tist"));
            // True
            Console.WriteLine(matcher.Matches("test ba'r tost tast tust baz foo tist"));
        }

        static void InvalidCharactersExample()
        {
            var query = "('foo' AND error 'bar') OR NOT 'baz'";

            // Throws UnexpectedCharacterException("Unexpected character 'e' at position '11'")
            var matcher = TextMatcher.New(query, StringComparison.Ordinal);
        }

        static void InvalidTokensExample()
        {
            var query = "('foo' AND 'bar') OR AND NOT 'baz'";

            // Throws UnexpectedTokenException("Unexpected token 'BinaryOperator' at position '21'")
            var matcher = TextMatcher.New(query, StringComparison.Ordinal);
        }

        static void IncompleteExpressionExample()
        {
            var query = "('foo' AND 'bar') OR";

            // Throws IncompleteExpressionException()
            var matcher = TextMatcher.New(query, StringComparison.Ordinal);
        }
    }
}
