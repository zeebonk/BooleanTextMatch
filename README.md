BooleanTextMatch
=================

A .NET library for text matching using a SQL like syntax with boolean operators.


Examples
--------

### A simple query

```csharp

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

```

### Invalid characters

```csharp

var query = "('foo' AND error 'bar') OR NOT 'baz'";

// Throws UnexpectedCharacterException("Unexpected character 'e' at position '11'")
var matcher = TextMatcher.New(query, StringComparison.Ordinal);

```

### Invalid syntax

```csharp

var query = "('foo' AND 'bar') OR AND NOT 'baz'";

// Throws UnexpectedTokenException("Unexpected token 'And' at position '21'")
var matcher = TextMatcher.New(query, StringComparison.Ordinal);

```

### Incomplete expression

```csharp

var query = "('foo' AND 'bar') OR";

// Throws IncompleteExpressionException()
var matcher = TextMatcher.New(query, StringComparison.Ordinal);

```


Syntax
------

The used syntax is comparable with that of SQL. Currently the boolean `AND`, `OR` and `NOT` operators are implemented and logical grouping is done with braces `(` `)`. Literals are enclosed in single quotes `'` and single quotes can be used in literals when escaped with a backslash `\'`. 


Implementation
--------------

The interpreter is implemented as a simple LR parser and gets fed by a basic lexer, which converts the raw input string into tokens.

The parser, based on the reduction rules, reduces the tokens into a LINQ expressions tree. When parsing is done, the GetCompiledExpression method can be used to compile the LINQ expression tree and return it as a `Func<string, StringComparison, bool>`.

The expression tree is a simple tree of boolean operators and `IndexOf()` calls. Every `IndexOf()` call can potentially cause a full text scan but by ordering the query to make the most of short-circuit evaluation it is posible to minimize the number of `IndexOf()` calls.


Possible future features
------------------------

* A more optimized implementation like a single text scan instead of a `IndexOf()` call per literal 
* Support for regex like wildcards, for example: `.` for any character or `*` for any number of characters
* An order operator, for example: `'literal a' BEFORE 'literal b'`
