using System;
using System.Collections.ObjectModel;
using LotsToDo.Backend;

namespace LotsToDo.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public ObservableCollection<TaskItem> ToDoItemList { get; set; }
    public MainViewModel()
    {
        ToDoItemList = [
            new TaskItem("Test1", new DateTime(2000, 1, 1, 1, 1, 1), new DateTime(2000, 1, 1, 1, 1, 1)),
            new TaskItem("Test2", new DateTime(2025, 10, 14, 10, 1, 1), new DateTime(2025, 10, 15, 12, 10, 30))
        ];
    }
}
