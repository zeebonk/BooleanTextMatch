BooleanTextSearch
=================

A .NET library for text searching/matching using a SQL like syntax with boolean operators.


Examples
--------

### A simple query

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

### Invalid characters

	var query = "('foo' AND error 'bar') OR NOT 'baz'";

    // Throws UnexpectedCharacterException("Unexpected character 'e' at position '11'")
    var matcher = BooleanTextSearchFactory.New(query); 

### Invalid syntax

	var query = "('foo' AND 'bar') OR AND NOT 'baz'";

    // Throws UnexpectedTokenException("Unexpected token 'And' at position '21'")
    var matcher = BooleanTextSearchFactory.New(query);

### Incomplete expression

	var query = "('foo' AND 'bar') OR";

    // Throws IncompleteExpressionException()
    var matcher = BooleanTextSearchFactory.New(query);


Syntax
------

The used syntax is comparable with that of SQL. Currently the boolean `AND`, `OR` and `NOT` operators are implemented. Literals are enclosed in single quotes `'` and logical grouping is done with braces `(` `)`.


Implementation
--------------

The interpreter is implemented as a simple LR parser and gets fed by a basic lexer, which converts the raw input string into tokens.

The parser, based on the reduction rules, reduces the tokens into a LINQ expressions tree. When parsing is done, the GetCompiledExpression method can be used to compile the LINQ expression tree and return it as a `Func<string, bool>`.

The expression tree is a simple tree of boolean operators and Contains() calls. This means that for every literal in the expression a full text scan is done. When having a large amount of literals and/or a large text to search in, this could potentially (_untested!_) add up.


Possible future features
------------------------

* Flag for case-(in)sensitive comparing
* Support single quotes in literals using an escape sequence like `\'`
* A more optimized implementation like a single text scan instead of a Contains() calls per literal 
* Support for regex like wildcards for example: `.` for any character or `*` for any number of characters
* A order operator, for example: `'literal a' BEFORE 'literal b'`
