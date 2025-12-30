using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend.FileIO;
// Example string format:
// Folder: Test
//     Test1
//         Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: {Folder.Item[0].CreateTime:MM/dd/yyyy HH:mm}
//     Test2
//         Start: 10/14/2025 10:01, Due: 10/15/2025 12:10, Created: {Folder.Item[1].CreateTime:MM/dd/yyyy HH:mm}
//         Tags: foo: (bar, baz), foo2: (bar2, baz)
//     Folder: TestInner
//         Test1
//             Created: {Folder.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
//     Folder: TestInner2
//         Test2
//             Created: {Folder.Folder[0].Item[0].CreateTime:MM/dd/yyyy HH:mm}
public class FileArchive : IToDoFileIO
{
    public void Export(string relativePath, string fileName, params ToDoFolder[] folders)
    {
        string fullPath = relativePath + "/" + fileName + ".txt";
        string content = String.Join(Environment.NewLine, folders.Select(x => x.ToString()));

        Directory.CreateDirectory(relativePath);
        File.AppendAllText(fullPath, content);
    }

    public bool Import(string relativePath, string fileName, out List<ToDoFolder> folders)
    {
        string fullPath = relativePath + "/" + fileName + ".txt";

        if (File.Exists(fullPath))
        {
            folders = FolderFromString(File.ReadAllText(fullPath));
            return true;
        }
        else
        {
            folders = [new()];
            return false;
        }
    }

    public static List<ToDoFolder> FolderFromString(string content)
    {
        List<ToDoFolder> folders = [];

        using StringReader reader = new(content);
        Stack<int> indentLevel = [];
        List<int> FolderLevelIndex = [];

        for (string? line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            int indentCount = IndentHandling.GetIndentCount(line);
            string noIndentContents = line.TrimStart();
            string[] splitLines = noIndentContents.Split(':');
            if (splitLines.Length == 1)
            {

            }
            else
            {
                if (splitLines[0].Trim().Equals("Folder"))
                {
                    //FolderLevelIndex.Push(new ToDoFolder(splitLines[1].Trim()));
                }
            }
        }
        return folders;
    }

    public static string GetString(ToDoFolder FolderItem, int indentLength)
    {
        string indentLiteral = "    ";
        StringBuilder text = new();

        text.Append(GetFolderString(FolderItem, indentLiteral, indentLength));
        if (FolderItem.Item != null && FolderItem.Item.Count != 0)
        {
            text.Append(Environment.NewLine);
            text.Append(GetFolderContentString(FolderItem, indentLiteral, indentLength));
        }
        if (FolderItem.Folder != null && FolderItem.Folder.Count != 0)
        {
            text.Append(Environment.NewLine);
            text.Append(GetSubFolderString(FolderItem, indentLength));
        }
        return text.ToString();

        static string GetFolderString(ToDoFolder FolderItem, string indentLiteral, int indentLength)
        {
            return $"{IndentHandling.GetIndent(indentLiteral, indentLength)}Folder: {FolderItem.FolderName}";
        }
        static string GetFolderContentString(ToDoFolder FolderItem, string indentLiteral, int indentLength)
        {
            string itemIndent = IndentHandling.GetIndent(indentLiteral, indentLength + 1);
            return String.Join(Environment.NewLine, FolderItem.Item.Select(x => IndentHandling.IndentString(x.ToString(), itemIndent)));
        }
        static string GetSubFolderString(ToDoFolder FolderItem, int indentLength)
        {
            return String.Join(Environment.NewLine, FolderItem.Folder.Select(x => GetString(x, indentLength + 1)));
        }
    }
}