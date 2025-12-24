using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LotsToDo.Backend;

// public interface IToDoItem
// {
//     public bool IsCompleted { get; set; }
//     public string Content { get; set; }
//     public DateTime StartTime { get; set; }
//     public DateTime DueDate { get; set; }
//     public DateTime CreateTime { get; set; }
//     public Dictionary<string, List<string>> Tags { get; set; }
//     public string ToString();
// }
public class ToDoItem
{
    public bool IsCompleted { get; set; }
    public string Content { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? DueDate { get; set; }
    public DateTime CreateTime { get; set; }
    public Dictionary<string, List<string>> Tags { get; set; }

    public static DateTime DefaultStartDate { get; } = DateTime.UtcNow;
    public static DateTime DefaultDueDate { get; } = DateTime.UtcNow.AddDays(1);

    public ToDoItem()
    {
        Content = "";
        Tags = [];
    }
    public ToDoItem(string content, DateTime? startTime = null, DateTime? dueDate = null, Dictionary<string, List<string>>? tags = default)
    {
        Content = content;
        StartTime = startTime;
        DueDate = dueDate;
        CreateTime = DateTime.UtcNow;
        Tags = tags ?? [];
    }

    public override string ToString()
    {
        string indent = "    ";

        StringBuilder item = new($"{Content}{Environment.NewLine}{indent}");
        item = TimeToString(item);
        if (Tags != null && Tags.Count != 0)
        {
            item = TagsToString(item, indent);
        }
        return item.ToString();
    }
    StringBuilder TimeToString(StringBuilder item)
    {
        if (StartTime != null)
        {
            item.Append($"Start: {StartTime:MM/dd/yyyy HH:mm}");
        }

        if (DueDate != null)
        {
            if (StartTime != null)
            {
                item.Append(", ");
            }
            item.Append($"Due: {DueDate:MM/dd/yyyy HH:mm}");
        }

        if (StartTime != null || DueDate != null)
        {
            item.Append(", ");
        }
        item.Append($"Created: {CreateTime:MM/dd/yyyy HH:mm}");

        return item;
    }
    StringBuilder TagsToString(StringBuilder item, string indent)
    {
        item.Append(Environment.NewLine);
        item.Append($"{indent}Tags: ");
        foreach (KeyValuePair<string, List<string>> tagPair in Tags)
        {
            item.Append($"{tagPair.Key}: (");
            for (int i = 0; i < tagPair.Value.Count; i++)
            {
                if (i == tagPair.Value.Count - 1)
                {
                    item.Append(tagPair.Value[i]);
                }
                else
                {
                    item.Append($"{tagPair.Value[i]}, ");
                }
            }
            if (tagPair.Key.Equals(Tags.Last().Key))
            {
                item.Append(')');
            }
            else
            {
                item.Append("), ");
            }
        }
        return item;
    }
}