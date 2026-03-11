using System;
using LotsToDo.Backend.FileIO.Parser;
using LotsToDo.Backend.FileIO.Parser.AttributeInfo;
using LotsToDo.Backend.FileIO.Parser.ParserMethods;

namespace LotsToDo.Tests.ParseTest;

public class ParseStringTest
{
    public required ParseSingleAttribute Parser { get; set; }
    [SetUp]
    public void Setup()
    {
        Parser = new("Foo", [
            new AttributeByWord("TestBase:", new([" ", ":"]), 1, ParseDirection.ParseRight),
            new AttributeByWord(new MatchInfo("TestCase:", StringComparison.InvariantCultureIgnoreCase), new([" ", ":"]), 1, ParseDirection.ParseRight),
            new AttributeByWord("TestBypass", new([" ", ":", "by", "as", "for"]), 1, ParseDirection.ParseRight),
            new AttributeByWord("TestParseLeft:", new([" ", ":"]), 1, ParseDirection.ParseLeft)]);
    }
    [Test]
    public void SimpleAttributeTest()
    {
        ParseSingleAttribute testParser = new("Bar", [new AttributeByWord("TestBase:", new([" ", ":"]), 1, ParseDirection.ParseRight)]);
        Assert.That(testParser.ParseAttributes("TestBase: Thing", out _)[0], Is.EqualTo("Thing"));
    }
    [Test]
    public void MultiAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("TestBase: Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TestBypass Thing", out _)[0], Is.EqualTo("Thing"));
        }
    }
    [Test]
    public void OtherFormatTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("TestBase:    :    Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TestBase:Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TestBypass     :    Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TestBypassThing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TestBypassbyas for Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TestBypass by as for Thing", out _)[0], Is.EqualTo("Thing"));
        }
    }
    [Test]
    public void CaseTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("TestCase: Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Testcase: Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("testcase: Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("TeStCaSe: Thing", out _)[0], Is.EqualTo("Thing"));
        }
    }
    [Test]
    public void ExtractAttributeTest()
    {
        Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. TestBase: Thing1 vitae pellentesque sem placerat in id", out _)[0], Is.EqualTo("Thing1"));
    }
    [Test]
    public void BypassKeywordTest()
    {
        Assert.That(Parser.ParseAttributes("TestBypass by Thing2", out _)[0], Is.EqualTo("Thing2"));
    }
    [Test]
    public void ExtractRemainingStringTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. TestBase: Thing1 vitae pellentesque sem placerat in id", out string result1);
            Assert.That(result1, Is.EqualTo("Lorem ipsum dolor sit amet consectetur adipiscing elit. vitae pellentesque sem placerat in id"));
            Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. TestBypass by Thing2 vitae pellentesque sem placerat in id", out string result2);
            Assert.That(result2, Is.EqualTo("Lorem ipsum dolor sit amet consectetur adipiscing elit. vitae pellentesque sem placerat in id"));
        }
    }
    [Test]
    public void TotalAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. TestBase: Thing1 vitae pellentesque sem placerat in id", out _)[0], Is.EqualTo("Thing1"));
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. TestBypass by Thing2 vitae pellentesque sem placerat in id", out _)[0], Is.EqualTo("Thing2"));
        }
    }
    [Test]
    public void NoAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. vitae pellentesque sem placerat in id", out string result), Is.Empty);
            Assert.That(result, Is.EqualTo("Lorem ipsum dolor sit amet consectetur adipiscing elit. vitae pellentesque sem placerat in id"));
        }
    }
}