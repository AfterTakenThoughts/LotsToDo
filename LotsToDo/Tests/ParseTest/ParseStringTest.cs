using System.Collections.Generic;
using LotsToDo.Backend.FileIO.Parser.ParserMethods;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;

namespace LotsToDo.Tests.ParseTest;

public class ParseStringTest
{
    public required ParseSingleAttribute Parser { get; set; }
    [SetUp]
    public void Setup()
    {
        Parser = new("Foo", [
            new("Test1:", [], [], Backend.FileIO.Parser.ParseDirection.ParseRight),
            new("Test2", ["by", "as", "for"], [], Backend.FileIO.Parser.ParseDirection.ParseRight)]);
    }
    [Test]
    public void SimpleAttributeTest()
    {
        ParseSingleAttribute testParser = new("Bar", [new("Test1:", [], [], Backend.FileIO.Parser.ParseDirection.ParseRight)]);
        Assert.That(testParser.ParseAttributes("Test1: Thing")[0], Is.EqualTo("Thing"));
    }
    [Test]
    public void MultiAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Test1: Thing")[0], Is.EqualTo("Thing"));
            Assert.That(Parser.ParseAttributes("Test2 Thing")[0], Is.EqualTo("Thing"));
        }
    }
    [Test]
    public void ExtractAttributeTest()
    {
        Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test1: Thing1 vitae pellentesque sem placerat in id")[0], Is.EqualTo("Thing1"));
    }
    [Test]
    public void BypassKeywordTest()
    {
        Assert.That(Parser.ParseAttributes("Test2 by Thing2")[0], Is.EqualTo("Thing2"));
    }
    [Test]
    public void TotalAttributeTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test1: Thing1 vitae pellentesque sem placerat in id")[0], Is.EqualTo("Thing1"));
            Assert.That(Parser.ParseAttributes("Lorem ipsum dolor sit amet consectetur adipiscing elit. Test2 by Thing2 vitae pellentesque sem placerat in id")[0], Is.EqualTo("Thing2"));
        }
    }
}