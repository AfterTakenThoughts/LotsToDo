using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO;
using NUnit.Framework.Legacy;

namespace LotsToDo.Tests.FileIOTest.Archive;

public class ArchiveImportOtherTest
{
    public required ToDoFolder EmptyFolder { get; set; }
    public required ToDoFolder SameNameFolder { get; set; }
    public required string TestPath { get; set; }
    public required string FileName { get; set; }
    [SetUp]
    public void Setup()
    {
        TestPath = "Tests/TextExport1";

        FileName = "todo";
        EmptyFolder = new ToDoFolder("Test");
        SameNameFolder = new ToDoFolder("Folder",
            [
                new ToDoItem("TestOtherContent")
            ]
        );
    }
    [Test]
    public void ParseEmptyFolderTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, EmptyFolder);

        if (fileExport.Import(TestPath, FileName, out List<ToDoFolder> testFolder))
        {
            Assert.That(testFolder[0].ToString(), Is.EqualTo(EmptyFolder.ToString()));
        }
        else
        {
            Assert.Fail();
        }
    }
    [Test]
    public void ParseSameNameTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, SameNameFolder);

        if (fileExport.Import(TestPath, FileName, out List<ToDoFolder> testFolder))
        {
            Assert.That(testFolder[0].ToString(), Is.EqualTo(SameNameFolder.ToString()));
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
