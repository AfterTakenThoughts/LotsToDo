using System;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using Avalonia.Metadata;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LotsToDo.Backend;

namespace LotsToDo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<TaskItemViewModel> ToDoItemList { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _textBoxItemEntry;
    public MainViewModel()
    {
        ToDoItemList = [
            new TaskItemViewModel(new("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1))),
            new TaskItemViewModel(new("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30)))
        ];
    }
    [RelayCommand]
    private void AddItem()
    {
        if (string.IsNullOrWhiteSpace(TextBoxItemEntry) == false)
        {
            ToDoItemList.Add(new TaskItemViewModel(new(TextBoxItemEntry)));
            TextBoxItemEntry = null;
        }
    }
    [RelayCommand]
    private void RemoveItem(TaskItemViewModel item)
    {
        // Remove the given item from the list
        ToDoItemList.Remove(item);
    }
}