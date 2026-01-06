using System;
using System.IO;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO;

namespace LotsToDo.Tests.FileIOTest.Archive;

public class ArchiveExportTest
{
    public required TaskFolder Folder1 { get; set; }
    public required TaskFolder Folder2 { get; set; }
    public required string TestPath { get; set; }
    public required string FileName { get; set; }
    [SetUp]
    public void Setup()
    {
        TestPath = "Tests/TextExport1";
        FileName = "todo";

        Folder1 = new TaskFolder("Test",
            [
                new TaskItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1)),
                new TaskItem("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30), new()
                {
                    { "foo", ["bar", "baz"] },
                    { "foo2", ["bar2", "baz"] }
                })
            ],
            new TaskFolder("TestInner",
            [
                new TaskItem("Test1")
            ]),
            new TaskFolder("TestInner2",
            [
                new TaskItem("Test2")
            ])
        );
        Folder2 = new TaskFolder("TestOther",
            [
                new TaskItem("TestOtherContent")
            ]
        );
    }
    [Test]
    public void FileNameTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, Folder1, Folder2);

        string file = Path.GetFileName(Directory.GetFiles(TestPath)[0]);
        Assert.That(file, Is.EqualTo(FileName + ".txt"));
    }
    [Test]
    public void FileContentTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, Folder1, Folder2);
        string content = File.ReadAllText(Directory.GetFiles(TestPath)[0]);

        string testItem = $"""
            Folder: Test
                Test1
                    Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Folder1.Item[0].CreateTime:MM/dd/yyyy HH:mm}
                Test2
                    Start: 10/14/2025 10:01, Due: 10/15/2025 12:10, Created: {Folder1.Item[1].CreateTime:MM/dd/yyyy HH:mm}
                    Tags: foo: (bar, baz), foo2: (bar2, baz)
                Folder: TestInner
                    Test1
                        Created: {Folder1.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
                Folder: TestInner2
                    Test2
                        Created: {Folder1.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
            Folder: TestOther
                TestOtherContent
                    Created: {Folder1.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
            """;
        Assert.That(content, Is.EqualTo(testItem));
    }
    [TearDown]
    public void TearDown()
    {
        Directory.Delete($"{TestPath}\\..", true);
    }
}
