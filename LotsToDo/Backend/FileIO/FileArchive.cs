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
//         Start: 01/01/2000 01:01, Due: 01/01/2000 01:01, Created: 12/31/2025 00:34
//     Test2
//         Start: 10/14/2025 10:01, Due: 10/15/2025 12:10, Created: 12/31/2025 00:34
//         Tags: foo: (bar, baz), foo2: (bar2, baz)
//     Folder: TestInner
//         Test1
//             Created: 12/31/2025 00:34
//     Folder: TestInner2
//         Test2
//             Created: 12/31/2025 00:34
// Folder: TestOther
//     TestOtherContent
//         Created: 12/31/2025 00:34
public class FileArchive : IToDoFileIO
{
    class FolderIndent
    {
        public FolderIndent()
        {
            Folder = new();
            IndentLength = -1;
        }
        public FolderIndent(ToDoFolder folder, int indentLevel)
        {
            Folder = folder;
            IndentLength = indentLevel;
        }
        public ToDoFolder Folder { get; set; }
        public int IndentLength { get; set; }
    }

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
        ToDoFolder otherContent = new("Other Items");
        using StringReader reader = new(content);

        Stack<FolderIndent> parentBranch = [];
        FolderIndent? folderBuild = null;
        int notCapturedIndent = -1;

        ToDoItem? itemBuild = null;
        //Used to block creating a new item until it parses properties (detected by indents)
        bool isParsingProperties = false;

        for (string? line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            int indentLength = IndentHandling.GetIndentLength(line);
            string noIndentContents = line.TrimStart();
            string[] splitLines = noIndentContents.Split(':');

            //Happens on next loop to capture indent.
            if (folderBuild != null && folderBuild.IndentLength == notCapturedIndent)
            {
                folderBuild.IndentLength = IndentHandling.GetIndentLength(line);
            }
            if (itemBuild != null && folderBuild != null && indentLength != folderBuild.IndentLength)
            {
                itemBuild = ParseToDoItem.ParseProperties(itemBuild, noIndentContents);
                isParsingProperties = true;
            }

            if (splitLines[0].TrimEnd().EndsWith("Folder"))
            {
                GetFolder(splitLines[1], indentLength);
            }
            else
            {
                GetItem(noIndentContents, indentLength);
            }
        }
        parentBranch ??= [];
        if (folderBuild != null)
        {
            if (itemBuild != null)
            {
                folderBuild.Folder.Item.Add(itemBuild);
            }
            parentBranch.Push(folderBuild);
        }
        RestructureFolder();
        folders.Add(parentBranch.Peek().Folder);
        return folders;

        void GetFolder(string name, int indentLength)
        {
            if (folderBuild == null)
            {
                folderBuild = new(new(name.Trim()), notCapturedIndent);
            }
            else
            {
                if (itemBuild != null)
                {
                    folderBuild.Folder.Item.Add(itemBuild);
                    CreateNewItem();
                }
                parentBranch.Push(folderBuild);
                if (indentLength >= folderBuild.IndentLength)
                {
                    RestructureFolder(indentLength);
                }
                folderBuild = new(new(name.Trim()), notCapturedIndent);
            }
        }
        void GetItem(string content, int indentLength)
        {
            if (itemBuild != null)
            {
                if (folderBuild == null)
                {
                    //Special case to capture other objects not categorized by folder.
                    otherContent.Item.Add(itemBuild);
                    CreateNewItem(content);
                }
                //Normal case
                else if (indentLength == folderBuild.IndentLength && isParsingProperties)
                {
                    folderBuild.Folder.Item.Add(itemBuild);
                    CreateNewItem(content);
                }
                //Special case where a seperator for a task occurs.
                else if (String.IsNullOrWhiteSpace(content))
                {
                    folderBuild.Folder.Item.Add(itemBuild);
                    CreateNewItem();
                }
            }
            else
            {
                CreateNewItem(content);
            }
        }
        void RestructureFolder(int indentLength = -1)
        {
            while (indentLength >= parentBranch.Peek().IndentLength && parentBranch.Count > 0)
            {
                ToDoFolder childFolder = parentBranch.Pop().Folder;

                FolderIndent currentItem = parentBranch.Peek();
                currentItem.Folder.Folder.Add(childFolder);

                parentBranch.Pop();
                parentBranch.Push(currentItem);
            }
        }
        void CreateNewItem(string content = "")
        {
            itemBuild = new(content);
            isParsingProperties = false;
        }
    }

    public static string GetString(ToDoFolder FolderItem, int indentLength)
    {
        string indentLiteral = "    ";
        StringBuilder text = new();

        Stack<FolderIndent> folderStack = new();
        folderStack.Push(new(FolderItem, 0));

        while (folderStack.Count > 0)
        {
            FolderIndent folderObject = folderStack.Pop();
            ToDoFolder folder = folderObject.Folder;
            int indentLevel = folderObject.IndentLength;

            text.Append(GetFolderString(folder, indentLiteral, indentLength + indentLevel));
            if (folder.Item != null && folder.Item.Count != 0)
            {
                text.Append($"{Environment.NewLine}{GetContentString(folder, indentLiteral, indentLength + indentLevel)}");
            }
            if (folder.Folder != null && folder.Folder.Count != 0)
            {
                for (int i = folder.Folder.Count - 1; i >= 0; i--)
                {
                    folderStack.Push(new(folder.Folder[i], indentLevel + 1));
                }
            }

            //Check after pop, not before pop to prevent adding a new line at the end of string.
            if (folderStack.Count > 0)
            {
                text.AppendLine();
            }
        }
        return text.ToString();


        static string GetFolderString(ToDoFolder FolderItem, string indentLiteral, int indentLength)
        {
            return $"{IndentHandling.GetIndent(indentLiteral, indentLength)}Folder: {FolderItem.FolderName}";
        }
        static string GetContentString(ToDoFolder FolderItem, string indentLiteral, int indentLength)
        {
            string itemIndent = IndentHandling.GetIndent(indentLiteral, indentLength + 1);
            return String.Join(Environment.NewLine, FolderItem.Item.Select(x => IndentHandling.IndentString(x.ToString(), itemIndent)));
        }
    }


}