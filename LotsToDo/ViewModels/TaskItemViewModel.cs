using System;
using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO.Parser.AttributeInfo;
using LotsToDo.Backend.FileIO.Parser.ParserMethods;
using LotsToDo.ViewModels;

namespace LotsToDo.ViewModels;
/// <summary>
/// A ViewModel that represents <see cref="TaskItem"/>
/// </summary>
public partial class TaskItemViewModel : ViewModelBase
{
    public TaskItem Item { get; set; }
    public readonly ParseSingleAttribute ParseDueDate = new("Due", [new AttributeByWord("Due", new([" ", ":", "by", "at"]), 1, Backend.FileIO.Parser.ParseDirection.ParseRight)]);
    public readonly ParseSingleAttribute ParseStartDate = new("Start", [new AttributeByWord("Start", new([" ", ":", "by", "at"]), 1, Backend.FileIO.Parser.ParseDirection.ParseRight)]);
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
        Item = new();
    }
    public TaskItemViewModel(string content)
    {
        Item = new();

        List<string> DueDateString = ParseDueDate.ParseAttributes(content, out string remainingContent);
        if (DueDateString.Count != 0)
        {
            Item.DueDate = DateTime.Parse(DueDateString[0]);
        }

        List<string> StartDateString = ParseStartDate.ParseAttributes(remainingContent, out remainingContent);
        if (StartDateString.Count != 0)
        {
            Item.DueDate = DateTime.Parse(StartDateString[0]);
        }

        Item.Content = remainingContent;
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