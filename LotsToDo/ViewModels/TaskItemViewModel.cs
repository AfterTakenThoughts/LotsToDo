using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using LotsToDo.Backend.FileIO.Parser;
using LotsToDo.Backend.FileIO.Parser.AttributeInfo;
using LotsToDo.Backend.FileIO.Parser.ParserMethods;
using LotsToDo.Backend.ToDoData;

namespace LotsToDo.ViewModels;
/// <summary>
/// A ViewModel that represents <see cref="TaskItem"/>
/// </summary>
public partial class TaskItemViewModel : ViewModelBase
{
    public TaskItem Item { get; set; }
    public readonly ParseSingleAttribute ParseDueDate = new("Due", [
        new AttributeByWord(new MatchInfo("Due", StringComparison.InvariantCultureIgnoreCase), new([" ", ":", "by", "at"]), 1, ParseDirection.ParseRight)
    ]);
    public readonly ParseSingleAttribute ParseStartDate = new("Start", [
        new AttributeByWord(new MatchInfo("Start", StringComparison.InvariantCultureIgnoreCase), new([" ", ":", "by", "at"]), 1, ParseDirection.ParseRight)
    ]);
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
    public ObservableCollection<string> Tags
    {
        get
        {
            ObservableCollection<string> tagsContent = [];
            if (string.IsNullOrEmpty(StartDate) == false)
            {
                tagsContent.Add($"Starts {StartDate}");
            }
            if (string.IsNullOrEmpty(DueDate) == false)
            {
                tagsContent.Add($"Due {DueDate}");
            }
            if (string.IsNullOrEmpty(CreateDate) == false)
            {
                tagsContent.Add($"Created {CreateDate}");
            }
            foreach (KeyValuePair<string, List<string>> tag in Item.Tags)
            {
                tagsContent.Add(TaskItem.GetTagString(tag.Key, tag.Value));
            }
            return tagsContent;
        }
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
        if (DueDateString.Count != 0 && DateTime.TryParse(DueDateString[0], out DateTime dateTime))
        {
            Item.DueDate = dateTime;
        }

        List<string> StartDateString = ParseStartDate.ParseAttributes(remainingContent, out remainingContent);
        if (StartDateString.Count != 0 && DateTime.TryParse(StartDateString[0], out dateTime))
        {
            Item.StartDate = dateTime;
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