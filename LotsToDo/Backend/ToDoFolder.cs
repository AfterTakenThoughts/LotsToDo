using System.Collections.Generic;
using System.Text;

namespace LotsToDo.Backend;
public class ToDoFolder(string folderName, List<IToDoItem> item, params List<ToDoFolder> folder)
{
    public string FolderName { get; set; } = folderName;
    public List<IToDoItem> Item { get; set; } = item;
    public List<ToDoFolder> Folder { get; set; } = folder;
    public string ToString(int depth = 0)
    {
        return GetString(this, depth);
    }
    static string GetString(ToDoFolder folder, int indentLength = 0)
    {
        if (folder.Item != null)
        {
            StringBuilder text = new();
            text.AppendLine(GetFolderString(folder, indentLength));
            if (folder.Folder != null)
            {
                for (int i = 0; i < folder.Folder.Count; i++)
                {
                    text.Append(GetString(folder.Folder[i], indentLength + 1));
                }
            }
            return text.ToString();
        }
        else
        {
            return $"{GetIndent(indentLength)}{folder.FolderName}";
        }
    }
    static string GetFolderString(ToDoFolder folder, int indentLength)
    {
        StringBuilder text = new($"{GetIndent(indentLength)}Folder: {folder.FolderName}\n");

        string itemIndent = GetIndent(indentLength + 1);
        for (int i = 0; i < folder.Item.Count; i++)
        {
            text.AppendLine($"{itemIndent}{folder.Item[i].ToString()}");
        }
        return text.ToString();
    }
    static string GetIndent(int indentLength)
    {
        string indentLiteral = "    ";
        StringBuilder indent = new();
        for (int i = 0; i < indentLength; i++)
        {
            indent.Append(indentLiteral);
        }
        return indent.ToString();
    }
}