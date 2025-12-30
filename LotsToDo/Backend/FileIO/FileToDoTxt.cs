using System;
using System.Collections.Generic;
using System.IO;

namespace LotsToDo.Backend.FileIO;

public class FileToDoTxt : IToDoFileIO
{
    
    public void Export(string relativePath, string fileName, params ToDoFolder[] folders)
    {
        Directory.CreateDirectory(relativePath);
        using (File.Create(relativePath + "/" + fileName + ".txt")) ;
    }

    public bool Import(string relativePath, string fileName, out List<ToDoFolder> folders)
    {
        throw new NotImplementedException();
    }
}
