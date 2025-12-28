[![.NET](https://github.com/gregyjames/pregex/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/gregyjames/pregex/actions/workflows/dotnet.yml)
[![NuGet latest version](https://badgen.net/nuget/v/Pregex)](https://www.nuget.org/packages/Pregex)
![NuGet Downloads](https://img.shields.io/nuget/dt/Pregex)
[![codecov](https://codecov.io/github/gregyjames/pregex/branch/main/graph/badge.svg?token=95UjrQ1tDl)](https://codecov.io/github/gregyjames/pregex)

# Pregex - Pretty RegEx!

A fluent, chainable API for building regular expressions in C# that's easy to read, write, and maintain.

## Why Pregex?

Regular expressions are powerful but notoriously difficult to read and maintain. Pregex solves this by providing a fluent interface that makes regex patterns self-documenting and easier to understand.

### Before (Traditional Regex)
```csharp
var regex = new Regex(@"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$");
```

### After (Pregex)
```csharp
var regex = RegexBuilder.Create()
    .StartOfString()
    .Set("a-zA-Z0-9._%-").OneOrMore()
    .Literal("@")
    .Set("a-zA-Z0-9.-").OneOrMore()
    .Literal(".")
    .Set("a-zA-Z").Between(2, 6)
    .EndOfString()
    .Build();
```

## Installation

```bash
dotnet add package Pregex
```

## Quick Start

```csharp
using Pregex;

// Simple phone number validation
var phoneRegex = RegexBuilder.Create()
    .StartOfString()
    .Literal("+").Optional()
    .Digit().Exactly(3)
    .Literal("-").Optional()
    .Digit().Exactly(3)
    .Literal("-").Optional()
    .Digit().Exactly(4)
    .EndOfString()
    .Build();

bool isValid = phoneRegex.IsMatch("123-456-7890"); // true
```

## Features

### Character Matching
```csharp
.Digit()           // \d - matches any digit
.Word()            // \w - matches word characters
.Whitespace()      // \s - matches whitespace
.NotDigit()        // \D - matches non-digits
.NotWord()         // \W - matches non-word characters
.NotWhitespace()   // \S - matches non-whitespace
.AnyChar()         // . - matches any character
.Literal("text")   // Matches literal text (auto-escapes special chars)
```

### Character Sets
```csharp
.Set("abc")           // [abc] - matches a, b, or c
.NotSet("abc")        // [^abc] - matches anything except a, b, or c
.Range('a', 'z')      // [a-z] - matches any lowercase letter
```

### Quantifiers
Quantifiers modify the preceding element:
```csharp
.Word().OneOrMore()      // \w+ - one or more word characters
.Digit().ZeroOrMore()    // \d* - zero or more digits
.Literal("s").Optional() // s? - optional 's'
.Digit().Exactly(3)      // \d{3} - exactly 3 digits
.Word().AtLeast(2)       // \w{2,} - at least 2 word characters
.Digit().Between(2, 4)   // \d{2,4} - between 2 and 4 digits
```

### Anchors
```csharp
.StartOfString()    // ^ - start of string
.EndOfString()      // $ - end of string
.WordBoundary()     // \b - word boundary
.NotWordBoundary()  // \B - not a word boundary
```

### Groups
```csharp
// Capturing group
.Group(b => b.Digit().Exactly(3))

// Non-capturing group
.NonCapturingGroup(b => b.Literal("http").Literal("s").Optional())

// Named group
.NamedGroup("area", b => b.Digit().Exactly(3))
```

### Alternation
```csharp
.Literal("cat")
.Or()
.Literal("dog")
// Matches "cat" or "dog"
```

### Lookahead and Lookbehind
```csharp
// Positive lookahead - match only if followed by pattern
.Digit().OneOrMore()
.PositiveLookahead(b => b.Literal("%"))

// Negative lookahead - match only if NOT followed by pattern
.Literal("test")
.NegativeLookahead(b => b.Literal("123"))

// Positive lookbehind - match only if preceded by pattern
.PositiveLookbehind(b => b.Literal("$"))
.Digit().OneOrMore()

// Negative lookbehind - match only if NOT preceded by pattern
.NegativeLookbehind(b => b.Literal("abc"))
.Literal("test")
```

### Regex Options
```csharp
.IgnoreCase()              // Case-insensitive matching
.Multiline()               // ^ and $ match line boundaries
.Singleline()              // . matches newline characters
.Compiled()                // Compile for better performance
.ExplicitCapture()         // Only named groups are captured
.IgnorePatternWhitespace() // Ignore whitespace in pattern
```

## Examples

### Email Validation
```csharp
var emailRegex = RegexBuilder.Create()
    .StartOfString()
    .Set("a-zA-Z0-9._%-").OneOrMore()
    .Literal("@")
    .Set("a-zA-Z0-9.-").OneOrMore()
    .Literal(".")
    .Set("a-zA-Z").Between(2, 6)
    .EndOfString()
    .Build();

emailRegex.IsMatch("user@example.com"); // true
emailRegex.IsMatch("invalid@"); // false
```

### URL Parsing with Named Groups
```csharp
var urlRegex = RegexBuilder.Create()
    .NamedGroup("protocol", b => b.Literal("http").Literal("s").Optional())
    .Literal("://")
    .NamedGroup("domain", b => b.NotSet(" /").OneOrMore())
    .NamedGroup("path", b => b.Literal("/").AnyChar().ZeroOrMore()).Optional()
    .Build();

var match = urlRegex.Match("https://example.com/path");
Console.WriteLine(match.Groups["protocol"].Value); // "https"
Console.WriteLine(match.Groups["domain"].Value);   // "example.com"
Console.WriteLine(match.Groups["path"].Value);     // "/path"
```

### Phone Number with Country Code
```csharp
var phoneRegex = RegexBuilder.Create()
    .StartOfString()
    .Literal("+").Optional()
    .Group(b => b.Digit().Exactly(3).Literal("-")).Optional()
    .Digit().Exactly(3)
    .Literal("-").Optional()
    .Digit().Exactly(4)
    .EndOfString()
    .Build();

phoneRegex.IsMatch("123-456-7890");    // true
phoneRegex.IsMatch("+1-123-456-7890"); // true
phoneRegex.IsMatch("1234567890");      // true
```

### Username Validation
```csharp
var usernameRegex = RegexBuilder.Create()
    .StartOfString()
    .Set("a-zA-Z0-9_").Between(3, 16)
    .EndOfString()
    .Build();

usernameRegex.IsMatch("john_doe"); // true
usernameRegex.IsMatch("ab");       // false (too short)
usernameRegex.IsMatch("user-name"); // false (invalid char)
```

### IP Address Validation
```csharp
var ipRegex = RegexBuilder.Create()
    .StartOfString()
    .Digit().Between(1, 3)
    .Literal(".")
    .Digit().Between(1, 3)
    .Literal(".")
    .Digit().Between(1, 3)
    .Literal(".")
    .Digit().Between(1, 3)
    .EndOfString()
    .Build();

ipRegex.IsMatch("192.168.0.1"); // true
```

### Hex Color Code
```csharp
var hexColorRegex = RegexBuilder.Create()
    .StartOfString()
    .Literal("#")
    .Set("0-9a-fA-F").Exactly(6)
    .EndOfString()
    .Build();

hexColorRegex.IsMatch("#FF5733"); // true
hexColorRegex.IsMatch("#FFF");    // false
```

### Password Validation (Complex)
```csharp
var passwordRegex = RegexBuilder.Create()
    .StartOfString()
    .PositiveLookahead(b => b.AnyChar().ZeroOrMore().Digit())        // At least one digit
    .PositiveLookahead(b => b.AnyChar().ZeroOrMore().Range('a', 'z')) // At least one lowercase
    .PositiveLookahead(b => b.AnyChar().ZeroOrMore().Range('A', 'Z')) // At least one uppercase
    .AnyChar().AtLeast(8)  // At least 8 characters total
    .EndOfString()
    .Build();

passwordRegex.IsMatch("Password1"); // true
passwordRegex.IsMatch("password");  // false (no uppercase or digit)
```

## Utility Methods

```csharp
// Get the raw pattern string
string pattern = builder.GetPattern();

// Get the regex options
RegexOptions options = builder.GetOptions();

// Convert to string representation
string representation = builder.ToString(); // e.g., "/^\d+$/gi"

// Build the final Regex object
Regex regex = builder.Build();
```

## Best Practices

### 1. Chain quantifiers AFTER the element they modify
```csharp
// ✅ Correct
.Digit().OneOrMore()  // \d+

// ❌ Wrong
.OneOrMore().Digit()  // +\d (invalid)
```

### 2. Use groups for complex patterns
```csharp
.Group(b => b
    .Literal("http")
    .Or()
    .Literal("https")
)
```

### 3. Use named groups for readability
```csharp
.NamedGroup("year", b => b.Digit().Exactly(4))
.Literal("-")
.NamedGroup("month", b => b.Digit().Exactly(2))
```

### 4. Anchor your patterns when appropriate
```csharp
// Match entire string
.StartOfString()
  // ... your pattern
.EndOfString()
```

### 5. Use word boundaries for whole word matching
```csharp
.WordBoundary()
.Literal("test")
.WordBoundary()
```

## Performance Tips

- Use `.Compiled()` for frequently used patterns
- Anchor patterns with `^` and `$` when possible to avoid unnecessary backtracking
- Use non-capturing groups when you don't need to extract the matched value

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License
MIT License

Copyright (c) 2025 Greg James

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

