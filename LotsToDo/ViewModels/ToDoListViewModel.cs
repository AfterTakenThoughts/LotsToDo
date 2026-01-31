using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LotsToDo.Backend;
using LotsToDo.Backend.FileIO.Settings;
using LotsToDo.Backend.FileIO.ToDoFileFormats;
using LotsToDo.Backend.ToDoData;

namespace LotsToDo.ViewModels;

///<summary>content with some stuff</summary>
public partial class ToDoListViewModel : ViewModelBase
{
    public ObservableCollection<TaskItemViewModel> ToDoItemList { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(AddItemCommand))]
    private string? _textBoxItemEntry;
    public ToDoListViewModel()
    {
        //TODO: Use the location to actual settings file when proper settings import is implemented.
        ParseSettings.Import("", "");
        ParseArchive fileParser = new();
        string? relativePath = Path.GetDirectoryName(Settings.ToDoDataLocation);
        string? fileName = Path.GetFileNameWithoutExtension(Settings.ToDoDataLocation);

        ToDoItemList = [];
        if (relativePath != null && fileName != null && fileParser.Import(relativePath, fileName, out List<TaskFolder> folderList))
        {
            foreach (TaskFolder folder in folderList)
            {
                ToDoItemList = GetItems(folder);
            }
        }
    }
    [RelayCommand(CanExecute = nameof(CanAddItem))]
    void AddItem()
    {
        if (string.IsNullOrWhiteSpace(TextBoxItemEntry) == false)
        {
            ToDoItemList.Add(new TaskItemViewModel(TextBoxItemEntry));
            TextBoxItemEntry = null;
            SaveContents(Settings.ToDoDataLocation);
        }
    }
    [RelayCommand]
    void RemoveItem(TaskItemViewModel item)
    {
        if (ToDoItemList.Remove(item))
        {
            SaveContents(Settings.ToDoDataLocation);
        }
    }
    bool CanAddItem()
    {
        return string.IsNullOrWhiteSpace(TextBoxItemEntry) == false;
    }

    //TODO: Update this method when folders are properly implemented.
    static ObservableCollection<TaskItemViewModel> GetItems(TaskFolder folder)
    {
        ObservableCollection<TaskItemViewModel> itemList = [];
        foreach (TaskItem item in folder.Item)
        {
            itemList.Add(new(item));
        }
        foreach (TaskFolder subFolder in folder.Folder)
        {
            ObservableCollection<TaskItemViewModel> subItemList = GetItems(subFolder);
            foreach (TaskItemViewModel subItem in subItemList)
            {
                itemList.Add(subItem);
            }
        }
        return itemList;
    }
    //TODO: Update this method when folders are properly implemented.
    static TaskFolder ConvertItemsToFolder(ObservableCollection<TaskItemViewModel> items)
    {
        return new("To Do List", [.. items.Select(x => x.Item)]);
    }
    void SaveContents(string? pathToFile)
    {
        ParseArchive fileParser = new();
        string? relativePath = Path.GetDirectoryName(pathToFile);
        string? fileName = Path.GetFileNameWithoutExtension(Settings.ToDoDataLocation);
        if (relativePath != null && fileName != null)
        {
            fileParser.Export(relativePath, fileName, ConvertItemsToFolder(ToDoItemList));
        }
    }
}