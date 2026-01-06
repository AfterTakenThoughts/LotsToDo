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
    [ObservableProperty]
    private bool _IsCompleted;

    [ObservableProperty]
    private string _Content;

    [ObservableProperty]
    private DateTime? _StartTime;

    [ObservableProperty]
    private DateTime? _DueDate;

    [ObservableProperty]
    private DateTime _CreateTime;

    public TaskItemViewModel()
    {
        Content = "";
    }

    public TaskItemViewModel(TaskItem item)
    {
        IsCompleted = item.IsCompleted;
        Content = item.Content;
        StartTime = item.StartTime;
        DueDate = item.DueDate;
        CreateTime = item.CreateTime;
    }
    public TaskItem GetItem()
    {
        return new TaskItem(Content, StartTime, DueDate);
    }
}