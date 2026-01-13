using System.Collections.Generic;
using LotsToDo.Backend.ToDoData;

namespace LotsToDo.Backend.FileIO.ToDoFileFormats;

public interface IParseToDo
{
    /// <summary>
    /// Exports the app's to do contents into a file format.
    /// </summary>
    /// <param name="relativePath"></param>
    /// <param name="fileName"></param>
    /// <param name="folder"></param>
    /// <returns>A boolean value that determines if the file exists and is overwritten.</returns>

    public bool Export(string relativePath, string fileName, params TaskFolder[] folders);

    /// <summary>
    /// Imports the contents of the to do file and converts those contents into <see cref="TaskFolder"/>
    /// </summary>
    /// <param name="relativePath"></param>
    /// <param name="fileName"></param>
    /// <param name="folder"></param>
    /// <returns>A boolean value that determines if the file exists.</returns>
    public bool Import(string relativePath, string fileName, out List<TaskFolder> folders);
}
