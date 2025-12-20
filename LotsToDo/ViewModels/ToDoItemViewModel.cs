using System;
using CommunityToolkit.Mvvm.ComponentModel;
using LotsToDo.Backend;
using LotsToDo.ViewModels;


/// <summary>
/// This is a ViewModel which represents a <see cref="Models.ToDoItem"/>
/// </summary>
public partial class ToDoItemViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _IsCompleted;

    [ObservableProperty]
    private string _Content;

    [ObservableProperty]
    private DateTime _StartTime;

    [ObservableProperty]
    private DateTime _DueDate;

    [ObservableProperty]
    private DateTime _CreateTime;

    public ToDoItemViewModel()
    {

    }

    public ToDoItemViewModel(ToDoItem item)
    {
        IsCompleted = item.IsCompleted;
        Content = item.Content;
        StartTime = item.StartTime;
        DueDate = item.DueDate;
        CreateTime = item.CreateTime;
    }
    public ToDoItem GetItem()
    {
        return new ToDoItem(Content, StartTime, DueDate);
    }
}