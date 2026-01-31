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
            new AttributeByWord("Test1:", new([" ", ":"]), 1, Backend.FileIO.Parser.ParseDirection.ParseRight),
            new AttributeByWord("Test2", new([" ", ":", "by", "as", "for"]), 1, Backend.FileIO.Parser.ParseDirection.ParseRight),
            new AttributeByWord("Test3:", new([" ", ":"]), 1, Backend.FileIO.Parser.ParseDirection.ParseLeft)]);
    }
    [Test]
    public void SimpleAttributeTest()
    {
        ParseSingleAttribute testParser = new("Bar", [new AttributeByWord("Test1:", new([" ", ":"]), 1, Backend.FileIO.Parser.ParseDirection.ParseRight)]);
        Assert.That(testParser.ParseAttributes("Test1: Thing", out _)[0], Is.EqualTo("Thing"));
    }
    [Test]
    public void MultiAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Test1: Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test2 Thing", out _)[0], Is.EqualTo("Thing"));
        }
    }
    [Test]
    public void OtherFormatTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Test1:    :    Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test1:Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test2     :    Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test2Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test2byas for Thing", out _)[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test2 by as for Thing", out _)[0], Is.EqualTo("Thing"));
        }
    }
    [Test]
    public void ExtractAttributeTest()
    {
        Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test1: Thing1 vitae pellentesque sem placerat in id", out _)[0], Is.EqualTo("Thing1"));
    }
    [Test]
    public void BypassKeywordTest()
    {
        Assert.That(Parser.ParseAttributes("Test2 by Thing2", out _)[0], Is.EqualTo("Thing2"));
    }
    [Test]
    public void ExtractRemainingStringTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test1: Thing1 vitae pellentesque sem placerat in id", out string result1);
            Assert.That(result1, Is.EqualTo("Lorem ipsum dolor sit amet consectetur adipiscing elit.   vitae pellentesque sem placerat in id"));
            Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test2 by Thing2 vitae pellentesque sem placerat in id", out string result2);
            Assert.That(result2, Is.EqualTo("Lorem ipsum dolor sit amet consectetur adipiscing elit.  by  vitae pellentesque sem placerat in id"));
        }
    }
    [Test]
    public void TotalAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test1: Thing1 vitae pellentesque sem placerat in id", out _)[0], Is.EqualTo("Thing1"));
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test2 by Thing2 vitae pellentesque sem placerat in id", out _)[0], Is.EqualTo("Thing2"));
        }
    }
}