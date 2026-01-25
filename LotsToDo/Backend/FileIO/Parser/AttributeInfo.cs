using System.Collections.Generic;

namespace LotsToDo.Backend.FileIO.Parser;

public readonly struct AttributeInfo(string attributeName, List<string> bypassKeywordList, List<string> attributeFormat, ParseDirection textDirection)
{
    public string AttributeName { get; } = attributeName;
    public List<string> BypassKeywordList { get; } = bypassKeywordList;
    public List<string> AttributeFormat { get; } = attributeFormat;
    public ParseDirection TextDirection { get; } = textDirection;
}