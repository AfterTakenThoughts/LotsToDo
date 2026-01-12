using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LotsToDo.Backend;
using LotsToDo.ViewModels;

namespace LotsToDo.ViewModels;
/// <summary>
/// This is a ViewModel which represents a <see cref="Models.ToDoItem"/>
/// </summary>
public partial class TaskItemViewModel : ViewModelBase
{
    public TaskItem Item { get; set; }
    public bool IsCompleted
    {
        get => Item.IsCompleted;
        set => SetProperty(Item.IsCompleted, value, Item, (item, isCompleted) => Item.IsCompleted = isCompleted);
    }

    public string Content
    {
        get => Item.Content;
        set => SetProperty(Item.Content, value, Item, (item, content) => Item.Content.Equals(content));
    }

    public string? StartDate => GetDateTimeString(DateTime.Now, Item.StartDate, 10);
    public string? DueDate => GetDateTimeString(DateTime.Now, Item.DueDate, 10);
    public string? CreateDate => GetDateTimeString(DateTime.Now, Item.CreateDate, 10);

    public TaskItemViewModel()
    {
        Content = "";
        Item = new();
    }

    public TaskItemViewModel(TaskItem item)
    {
        Item = item;
    }

    public static string GetDateTimeString(DateTime currentTime, DateTime? targetTime, int maxDaysBefore)
    {
        if (targetTime == null)
        {
            return "";
        }
        DateTime newTargetTime = targetTime.GetValueOrDefault();
        if (newTargetTime.Date == currentTime.Date)
        {
            return $"by {newTargetTime.TimeOfDay - currentTime.TimeOfDay}";
        }
        else
        {
            int daysBefore = (newTargetTime - currentTime).Days;
            if (maxDaysBefore > daysBefore && daysBefore > 0)
            {
                return $"in {daysBefore} day(s)";
            }
            else
            {
                return $"by {newTargetTime.ToShortDateString()}";
            }
        }
    }
}