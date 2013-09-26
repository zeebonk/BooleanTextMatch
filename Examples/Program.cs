using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BooleanTextSearch;

namespace Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            ValidExample();
            // InvalidCharactersExample();
            // InvalidTokensExample();
            // IncompleteExpressionExample();
            
            Console.ReadKey();
        }

        static void ValidExample()
        {
            var query = "('foo' AND 'bar') OR ('baz' AND 'qux')";

            var matcher = BooleanTextSearchFactory.New(query);

            // False
            Console.WriteLine(matcher("test footost tast tust tist"));
            // False
            Console.WriteLine(matcher("test tost tast qux tust tist"));
            // True
            Console.WriteLine(matcher("test bartost tast foo tist"));
            // True
            Console.WriteLine(matcher("test tostbaz tast foo tustqux tist")); // True
        }

        static void InvalidCharactersExample()
        {
            var query = "('foo' AND error 'bar') OR ('baz' AND 'qux')";

            // Throws UnexpectedCharacterException("Unexpected character 'e' at position '11'")
            var matcher = BooleanTextSearchFactory.New(query); 
        }

        static void InvalidTokensExample()
        {
            var query = "('foo' AND 'bar') OR AND ('baz' AND 'qux')";

            // Throws UnexpectedTokenException("Unexpected token 'And' at position '21'")
            var matcher = BooleanTextSearchFactory.New(query);
        }

        static void IncompleteExpressionExample()
        {
            var query = "('foo' AND 'bar') OR";

            // Throws IncompleteExpressionException()
            var matcher = BooleanTextSearchFactory.New(query);
        }
    }
}
