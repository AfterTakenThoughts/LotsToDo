using System;
using LotsToDo.Backend;

namespace LotsToDo.Tests.ToDoItemTest;

class ToDoItemTest
{
    public required ToDoItem Item { get; set; }
    [SetUp]
    public void Setup()
    {
        Item = new ToDoItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1), new()
        {
            { "foo", ["bar", "baz"] },
            { "foo2", ["bar2", "baz"] }
        });
    }
    [Test]
    public void InitItemTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Item.IsCompleted, Is.False);
            Assert.That(Item.Content, Is.EqualTo("Test1"));
            Assert.That(Item.Tags, Does.ContainKey("foo"));
            Assert.That(Item.Tags["foo"], Is.EqualTo(["bar", "baz"]));
        }
    }
    [Test]
    public void ItemToStringTest()
    {
        string testString = $"""
            Test1
                Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Item.CreateTime:MM/dd/yyyy HH:mm}
                Tags: foo: (bar, baz), foo2: (bar2, baz)
            """;
        Assert.That(Item.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));
    }
    [Test]
    public void NoTagTest()
    {
        ToDoItem testItem = new("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1));
        string testString = $"""
            Test1
                Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {testItem.CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(testItem.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));
    }
    [Test]
    public void NoTagNoTimeTest()
    {
        ToDoItem testItem = new("Test1");
        string testString = $"""
            Test1
                Created: {testItem.CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(testItem.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));
    }
    [Test]
    public void OtherCombinationTimeTest()
    {
        ToDoItem testItem = new("Test1", new DateTime(2000, 1, 1, 1, 1, 1));
        string testString = $"""
            Test1
                Start: 01/01/2000 01:01, Created: {testItem.CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(testItem.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));

        testItem = new("Test2", null, new DateTime(2000, 1, 1, 1, 1, 1));
        testString = $"""
            Test2
                Due: 01/01/2000 01:01, Created: {testItem.CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(testItem.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));

        testItem = new("Test3", new DateTime(2000, 1, 1, 1, 1, 1), null, new()
            {
                { "foo", ["bar", "baz"] },
                { "foo2", ["bar2", "baz"] }
            });
        testString = $"""
            Test3
                Start: 01/01/2000 01:01, Created: {testItem.CreateTime:MM/dd/yyyy HH:mm}
                Tags: foo: (bar, baz), foo2: (bar2, baz)
            """;
        Assert.That(testItem.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));

        testItem = new("Test4", null, new DateTime(2000, 1, 1, 1, 1, 1), new()
            {
                { "foo", ["bar", "baz"] },
                { "foo2", ["bar2", "baz"] }
            });
        testString = $"""
            Test4
                Due: 01/01/2000 01:01, Created: {testItem.CreateTime:MM/dd/yyyy HH:mm}
                Tags: foo: (bar, baz), foo2: (bar2, baz)
            """;
        Assert.That(testItem.ToString(), Is.EqualTo(testString.ReplaceLineEndings()));
    }
}
