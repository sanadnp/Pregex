using Pregex;
using System.Text.RegularExpressions;

namespace PregexTests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    #region Email Tests
    [Test]
    public void TestEmailMatch()
    {
        var emailRegex = RegexBuilder.Create()
            .StartOfString()
            .Word().OneOrMore()
            .Literal("@")
            .Word().OneOrMore()
            .Literal(".")
            .Set("a-zA-Z").Between(2, 6)
            .EndOfString()
            .Build();
        var match = emailRegex.IsMatch("test@example.com");
        Assert.IsTrue(match);
    }

    [Test]
    public void TestEmailNotMatch()
    {
        var emailRegex = RegexBuilder.Create()
            .StartOfString()
            .Word().OneOrMore()
            .Literal("@")
            .Word().OneOrMore()
            .Literal(".")
            .Set("a-zA-Z").Between(2, 6)
            .EndOfString()
            .Build();
        Assert.IsFalse(emailRegex.IsMatch("invalid@"));
    }

    [Test]
    public void TestEmailWithNumbers()
    {
        var emailRegex = RegexBuilder.Create()
            .StartOfString()
            .Word().OneOrMore()
            .Literal("@")
            .Word().OneOrMore()
            .Literal(".")
            .Set("a-zA-Z").Between(2, 6)
            .EndOfString()
            .Build();
        Assert.IsTrue(emailRegex.IsMatch("user123@test456.com"));
    }

    [Test]
    public void TestEmailNoAtSymbol()
    {
        var emailRegex = RegexBuilder.Create()
            .StartOfString()
            .Word().OneOrMore()
            .Literal("@")
            .Word().OneOrMore()
            .Literal(".")
            .Set("a-zA-Z").Between(2, 6)
            .EndOfString()
            .Build();
        Assert.IsFalse(emailRegex.IsMatch("testexample.com"));
    }

    [Test]
    public void TestEmailInvalidTLD()
    {
        var emailRegex = RegexBuilder.Create()
            .StartOfString()
            .Word().OneOrMore()
            .Literal("@")
            .Word().OneOrMore()
            .Literal(".")
            .Set("a-zA-Z").Between(2, 6)
            .EndOfString()
            .Build();
        Assert.IsFalse(emailRegex.IsMatch("test@example.c"));
        Assert.IsFalse(emailRegex.IsMatch("test@example.toolong"));
    }
    #endregion

    #region Phone Tests
    [Test]
    public void TestPhoneMatch()
    {
        var phoneRegex = RegexBuilder.Create()
            .StartOfString()
            .Literal("+").Optional()
            .Group(b => b.Digit().Exactly(3).Literal("-")).Optional()
            .Digit().Exactly(3)
            .Literal("-").Optional()
            .Digit().Exactly(4)
            .EndOfString()
            .Build();
        var match = phoneRegex.IsMatch("123-456-7890");
        Assert.IsTrue(match);
    }

    [Test]
    public void TestPhoneNotMatch()
    {
        var phoneRegex = RegexBuilder.Create()
            .StartOfString()
            .Literal("+").Optional()
            .Group(b => b.Digit().Exactly(3).Literal("-")).Optional()
            .Digit().Exactly(3)
            .Literal("-").Optional()
            .Digit().Exactly(4)
            .EndOfString()
            .Build();
        Assert.IsFalse(phoneRegex.IsMatch("+1-123-456"));
    }

    [Test]
    public void TestPhoneWithCountryCode()
    {
        var phoneRegex = RegexBuilder.Create()
            .StartOfString()
            .Literal("+").Optional()
            .Digit().Exactly(1).Literal("-")
            .Group(b => b.Digit().Exactly(3).Literal("-")).Optional()
            .Digit().Exactly(3)
            .Literal("-").Optional()
            .Digit().Exactly(4)
            .EndOfString()
            .Build();
        Assert.IsTrue(phoneRegex.IsMatch("+1-123-456-7890"));
    }

    [Test]
    public void TestPhoneWithoutDashes()
    {
        var phoneRegex = RegexBuilder.Create()
            .StartOfString()
            .Literal("+").Optional()
            .Digit().AtLeast(10)
            .EndOfString()
            .Build();
        Assert.IsTrue(phoneRegex.IsMatch("1234567890"));
    }

    [Test]
    public void TestPhoneTooShort()
    {
        var phoneRegex = RegexBuilder.Create()
            .StartOfString()
            .Literal("+").Optional()
            .Group(b => b.Digit().Exactly(3).Literal("-")).Optional()
            .Digit().Exactly(3)
            .Literal("-").Optional()
            .Digit().Exactly(4)
            .EndOfString()
            .Build();
        Assert.IsFalse(phoneRegex.IsMatch("123-456"));
    }
    #endregion

    #region Character Matching Tests
    [Test]
    public void TestDigitMatching()
    {
        var regex = RegexBuilder.Create().Digit().Build();
        Assert.IsTrue(regex.IsMatch("5"));
        Assert.IsFalse(regex.IsMatch("a"));
    }

    [Test]
    public void TestWordMatching()
    {
        var regex = RegexBuilder.Create().Word().Build();
        Assert.IsTrue(regex.IsMatch("a"));
        Assert.IsTrue(regex.IsMatch("Z"));
        Assert.IsTrue(regex.IsMatch("5"));
        Assert.IsTrue(regex.IsMatch("_"));
        Assert.IsFalse(regex.IsMatch("!"));
    }

    [Test]
    public void TestWhitespaceMatching()
    {
        var regex = RegexBuilder.Create().Whitespace().Build();
        Assert.IsTrue(regex.IsMatch(" "));
        Assert.IsTrue(regex.IsMatch("\t"));
        Assert.IsTrue(regex.IsMatch("\n"));
        Assert.IsFalse(regex.IsMatch("a"));
    }

    [Test]
    public void TestNotDigitMatching()
    {
        var regex = RegexBuilder.Create().NotDigit().Build();
        Assert.IsTrue(regex.IsMatch("a"));
        Assert.IsFalse(regex.IsMatch("5"));
    }

    [Test]
    public void TestAnyCharMatching()
    {
        var regex = RegexBuilder.Create().AnyChar().Build();
        Assert.IsTrue(regex.IsMatch("a"));
        Assert.IsTrue(regex.IsMatch("5"));
        Assert.IsTrue(regex.IsMatch("!"));
    }

    [Test]
    public void TestLiteralEscaping()
    {
        var regex = RegexBuilder.Create().Literal("test.com").Build();
        Assert.IsTrue(regex.IsMatch("test.com"));
        Assert.IsFalse(regex.IsMatch("testacom"));
    }
    
    [Test]
    public void TestNotWordMatching()
    {
        var regex = RegexBuilder.Create().NotWord().Build();
        Assert.IsTrue(regex.IsMatch("!"));
        Assert.IsTrue(regex.IsMatch("@"));
        Assert.IsTrue(regex.IsMatch(" "));
        Assert.IsFalse(regex.IsMatch("a"));
        Assert.IsFalse(regex.IsMatch("Z"));
        Assert.IsFalse(regex.IsMatch("5"));
        Assert.IsFalse(regex.IsMatch("_"));
    }

    [Test]
    public void TestNotWhitespaceMatching()
    {
        var regex = RegexBuilder.Create().NotWhitespace().Build();
        Assert.IsTrue(regex.IsMatch("a"));
        Assert.IsTrue(regex.IsMatch("5"));
        Assert.IsTrue(regex.IsMatch("!"));
        Assert.IsFalse(regex.IsMatch(" "));
        Assert.IsFalse(regex.IsMatch("\t"));
        Assert.IsFalse(regex.IsMatch("\n"));
    }
    
    #endregion

    #region Character Set Tests
    [Test]
    public void TestCharacterSet()
    {
        var regex = RegexBuilder.Create().Set("abc").Build();
        Assert.IsTrue(regex.IsMatch("a"));
        Assert.IsTrue(regex.IsMatch("b"));
        Assert.IsTrue(regex.IsMatch("c"));
        Assert.IsFalse(regex.IsMatch("d"));
    }

    [Test]
    public void TestNotCharacterSet()
    {
        var regex = RegexBuilder.Create().NotSet("abc").Build();
        Assert.IsFalse(regex.IsMatch("a"));
        Assert.IsFalse(regex.IsMatch("b"));
        Assert.IsFalse(regex.IsMatch("c"));
        Assert.IsTrue(regex.IsMatch("d"));
    }

    [Test]
    public void TestRange()
    {
        var regex = RegexBuilder.Create().Range('a', 'z').Build();
        Assert.IsTrue(regex.IsMatch("a"));
        Assert.IsTrue(regex.IsMatch("m"));
        Assert.IsTrue(regex.IsMatch("z"));
        Assert.IsFalse(regex.IsMatch("A"));
        Assert.IsFalse(regex.IsMatch("5"));
    }
    #endregion

    #region Quantifier Tests
    [Test]
    public void TestOneOrMore()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Digit().OneOrMore()
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("1"));
        Assert.IsTrue(regex.IsMatch("123"));
        Assert.IsFalse(regex.IsMatch(""));
        Assert.IsFalse(regex.IsMatch("abc"));
    }

    [Test]
    public void TestZeroOrMore()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Digit().ZeroOrMore()
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch(""));
        Assert.IsTrue(regex.IsMatch("1"));
        Assert.IsTrue(regex.IsMatch("123"));
        Assert.IsFalse(regex.IsMatch("abc"));
    }

    [Test]
    public void TestOptional()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Literal("http")
            .Literal("s").Optional()
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("http"));
        Assert.IsTrue(regex.IsMatch("https"));
        Assert.IsFalse(regex.IsMatch("httpss"));
    }

    [Test]
    public void TestExactly()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Digit().Exactly(3)
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("123"));
        Assert.IsFalse(regex.IsMatch("12"));
        Assert.IsFalse(regex.IsMatch("1234"));
    }

    [Test]
    public void TestAtLeast()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Digit().AtLeast(2)
            .EndOfString()
            .Build();
        Assert.IsFalse(regex.IsMatch("1"));
        Assert.IsTrue(regex.IsMatch("12"));
        Assert.IsTrue(regex.IsMatch("123"));
        Assert.IsTrue(regex.IsMatch("1234"));
    }

    [Test]
    public void TestBetween()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Digit().Between(2, 4)
            .EndOfString()
            .Build();
        Assert.IsFalse(regex.IsMatch("1"));
        Assert.IsTrue(regex.IsMatch("12"));
        Assert.IsTrue(regex.IsMatch("123"));
        Assert.IsTrue(regex.IsMatch("1234"));
        Assert.IsFalse(regex.IsMatch("12345"));
    }
    #endregion

    #region Anchor Tests
    [Test]
    public void TestStartOfString()
    {
        var regex = RegexBuilder.Create().StartOfString().Literal("test").Build();
        Assert.IsTrue(regex.IsMatch("test"));
        Assert.IsTrue(regex.IsMatch("test123"));
        Assert.IsFalse(regex.IsMatch("123test"));
    }

    [Test]
    public void TestEndOfString()
    {
        var regex = RegexBuilder.Create().Literal("test").EndOfString().Build();
        Assert.IsTrue(regex.IsMatch("test"));
        Assert.IsTrue(regex.IsMatch("123test"));
        Assert.IsFalse(regex.IsMatch("test123"));
    }

    [Test]
    public void TestWordBoundary()
    {
        var regex = RegexBuilder.Create()
            .WordBoundary()
            .Literal("test")
            .WordBoundary()
            .Build();
        Assert.IsTrue(regex.IsMatch("test"));
        Assert.IsTrue(regex.IsMatch("a test b"));
        Assert.IsFalse(regex.IsMatch("testing"));
        Assert.IsFalse(regex.IsMatch("atestb"));
    }
    #endregion

    #region Group Tests
    [Test]
    public void TestCapturingGroup()
    {
        var regex = RegexBuilder.Create()
            .Group(b => b.Digit().Exactly(3))
            .Literal("-")
            .Group(b => b.Digit().Exactly(4))
            .Build();
        var match = regex.Match("123-4567");
        Assert.IsTrue(match.Success);
        Assert.AreEqual("123", match.Groups[1].Value);
        Assert.AreEqual("4567", match.Groups[2].Value);
    }

    [Test]
    public void TestNonCapturingGroup()
    {
        var regex = RegexBuilder.Create()
            .NonCapturingGroup(b => b.Literal("http").Literal("s").Optional())
            .Literal("://")
            .Build();
        var match = regex.Match("https://");
        Assert.IsTrue(match.Success);
        Assert.AreEqual(1, match.Groups.Count); // Only the full match, no captured groups
    }

    [Test]
    public void TestNamedGroup()
    {
        var regex = RegexBuilder.Create()
            .NamedGroup("area", b => b.Digit().Exactly(3))
            .Literal("-")
            .NamedGroup("number", b => b.Digit().Exactly(4))
            .Build();
        var match = regex.Match("123-4567");
        Assert.IsTrue(match.Success);
        Assert.AreEqual("123", match.Groups["area"].Value);
        Assert.AreEqual("4567", match.Groups["number"].Value);
    }
    #endregion

    #region Alternation Tests
    [Test]
    public void TestAlternation()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Literal("cat")
            .Or()
            .Literal("dog")
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("cat"));
        Assert.IsTrue(regex.IsMatch("dog"));
        Assert.IsFalse(regex.IsMatch("bird"));
    }

    [Test]
    public void TestAlternationWithGroups()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Group(b => b.Literal("http").Or().Literal("https"))
            .Literal("://")
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("http://"));
        Assert.IsTrue(regex.IsMatch("https://"));
        Assert.IsFalse(regex.IsMatch("ftp://"));
    }
    #endregion

    #region Lookahead/Lookbehind Tests
    [Test]
    public void TestPositiveLookahead()
    {
        var regex = RegexBuilder.Create()
            .Digit().OneOrMore()
            .PositiveLookahead(b => b.Literal("%"))
            .Build();
        Assert.IsTrue(regex.IsMatch("50%"));
        Assert.IsFalse(regex.IsMatch("50"));
        var match = regex.Match("50%");
        Assert.AreEqual("50", match.Value); // Lookahead doesn't consume
    }

    [Test]
    public void TestNegativeLookahead()
    {
        // Test with literal strings to avoid digit matching complexity
        var regex = RegexBuilder.Create()
            .Literal("test")
            .NegativeLookahead(b => b.Literal("123"))
            .Build();
        Assert.IsTrue(regex.IsMatch("test"));
        Assert.IsTrue(regex.IsMatch("test456"));
        Assert.IsFalse(regex.IsMatch("test123"));
    }

    [Test]
    public void TestPositiveLookbehind()
    {
        var regex = RegexBuilder.Create()
            .PositiveLookbehind(b => b.Literal("$"))
            .Digit().OneOrMore()
            .WordBoundary()
            .Build();
        var matchWithDollar = regex.Match("Price: $50 total");
        Assert.IsTrue(matchWithDollar.Success);
        Assert.AreEqual("50", matchWithDollar.Value); // Lookbehind doesn't consume
        
        var matchWithoutDollar = regex.Match("Quantity: 50 total");
        Assert.IsFalse(matchWithoutDollar.Success);
    }

    [Test]
    public void TestNegativeLookbehind()
    {
        // Match "test" only when NOT preceded by "abc"
        var regex = RegexBuilder.Create()
            .NegativeLookbehind(b => b.Literal("abc"))
            .Literal("test")
            .Build();
        Assert.IsTrue(regex.IsMatch("test"));
        Assert.IsTrue(regex.IsMatch("xyztest"));
        Assert.IsFalse(regex.IsMatch("abctest"));
    }
    
    #endregion

    #region Regex Options Tests
    [Test]
    public void TestIgnoreCase()
    {
        var regex = RegexBuilder.Create()
            .Literal("test")
            .IgnoreCase()
            .Build();
        Assert.IsTrue(regex.IsMatch("test"));
        Assert.IsTrue(regex.IsMatch("TEST"));
        Assert.IsTrue(regex.IsMatch("TeSt"));
    }

    [Test]
    public void TestMultiline()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Literal("test")
            .Multiline()
            .Build();
        Assert.IsTrue(regex.IsMatch("test\nmore"));
        Assert.IsTrue(regex.IsMatch("first\ntest"));
    }

    [Test]
    public void TestSingleline()
    {
        var regex = RegexBuilder.Create()
            .AnyChar().ZeroOrMore()
            .Singleline()
            .Build();
        Assert.IsTrue(regex.IsMatch("test\nmore"));
    }
    #endregion

    #region Pattern Building Tests
    [Test]
    public void TestGetPattern()
    {
        var builder = RegexBuilder.Create()
            .StartOfString()
            .Digit().OneOrMore()
            .EndOfString();
        Assert.AreEqual(@"^\d+$", builder.GetPattern());
    }

    [Test]
    public void TestComplexPattern()
    {
        var builder = RegexBuilder.Create()
            .Literal("http")
            .Literal("s").Optional()
            .Literal("://")
            .Word().OneOrMore();
        Assert.AreEqual(@"https?://\w+", builder.GetPattern());
    }
    #endregion
    
    #region Edge Case Tests
    [Test]
    public void TestEmptyPattern()
    {
        var regex = RegexBuilder.Create().Build();
        Assert.IsTrue(regex.IsMatch(""));
        Assert.IsTrue(regex.IsMatch("anything"));
    }

    [Test]
    public void TestSpecialCharacterEscaping()
    {
        var regex = RegexBuilder.Create()
            .Literal("$100.00")
            .Build();
        Assert.IsTrue(regex.IsMatch("$100.00"));
        Assert.IsFalse(regex.IsMatch("$10000"));
    }

    [Test]
    public void TestNestedGroups()
    {
        var regex = RegexBuilder.Create()
            .Group(b => b
                .Group(inner => inner.Digit().Exactly(3))
                .Literal("-")
                .Digit().Exactly(4))
            .Build();
        var match = regex.Match("123-4567");
        Assert.IsTrue(match.Success);
        Assert.AreEqual("123-4567", match.Groups[1].Value);
        Assert.AreEqual("123", match.Groups[2].Value);
    }

    [Test]
    public void TestMultipleQuantifiers()
    {
        var regex = RegexBuilder.Create()
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
        Assert.IsTrue(regex.IsMatch("192.168.1.1"));
        Assert.IsTrue(regex.IsMatch("1.2.3.4"));
        Assert.IsTrue(regex.IsMatch("999.999.999.999"));
    }
    #endregion

    #region Real World Examples
    [Test]
    public void TestIPAddress()
    {
        var regex = RegexBuilder.Create()
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
        Assert.IsTrue(regex.IsMatch("192.168.0.1"));
        Assert.IsTrue(regex.IsMatch("10.0.0.1"));
        Assert.IsFalse(regex.IsMatch("192.168.0"));
    }

    [Test]
    public void TestHexColor()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Literal("#")
            .Set("0-9a-fA-F").Exactly(6)
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("#FF5733"));
        Assert.IsTrue(regex.IsMatch("#000000"));
        Assert.IsFalse(regex.IsMatch("#FFF"));
        Assert.IsFalse(regex.IsMatch("FF5733"));
    }

    [Test]
    public void TestUsername()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Set("a-zA-Z0-9_").Between(3, 16)
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("user123"));
        Assert.IsTrue(regex.IsMatch("john_doe"));
        Assert.IsFalse(regex.IsMatch("ab")); // Too short
        Assert.IsFalse(regex.IsMatch("user-name")); // Invalid character
    }

    [Test]
    public void TestDate_MMDDYYYY()
    {
        var regex = RegexBuilder.Create()
            .StartOfString()
            .Digit().Exactly(2)
            .Literal("/")
            .Digit().Exactly(2)
            .Literal("/")
            .Digit().Exactly(4)
            .EndOfString()
            .Build();
        Assert.IsTrue(regex.IsMatch("12/25/2023"));
        Assert.IsTrue(regex.IsMatch("01/01/2000"));
        Assert.IsFalse(regex.IsMatch("1/1/2023"));
        Assert.IsFalse(regex.IsMatch("12-25-2023"));
    }
    #endregion
}