using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace LotsToDo.Backend;

public class TaskList
{
    public TaskList()
    {
        ToDoFolderList = [];
    }
    public List<TaskFolder> ToDoFolderList { get; set; }
    public void Export()
    {
        // StringBuilder text = new();
        // for (int i = 0; i < ToDoFolderList.Count; i++)
        // {
        //     text.AppendLine(GetFolderText(ToDoFolderList[i]));
        // }
        // File.WriteAllText("todo.txt", text.ToString());
    }
}