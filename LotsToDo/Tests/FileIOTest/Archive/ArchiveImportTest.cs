using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO;
using NUnit.Framework.Legacy;

namespace LotsToDo.Tests.FileIOTest.Archive;

public class ArchiveImportTest
{
    public required TaskFolder SimpleFolder { get; set; }
    public required TaskFolder SimpleFolderWithTime { get; set; }
    public required TaskFolder SimpleFolderWithTags { get; set; }
    public required TaskFolder ComplexFolder { get; set; }
    public required string TestPath { get; set; }
    public required string FileName { get; set; }
    [SetUp]
    public void Setup()
    {
        TestPath = "Tests/TextExport1";

        FileName = "todo";
        SimpleFolder = new TaskFolder("Test1",
            [
                new TaskItem("TestOtherContent")
            ]
        );
        SimpleFolderWithTime = new TaskFolder("Test2",
            [
                new TaskItem("TestOtherContent", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1))
            ]
        );
        SimpleFolderWithTags = new TaskFolder("Test3",
            [
                new TaskItem("TestOtherContent", null, null, new()
                {
                    { "foo", ["bar", "baz"] },
                    { "foo2", ["bar2", "baz"] }
                })
            ]
        );
        ComplexFolder = new TaskFolder("Test4",
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
    }
    [Test]
    public void ParseSimpleTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, SimpleFolder);

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
        {
            Assert.That(testFolder[0].ToString(), Is.EqualTo(SimpleFolder.ToString()));
        }
        else
        {
            Assert.Fail();
        }
    }
    [Test]
    public void ParseTimeTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, SimpleFolderWithTime);

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
        {
            Assert.That(testFolder[0].ToString(), Is.EqualTo(SimpleFolderWithTime.ToString()));
        }
        else
        {
            Assert.Fail();
        }
    }
    [Test]
    public void ParseTagsTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, SimpleFolderWithTags);

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
        {
            Assert.That(testFolder[0].ToString(), Is.EqualTo(SimpleFolderWithTags.ToString()));
        }
        else
        {
            Assert.Fail();
        }
    }
    [Test]
    public void ParseComplexTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, ComplexFolder);

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
        {
            Assert.That(testFolder[0].ToString(), Is.EqualTo(ComplexFolder.ToString()));
        }
        else
        {
            Assert.Fail();
        }
    }
    [Test]
    public void ParseAllTest()
    {
        FileArchive fileExport = new();
        fileExport.Export(TestPath, FileName, SimpleFolder, SimpleFolderWithTime, SimpleFolderWithTags, ComplexFolder);

        if (fileExport.Import(TestPath, FileName, out List<TaskFolder> testFolder))
        {
            TestContext.Out.WriteLine(File.ReadAllText(TestPath + "/" + FileName + ".txt"));
            using (Assert.EnterMultipleScope())
            {
                Assert.That(testFolder.Find(x => x.FolderName == "Test1").ToString(), Is.EqualTo(SimpleFolder.ToString()));
                Assert.That(testFolder.Find(x => x.FolderName == "Test2").ToString(), Is.EqualTo(SimpleFolderWithTime.ToString()));
                Assert.That(testFolder.Find(x => x.FolderName == "Test3").ToString(), Is.EqualTo(SimpleFolderWithTags.ToString()));
                Assert.That(testFolder.Find(x => x.FolderName == "Test4").ToString(), Is.EqualTo(ComplexFolder.ToString()));
            }
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
