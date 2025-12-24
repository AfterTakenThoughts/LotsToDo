using System;
using System.Collections.Generic;
using LotsToDo.Backend;
using NUnit.Framework.Constraints;

namespace LotsToDo.Tests;

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
        string testItem = $"""
            Test1
                Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Item.CreateTime:MM/dd/yyyy HH:mm}
                Tags: foo: (bar, baz), foo2: (bar2, baz)
            """;
        Assert.That(Item.ToString(), Is.EqualTo(testItem.ReplaceLineEndings()));
    }
    [Test]
    public void NoTagTest()
    {
        Item = new ToDoItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1));
        string testItem = $"""
            Test1
                Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Item.CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(Item.ToString(), Is.EqualTo(testItem.ReplaceLineEndings()));
    }
    [Test]
    public void NoTagNoTimeTest()
    {
        Item = new ToDoItem("Test1");
        string testItem = $"""
            Test1
                Created: {Item.CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(Item.ToString(), Is.EqualTo(testItem.ReplaceLineEndings()));
    }
}
