using System.IO;

namespace LotsToDo.Backend;

public interface IToDoFileIO
{
    public void Export(ToDoFolder folder, string relativePath, string fileName);
    public ToDoFolder Import(string relativePathToFile);
}
public class FileToDoTxt : IToDoFileIO
{
    public void Export(ToDoFolder folder, string relativePath, string fileName)
    {
        Directory.CreateDirectory(relativePath);
        File.Create(relativePath + "/" + fileName + ".txt");
    }

    public ToDoFolder Import(string relativePathToFile)
    {
        throw new System.NotImplementedException();
    }
}
public class FileArchive : IToDoFileIO
{
    public void Export(ToDoFolder folder, string relativePath, string fileName)
    {
        Directory.CreateDirectory(relativePath);
        File.AppendAllText(relativePath + "/" + fileName + ".txt", folder.ToString());
    }

    public ToDoFolder Import(string relativePathToFile)
    {
        throw new System.NotImplementedException();
    }
}