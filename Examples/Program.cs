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
            //InvalidCharactersExample();
            //InvalidTokensExample();
            //IncompleteExpressionExample();
            
            Console.ReadKey();
        }

        static void ValidExample()
        {
            var query = "('foo' AND 'bar') OR NOT 'baz'";

            var matcher = BooleanTextSearchFactory.New(query);

            // True
            Console.WriteLine(matcher("test tost tast tust tist"));
            // False
            Console.WriteLine(matcher("test tost tast tust baz tist"));
            // False
            Console.WriteLine(matcher("test foo tost tast tust baz tist"));
            // True
            Console.WriteLine(matcher("test bar tost tast tust baz foo tist"));
        }

        static void InvalidCharactersExample()
        {
            var query = "('foo' AND error 'bar') OR NOT 'baz'";

            // Throws UnexpectedCharacterException("Unexpected character 'e' at position '11'")
            var matcher = BooleanTextSearchFactory.New(query); 
        }

        static void InvalidTokensExample()
        {
            var query = "('foo' AND 'bar') OR AND NOT 'baz'";

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
