using System.Collections.Generic;
using LotsToDo.Backend.FileIO.ToDoFileFormats;

namespace LotsToDo.Backend.ToDoData;


public class TaskFolder
{
    public TaskFolder()
    {
        FolderName = "";
        Item = [];
        Folder = [];
    }
    public TaskFolder(string folderName, List<TaskItem>? item = null, params List<TaskFolder> folder)
    {
        FolderName = folderName;
        Item = item ?? [];
        Folder = folder;
    }
    public string FolderName { get; set; }
    public List<TaskItem> Item { get; set; }
    public List<TaskFolder> Folder { get; set; }
    public override string ToString()
    {
        return ParseArchive.GetString(this, 0);
    }
}