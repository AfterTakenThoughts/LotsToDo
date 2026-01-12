namespace LotsToDo.Backend;

public class Settings(string toDoDataLocation)
{
    public string ToDoDataLocation { get; set; } = toDoDataLocation;
}