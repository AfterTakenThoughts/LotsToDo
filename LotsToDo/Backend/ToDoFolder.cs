using System;
using System.Collections.Generic;
using System.Text;
using LotsToDo.Backend.FileIO;
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
        return FileArchive.GetString(this, 0);
    }
}