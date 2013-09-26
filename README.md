BooleanTextSearch
=================

A .NET library for text matching using a SQL like syntax.


Examples
--------

A simple query:

	var query = "('foo' AND 'bar') OR ('baz' AND 'qux')";

	var matcher = BooleanTextSearchFactory.New(query);

	Console.WriteLine(matcher("test riktost tast tust tist")); // False
	Console.WriteLine(matcher("test teost tast foo tust tist")); // False
	Console.WriteLine(matcher("test bartost tast foo tist")); // True
	Console.WriteLine(matcher("test tostbaz tast foo tustqux tist")); // True

Invalid chracters:

	var query = "('foo' crash AND 'bar') OR ('baz' AND 'qux')";

	var matcher = BooleanTextSearchFactory.New(query); // throws UnexpectedCharacterException("Unexpected character at 7");

Invalid syntax:

	var query = "('foo' AND 'bar') OR AND ('baz' AND 'qux')";

	var matcher = BooleanTextSearchFactory.New(query); // throws UnexpectedTokenException("Unexpected token 'And' at position '21');


Syntax
------

The used syntax is comparable with that of SQL. Currently only the AND and OR operators are implemented, literals are enclosed in single quotes and logical grouping is done with braces.


Implementation
--------------

The interpreter is implemented as a simple LR parser and gets fed by a simple lexer, which converts the raw input string into tokens.

The parser, based on the reduction rules, converts the tokens into a LINQ expressions tree. When parsing is done, the GetExpression method can be used to compile the LINQ expression tree and return it as a Func<string, bool>.