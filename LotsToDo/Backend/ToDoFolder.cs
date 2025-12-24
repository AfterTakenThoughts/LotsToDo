using System;
using System.Collections.Generic;
using System.Text;
using LotsToDo.Backend.StringHandlingExtensions;

namespace LotsToDo.Backend;


public class ToDoFolder
{
    public ToDoFolder()
    {
        FolderName = "";
        Item = [];
        Folder = [];
    }
    public ToDoFolder(string folderName, List<ToDoItem>? item = null, params List<ToDoFolder> folder)
    {
        FolderName = folderName;
        Item = item ?? [];
        Folder = folder;
    }
    public string FolderName { get; set; }
    public List<ToDoItem> Item { get; set; }
    public List<ToDoFolder> Folder { get; set; }
    public override string ToString()
    {
        return ToString(0);
    }
    public string ToString(int indentLength)
    {
        string indentLiteral = "    ";
        StringBuilder text = new();

        text.Append(GetFolderString(indentLiteral, indentLength));
        if (Item != null && Item.Count != 0)
        {
            text.Append(Environment.NewLine);
            text.Append(GetFolderContentString(indentLiteral, indentLength));
        }
        if (Folder != null && Folder.Count != 0)
        {
            text.Append(Environment.NewLine);
            text.Append(GetSubFolderString(indentLength));
        }
        return text.ToString();
    }
    string GetFolderString(string indentLiteral, int indentLength)
    {
        return $"{IndentHandling.GetIndent(indentLiteral, indentLength)}Folder: {FolderName}";
    }
    string GetFolderContentString(string indentLiteral, int indentLength)
    {
        StringBuilder text = new();
        string itemIndent = IndentHandling.GetIndent(indentLiteral, indentLength + 1);
        for (int i = 0; i < Item.Count; i++)
        {
            if (i == Item.Count - 1)
            {
                text.Append(IndentHandling.IndentString(Item[i].ToString(), itemIndent));
            }
            else
            {
                text.AppendLine(IndentHandling.IndentString(Item[i].ToString(), itemIndent));
            }
        }
        return text.ToString();
    }
    string GetSubFolderString(int indentLength)
    {
        StringBuilder text = new();
        for (int i = 0; i < Folder.Count; i++)
        {
            if (i == Folder.Count - 1)
            {
                text.Append(Folder[i].ToString(indentLength + 1));
            }
            else
            {
                text.AppendLine(Folder[i].ToString(indentLength + 1));
            }
        }
        return text.ToString();
    }
}