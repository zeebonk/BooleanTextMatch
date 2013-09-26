BooleanTextSearch
=================

A .NET library for text matching using a SQL like syntax.

Example
----------------

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