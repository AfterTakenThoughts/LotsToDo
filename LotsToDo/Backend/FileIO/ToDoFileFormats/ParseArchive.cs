using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LotsToDo.Backend.StringHandlingExtensions;
using LotsToDo.Backend.ToDoData;

namespace LotsToDo.Backend.FileIO.ToDoFileFormats;
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
public class ParseArchive : IParseToDo
{
    class FolderIndent
    {
        public FolderIndent()
        {
            Folder = new();
            IndentLength = -1;
        }
        public FolderIndent(TaskFolder folder, int indentLevel)
        {
            Folder = folder;
            IndentLength = indentLevel;
        }
        public TaskFolder Folder { get; set; }
        public int IndentLength { get; set; }
    }

    public void Export(string relativePath, string fileName, params TaskFolder[] folders)
    {
        string fullPath = relativePath + "/" + fileName + ".txt";
        string content = string.Join(Environment.NewLine, folders.Select(x => x.ToString()));

        Directory.CreateDirectory(relativePath);
        File.AppendAllText(fullPath, content);
    }

    public bool Import(string relativePath, string fileName, out List<TaskFolder> folders)
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

    public static List<TaskFolder> FolderFromString(string content)
    {
        List<TaskFolder> folders = [];
        TaskFolder otherContent = new("Other Items");
        using StringReader reader = new(content);

        Stack<FolderIndent> folderBranch = [];
        int notCapturedIndent = -1;

        TaskItem? itemBuild = null;
        //Used to block creating a new item until it parses properties (detected by indents)
        bool isParsingProperties = false;

        for (string? line = reader.ReadLine(); line != null; line = reader.ReadLine())
        {
            int indentLength = IndentHandling.GetIndentLength(line);
            string noIndentContents = line.TrimStart();
            string[] splitLines = noIndentContents.Split(':');

            //Happens on next loop to capture indent.
            if (folderBranch.Count != 0 && folderBranch.Peek().IndentLength == notCapturedIndent)
            {
                folderBranch.Peek().IndentLength = IndentHandling.GetIndentLength(line);
            }
            if (itemBuild != null && folderBranch.Count != 0 && indentLength != folderBranch.Peek().IndentLength)
            {
                itemBuild = ParseTaskItem.ParseProperties(itemBuild, noIndentContents);
                isParsingProperties = true;
            }

            if (splitLines[0].TrimEnd().EndsWith("Folder"))
            {
                AddAndCreateFolder(splitLines[1], indentLength);
            }
            else
            {
                AddAndCreateItem(noIndentContents, indentLength);
            }
        }
        folderBranch ??= [];
        AddAndCreateItem("", 0);
        RestructureFolder();
        AddToListing();
        return folders;

        void AddAndCreateFolder(string name, int indentLength)
        {
            if (folderBranch.Count != 0)
            {
                if (itemBuild != null)
                {
                    folderBranch.Peek().Folder.Item.Add(itemBuild);
                    ResetItem();
                }
                if (indentLength < folderBranch.Peek().IndentLength)
                {
                    RestructureFolder(indentLength);
                    AddToListing(indentLength);
                }
            }
            folderBranch.Push(new(new(name.Trim()), notCapturedIndent));
        }
        void AddAndCreateItem(string content, int indentLength)
        {
            if (itemBuild != null)
            {
                if (folderBranch.Count == 0)
                {
                    //Special case to capture other objects not categorized by folder.
                    otherContent.Item.Add(itemBuild);
                    CreateNewItem(content);
                }
                //Normal case
                else if (indentLength == folderBranch.Peek().IndentLength && isParsingProperties)
                {
                    folderBranch.Peek().Folder.Item.Add(itemBuild);
                    CreateNewItem(content);
                }
                //Special case where a seperator for a task occurs.
                else if (string.IsNullOrWhiteSpace(content))
                {
                    folderBranch.Peek().Folder.Item.Add(itemBuild);
                    CreateNewItem();
                }
                else if (isParsingProperties == false)
                {
                    itemBuild.Content += content;
                }
            }
            else
            {
                CreateNewItem(content);
            }
        }
        void RestructureFolder(int indentLength = -1)
        {
            while (indentLength < folderBranch.Peek().IndentLength && folderBranch.Count >= 2)
            {
                TaskFolder childFolder = folderBranch.Pop().Folder;

                FolderIndent currentItem = folderBranch.Peek();
                currentItem.Folder.Folder.Add(childFolder);

                folderBranch.Pop();
                folderBranch.Push(currentItem);
            }
        }
        void AddToListing(int indentLength = -1)
        {
            if (indentLength < folderBranch.Peek().IndentLength && folderBranch.Count == 1)
            {
                folders.Add(folderBranch.Pop().Folder);
            }
        }
        void CreateNewItem(string content = "")
        {
            itemBuild = new(content);
            isParsingProperties = false;
        }
        void ResetItem()
        {
            itemBuild = null;
            isParsingProperties = false;
        }
    }

    public static string GetString(TaskFolder FolderItem, int indentLength)
    {
        string indentLiteral = "    ";
        StringBuilder text = new();

        Stack<FolderIndent> folderStack = new();
        folderStack.Push(new(FolderItem, 0));

        while (folderStack.Count > 0)
        {
            FolderIndent folderObject = folderStack.Pop();
            TaskFolder folder = folderObject.Folder;
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


        static string GetFolderString(TaskFolder FolderItem, string indentLiteral, int indentLength)
        {
            return $"{IndentHandling.GetIndent(indentLiteral, indentLength)}Folder: {FolderItem.FolderName}";
        }
        static string GetContentString(TaskFolder FolderItem, string indentLiteral, int indentLength)
        {
            string itemIndent = IndentHandling.GetIndent(indentLiteral, indentLength + 1);
            return string.Join(Environment.NewLine, FolderItem.Item.Select(x => IndentHandling.IndentString(x.ToString(), itemIndent)));
        }
    }


}