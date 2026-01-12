using System;
using System.Collections.Generic;
using System.IO;
using LotsToDo.Backend.ToDoData;

namespace LotsToDo.Backend.FileIO.ToDoFileFormats;

public class ParseToDoTxt : IParseToDo
{
    
    public void Export(string relativePath, string fileName, params TaskFolder[] folders)
    {
        Directory.CreateDirectory(relativePath);
        using (File.Create(relativePath + "/" + fileName + ".txt")) ;
    }

    public bool Import(string relativePath, string fileName, out List<TaskFolder> folders)
    {
        throw new NotImplementedException();
    }
}
