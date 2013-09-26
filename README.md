BooleanTextSearch
=================

A .NET library for text searching/matching using a SQL like syntax with boolean operators.


Examples
--------

A simple query:

	var query = "('foo' AND 'bar') OR ('baz' AND 'qux')";

	var matcher = BooleanTextSearchFactory.New(query);

	// False
	Console.WriteLine(matcher("test tost tast tust tist")); 
	// False
	Console.WriteLine(matcher("test tost tast foo tust tist"));
	// True
	Console.WriteLine(matcher("test bartost tast foo tist")); 
	// True
	Console.WriteLine(matcher("test tostbaz tast foo tustqux tist")); 

Invalid chracters:

	var query = "('foo' fail AND 'bar') OR ('baz' AND 'qux')";

	// Throws UnexpectedCharacterException("Unexpected character 'f' at position '7'")
	var matcher = BooleanTextSearchFactory.New(query); 

Invalid syntax:

	var query = "('foo' AND 'bar') OR AND ('baz' AND 'qux')";

	// Throws UnexpectedTokenException("Unexpected token 'And' at position '21')
	var matcher = BooleanTextSearchFactory.New(query); 

Incomplete expression:

	var query = "('foo' AND 'bar') OR";

	// Throws IncompleteExpressionException()
	var matcher = BooleanTextSearchFactory.New(query); 	


Syntax
------

The used syntax is comparable with that of SQL. Currently only the AND and OR operators are implemented, literals are enclosed in single quotes and logical grouping is done with braces.


Implementation
--------------

The interpreter is implemented as a simple LR parser and gets fed by a simple lexer, which converts the raw input string into tokens.

The parser, based on the reduction rules, converts the tokens into a LINQ expressions tree. When parsing is done, the GetExpression method can be used to compile the LINQ expression tree and return it as a Func<string, bool>.