using System;
using System.IO;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO.ToDoFileFormats;
using LotsToDo.Backend.ToDoData;

namespace LotsToDo.Tests.FileIOTest.ToDoTxt;

public class ToDoTxtExportTest
{
    public required TaskFolder Folder { get; set; }
    public required string TestPath { get; set; }
    public required string FileName { get; set; }
    [SetUp]
    public void Setup()
    {
        TestPath = "Tests/TextExport2";
        FileName = "archive";

        Folder =
            new TaskFolder("Test", [new TaskItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1)), new TaskItem("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30))],
                new TaskFolder("TestInner", [new TaskItem("Test1")])
            );
    }
    [Test]
    public void FileNameTest()
    {
        ParseToDoTxt fileExport1 = new();
        fileExport1.Export(TestPath, FileName, Folder);

        string file = Path.GetFileName(Directory.GetFiles(TestPath)[0]);
        Assert.That(file, Is.EqualTo(FileName + ".txt"));
    }
    // [Test]
    // public void FileContentTest()
    // {
    //     throw new NotImplementedException();
    //     FileArchive fileExport = new();
    //     fileExport.Export(Folder, TestPath, FileName);
    //     string content = File.ReadAllText(Directory.GetFiles(TestPath)[0]);

    //     string testItem = $"""
    //         Folder: Test
    //             Test1
    //                 Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Folder.Item[0].CreateTime:MM/dd/yyyy HH:mm}
    //             Test2
    //                 Start: 10/14/2025 10:01, Due: 10/15/2025 12:10, Created: {Folder.Item[1].CreateTime:MM/dd/yyyy HH:mm}
    //                 Tags: foo: (bar, baz), foo2: (bar2, baz)
    //             Folder: TestInner
    //                 Test1
    //                     Created: {Folder.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
    //             Folder: TestInner2
    //                 Test2
    //                     Created: {Folder.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
    //         """;
    //     Assert.That(content, Is.EqualTo(testItem));
    // }
    [TearDown]
    public void TearDown()
    {
        Directory.Delete($"{TestPath}\\..", true);
    }
}
