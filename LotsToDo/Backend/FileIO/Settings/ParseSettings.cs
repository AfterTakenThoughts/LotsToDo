namespace LotsToDo.Backend.FileIO.Settings;
public static class ParseSettings
{
    //TODO: import from a file rather than on a default hardcoded value.
    public static void Import(string relativePath, string fileName)
    {
        Backend.Settings.ToDoDataLocation = "Data/ToDoList.txt";
    }
}