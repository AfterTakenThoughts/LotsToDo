using System;
using System.Collections.Generic;
using System.IO;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO;

namespace LotsToDo.Tests.FileIOTest.Archive;

public class ArchiveImportTest
{
    public required ToDoFolder Folder1 { get; set; }
    public required ToDoFolder Folder2 { get; set; }
    public required string TestPath { get; set; }
    public required string FileName { get; set; }
    [SetUp]
    public void Setup()
    {
        TestPath = "Tests/TextExport1";

        FileName = "todo";
                Folder1 = new ToDoFolder("Test",
            [
                new ToDoItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1)),
                new ToDoItem("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30), new()
                {
                    { "foo", ["bar", "baz"] },
                    { "foo2", ["bar2", "baz"] }
                })
            ],
            new ToDoFolder("TestInner",
            [
                new ToDoItem("Test1")
            ]),
            new ToDoFolder("TestInner2",
            [
                new ToDoItem("Test2")
            ])
        );
        Folder2 = new ToDoFolder("TestOther",
            [
                new ToDoItem("TestOtherContent")
            ]
        );
    }
    [Test]
    public void FileReadTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, Folder1, Folder2);

        if (fileExport.Import(TestPath, FileName, out List<ToDoFolder> testFolder))
        {
            Assert.That(testFolder[0], Is.EqualTo(Folder1));
        }
        else
        {
            Assert.Fail();
        }
    }
    [TearDown]
    public void TearDown()
    {
        Directory.Delete($"{TestPath}\\..", true);
    }
}
