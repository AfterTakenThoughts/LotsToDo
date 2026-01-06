// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Linq;
// using LotsToDo.Backend;
// using LotsToDo.Backend.FileIO;
// using NUnit.Framework.Legacy;

// namespace LotsToDo.Tests.FileIOTest.Archive;


// /// <summary>
// /// A set of test cases for when a user manually alters the archive file.
// /// </summary>
// /// <remarks> Generally cases that don't occur when normally exporting and importing the file. </remarks>

// public class UserInputImportTest
// {
//     public required string TestPath { get; set; }
//     public required string FileName { get; set; }
//     [SetUp]
//     public void Setup()
//     {
//         TestPath = "Tests/TextExport1";
//         FileName = "todo";
//     }
//     [Test]
//     public void ParseOtherStringTest()
//     {
//         string OtherString = """
//             Folder: Test
//                 Test1
//                     Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: 12/31/2025 04:25
//                 Test2
//                     Start: 10/14/2025 10:01, Due: 10/15/2025 12:10, Created: 12/31/2025 04:25
//                     Tags: foo: (bar, baz), foo2: (bar2, baz)
//                 Folder: TestInner
//                     Test1
//                         Created: 12/31/2025 04:25
//                 Folder: TestInner2
//                     Test2
//                         Created: 12/31/2025 04:25
//             """;
//         ToDoFolder OtherStringTest = new();
//         Assert.That(FileArchive.FolderFromString(OtherString)[0].ToString(), Is.EqualTo(OtherStringTest.ToString()));
//     }
// }
