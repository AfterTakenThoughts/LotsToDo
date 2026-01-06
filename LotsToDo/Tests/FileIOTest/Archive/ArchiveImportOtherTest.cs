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
    public required TaskFolder EmptyFolder { get; set; }
    public required TaskFolder SameNameFolder { get; set; }
    public required string TestPath { get; set; }
    public required string FileName { get; set; }
    [SetUp]
    public void Setup()
    {
        TestPath = "Tests/TextExport1";

        FileName = "todo";
        EmptyFolder = new TaskFolder("Test");
        SameNameFolder = new TaskFolder("Folder",
            [
                new TaskItem("TestOtherContent")
            ]
        );
    }
    [Test]
    public void ParseEmptyFolderTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, EmptyFolder);

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
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

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
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
