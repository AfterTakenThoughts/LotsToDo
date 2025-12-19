using System;

namespace LotsToDo.Backend;
public interface IToDoItem
{
    public string Content { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreateTime { get; set; }
    public string ToString();
}
public class ToDoItem : IToDoItem
{
    public ToDoItem(string content, DateTime? startTime = null, DateTime? dueDate = null)
    {
        Content = content;
        StartTime = startTime.GetValueOrDefault(DefaultStartDate);
        DueDate = dueDate.GetValueOrDefault(DefaultDueDate);
        CreateTime = DateTime.UtcNow;
    }
    public string Content { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime CreateTime { get; set; }

    public static DateTime DefaultStartDate { get; set; } = DateTime.UtcNow;
    public static DateTime DefaultDueDate { get; set; } = DateTime.UtcNow.AddDays(1);

    public override string ToString()
    {
        return $"{Content}: Started {StartTime:MM/dd/yyyy HH:mm}, Due {DueDate:MM/dd/yyyy HH:mm}, Created {CreateTime:MM/dd/yyyy HH:mm}.";
    }
}