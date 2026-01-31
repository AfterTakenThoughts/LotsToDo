using System.Collections.Generic;

namespace LotsToDo.Backend.FileIO.Parser.ParserMethods;
public interface ITagParser
{
    public string TagName { get; init; }
    public List<IAttributeInfo> KeyWordList { get; set; }
}
