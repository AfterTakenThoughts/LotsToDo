using System;
using LotsToDo.Backend;
using NUnit.Framework.Legacy;

namespace LotsToDo.Tests;

public class Tests
{
    ToDoFolder Folder { get; set; }
    [SetUp]
    public void Setup()
    {
        Folder =
            new ToDoFolder("Test", [new ToDoItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1)), new ToDoItem("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30))],
                new ToDoFolder("TestInner", [new ToDoItem("Test1")])
            );
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
        TestContext.Out.WriteLine(Folder.ToString());
        Assert.That(Folder.ToString(), Is.EqualTo("Folder: Test\n" +
            $"    Test1: Started 01/01/2000 01:01, Due 01/01/2000 01:01, Created {Folder.Item[0].CreateTime:MM/dd/yyyy HH:mm}.{System.Environment.NewLine}" +
            $"    Test2: Started 10/14/2025 10:01, Due 10/15/2025 12:10, Created {Folder.Item[1].CreateTime:MM/dd/yyyy HH:mm}.{System.Environment.NewLine}" +
            $"{System.Environment.NewLine}" +
            $"    Folder: TestInner\n" +
            $"        Test1: Started {Folder.Folder[0].Item[0].StartTime:MM/dd/yyyy HH:mm}, Due {Folder.Folder[0].Item[0].DueDate:MM/dd/yyyy HH:mm}, Created {Folder.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}.{System.Environment.NewLine}{System.Environment.NewLine}"
            ));
    }
}
