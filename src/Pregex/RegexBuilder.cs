using System.Text;
using System.Text.RegularExpressions;
using Cysharp.Text;

namespace Pregex;

public class RegexBuilder
{
    private Utf16ValueStringBuilder _pattern;
    private RegexOptions _options;

    private RegexBuilder()
    {
        _pattern = ZString.CreateStringBuilder();
        _options = RegexOptions.None;
    }
    
    public RegexBuilder Literal(string str)
    {
        _pattern.Append(Regex.Escape(str));
        return this;
    }

    public RegexBuilder Digit()
    {
        _pattern.Append(@"\d");
        return this;
    }

    public RegexBuilder Word()
    {
        _pattern.Append(@"\w");
        return this;
    }

    public RegexBuilder Whitespace()
    {
        _pattern.Append(@"\s");
        return this;
    }

    public RegexBuilder NotDigit()
    {
        _pattern.Append(@"\D");
        return this;
    }

    public RegexBuilder NotWord()
    {
        _pattern.Append(@"\W");
        return this;
    }

    public RegexBuilder NotWhitespace()
    {
        _pattern.Append(@"\S");
        return this;
    }

    public RegexBuilder AnyChar()
    {
        _pattern.Append(".");
        return this;
    }

    // Character sets
    public RegexBuilder Set(string chars)
    {
        _pattern.Append($"[{Regex.Escape(chars).Replace(@"\-", "-")}]");
        return this;
    }

    public RegexBuilder NotSet(string chars)
    {
        _pattern.Append($"[^{Regex.Escape(chars).Replace(@"\-", "-")}]");
        return this;
    }

    public RegexBuilder Range(char start, char end)
    {
        _pattern.Append($"[{start}-{end}]");
        return this;
    }

    // Quantifiers
    public RegexBuilder OneOrMore()
    {
        _pattern.Append("+");
        return this;
    }

    public RegexBuilder ZeroOrMore()
    {
        _pattern.Append("*");
        return this;
    }

    public RegexBuilder Optional()
    {
        _pattern.Append("?");
        return this;
    }

    public RegexBuilder Exactly(int n)
    {
        _pattern.Append($"{{{n}}}");
        return this;
    }

    public RegexBuilder AtLeast(int n)
    {
        _pattern.Append($"{{{n},}}");
        return this;
    }

    public RegexBuilder Between(int min, int max)
    {
        _pattern.Append($"{{{min},{max}}}");
        return this;
    }

    // Anchors
    public RegexBuilder StartOfString()
    {
        _pattern.Append("^");
        return this;
    }

    public RegexBuilder EndOfString()
    {
        _pattern.Append("$");
        return this;
    }

    public RegexBuilder WordBoundary()
    {
        _pattern.Append(@"\b");
        return this;
    }

    public RegexBuilder NotWordBoundary()
    {
        _pattern.Append(@"\B");
        return this;
    }

    // Groups
    public RegexBuilder Group(Action<RegexBuilder> callback)
    {
        _pattern.Append("(");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    public RegexBuilder NonCapturingGroup(Action<RegexBuilder> callback)
    {
        _pattern.Append("(?:");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    public RegexBuilder NamedGroup(string name, Action<RegexBuilder> callback)
    {
        _pattern.Append($"(?<{name}>");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    // Alternation
    public RegexBuilder Or()
    {
        _pattern.Append("|");
        return this;
    }

    // Lookahead/Lookbehind
    public RegexBuilder PositiveLookahead(Action<RegexBuilder> callback)
    {
        _pattern.Append("(?=");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    public RegexBuilder NegativeLookahead(Action<RegexBuilder> callback)
    {
        _pattern.Append("(?!");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    public RegexBuilder PositiveLookbehind(Action<RegexBuilder> callback)
    {
        _pattern.Append("(?<=");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    public RegexBuilder NegativeLookbehind(Action<RegexBuilder> callback)
    {
        _pattern.Append("(?<!");
        callback(this);
        _pattern.Append(")");
        return this;
    }

    // Regex Options (Flags)
    public RegexBuilder IgnoreCase()
    {
        _options |= RegexOptions.IgnoreCase;
        return this;
    }

    public RegexBuilder Multiline()
    {
        _options |= RegexOptions.Multiline;
        return this;
    }

    public RegexBuilder Singleline()
    {
        _options |= RegexOptions.Singleline;
        return this;
    }

    public RegexBuilder ExplicitCapture()
    {
        _options |= RegexOptions.ExplicitCapture;
        return this;
    }

    public RegexBuilder Compiled()
    {
        _options |= RegexOptions.Compiled;
        return this;
    }

    public RegexBuilder IgnorePatternWhitespace()
    {
        _options |= RegexOptions.IgnorePatternWhitespace;
        return this;
    }

    // Build methods
    public Regex Build()
    {
        return new Regex(_pattern.ToString(), _options);
    }

    public string GetPattern()
    {
        return _pattern.ToString();
    }

    public RegexOptions GetOptions()
    {
        return _options;
    }

    public override string ToString()
    {
        return $"/{_pattern}/{_options}";
    }

    // Static factory
    public static RegexBuilder Create()
    {
        return new RegexBuilder();
    }
}