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
            var query = "('foo' AND 'bar') OR ('baz' AND 'qux')";

            var matcher = BooleanTextSearchFactory.New(query);

            Console.WriteLine(matcher("test riktost tast tust tist")); // False
            Console.WriteLine(matcher("test teost tast foo tust tist")); // False
            Console.WriteLine(matcher("test bartost tast foo tist")); // True
            Console.WriteLine(matcher("test tostbaz tast foo tustqux tist")); // True

            Console.ReadKey();
        }
    }
}
