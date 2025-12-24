using System;
using LotsToDo.Backend;

namespace LotsToDo.Tests;

class ToDoFolderTest
{
    public required ToDoFolder Folder { get; set; }
    [SetUp]
    public void Setup()
    {
        Folder =
            new ToDoFolder("Test",
            [
                new ToDoItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1)),
                new ToDoItem("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30))
            ],
            new ToDoFolder("TestInner",
            [
                new ToDoItem("Test1")
            ]
            ));
    }
    [Test]
    public void InitFolderTest()
    {
        using (Assert.EnterMultipleScope())
        {
            Assert.That(Folder.FolderName, Is.EqualTo("Test"));
            Assert.That(Folder.Item, Has.Count.EqualTo(2));
            Assert.That(Folder.Folder, Has.Count.EqualTo(1));
        }
    }
    [Test]
    public void FolderToStringTest()
    {
        string testItem = $"""
            Test1
                Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Folder.Item[0].CreateTime:MM/dd/yyyy HH:mm}
                Tags: foo: (bar, baz), foo2: (bar2, baz)
            """;
        Assert.That(Folder.ToString(), Is.EqualTo($"""
            Folder: Test
                Test1
                    Started 01/01/2000 01:01, Due 01/01/2000 01:01, Created {Folder.Item[0].CreateTime:MM/dd/yyyy HH:mm}
                Test2
                    Started 10/14/2025 10:01, Due 10/15/2025 12:10, Created {Folder.Item[1].CreateTime:MM/dd/yyyy HH:mm}

                Folder: TestInner
                    Test1
                        Started {Folder.Folder[0].Item[0].StartTime:MM/dd/yyyy HH:mm}, Due {Folder.Folder[0].Item[0].DueDate:MM/dd/yyyy HH:mm}, Created {Folder.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}"
            """));
    }
}
